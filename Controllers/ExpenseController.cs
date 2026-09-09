using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExpenseController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Expense
        public async Task<IActionResult> Index(
    string search,
    string category,
    DateTime? fromDate,
    DateTime? toDate)
        {
            var userId = _userManager.GetUserId(User);

            var query = _context.Expenses
                .Where(e => e.UserId == userId)
                .AsQueryable();

            // Search by description
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e =>
                    e.Description.Contains(search));
            }

            // Filter by category
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e =>
                    e.Category == category);
            }

            // Filter from date
            if (fromDate.HasValue)
            {
                query = query.Where(e =>
                    e.Date >= fromDate.Value);
            }

            // Filter to date
            if (toDate.HasValue)
            {
                query = query.Where(e =>
                    e.Date <= toDate.Value);
            }

            var expenses = await query
                .OrderByDescending(e => e.Date)
                .ThenByDescending(e => e.Id)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(expenses);
        }

        // GET: /Expense/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Expense/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (expense.Date.Date > DateTime.Today)
            {
                ModelState.AddModelError(
                    nameof(expense.Date),
                    "Expense date cannot be in the future.");
            }
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                expense.UserId = userId!;

                _context.Expenses.Add(expense);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(expense);
        }
        // GET: /Expense/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e =>
                    e.Id == id &&
                    e.UserId == userId);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: /Expense/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            if (expense.Date.Date > DateTime.Today)
            {
                ModelState.AddModelError(
                    nameof(expense.Date),
                    "Expense date cannot be in the future.");
            }

            if (id != expense.Id)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var existingExpense = await _context.Expenses
                .FirstOrDefaultAsync(e =>
                    e.Id == id &&
                    e.UserId == userId);

            if (existingExpense == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                existingExpense.Description = expense.Description;
                existingExpense.Amount = expense.Amount;
                existingExpense.Category = expense.Category;
                existingExpense.Date = expense.Date;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(expense);
        }
        // GET: /Expense/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e =>
                    e.Id == id &&
                    e.UserId == userId);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }


        // POST: /Expense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e =>
                    e.Id == id &&
                    e.UserId == userId);

            if (expense != null)
            {
                _context.Expenses.Remove(expense);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
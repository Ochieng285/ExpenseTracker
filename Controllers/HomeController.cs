using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Public landing page
        public IActionResult Index()
        {
            return View();
        }

        // Protected dashboard
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var today = DateTime.Today;

            var firstDayOfMonth = new DateTime(
                today.Year,
                today.Month,
                1
            );

            var userId = _userManager.GetUserId(User);

            var userExpenses = _context.Expenses
                .Where(e => e.UserId == userId);

            var monthlyExpenses = await userExpenses
                .OrderBy(e => e.Date)
                .Select(e => new
                {
                    e.Date,
                    e.Amount
                })
                .ToListAsync();

            var monthlySummaries = monthlyExpenses
                .GroupBy(e => new
                {
                    e.Date.Year,
                    e.Date.Month
                })
                .Select(g => new MonthlySummary
                {
                    Month = new DateTime(
                        g.Key.Year,
                        g.Key.Month,
                        1
                    ).ToString("MMM yyyy"),

                    Total = g.Sum(e => e.Amount)
                })
                .ToList();

            var dashboard = new DashboardViewModel
            {
                TotalExpenses = await userExpenses
                    .SumAsync(e => (decimal?)e.Amount) ?? 0,

                TodayExpenses = await userExpenses
                    .Where(e => e.Date.Date == today)
                    .SumAsync(e => (decimal?)e.Amount) ?? 0,

                MonthlyExpenses = await userExpenses
                    .Where(e => e.Date >= firstDayOfMonth)
                    .SumAsync(e => (decimal?)e.Amount) ?? 0,

                ExpenseCount = await userExpenses.CountAsync(),

                CategorySummaries = await userExpenses
                    .GroupBy(e => e.Category)
                    .Select(g => new CategorySummary
                    {
                        Category = g.Key,
                        Total = g.Sum(e => e.Amount)
                    })
                    .OrderByDescending(x => x.Total)
                    .ToListAsync(),

                RecentExpenses = await userExpenses
                    .OrderByDescending(e => e.Date)
                    .ThenByDescending(e => e.Id)
                    .Take(5)
                    .ToListAsync(),

                MonthlySummaries = monthlySummaries
            };

            return View(dashboard);
        }
    }
}
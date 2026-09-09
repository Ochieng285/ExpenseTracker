using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ExpenseTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Expense> Expenses { get; set; }
            = new List<Expense>();
    }
}
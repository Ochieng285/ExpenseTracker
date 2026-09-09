namespace ExpenseTracker.Models
{
    public class DashboardViewModel
    {
        public decimal TotalExpenses { get; set; }

        public decimal TodayExpenses { get; set; }

        public decimal MonthlyExpenses { get; set; }

        public int ExpenseCount { get; set; }

        public List<CategorySummary> CategorySummaries { get; set; }
            = new List<CategorySummary>();

        public List<Expense> RecentExpenses { get; set; }
            = new List<Expense>();

        public List<MonthlySummary> MonthlySummaries { get; set; }
            = new List<MonthlySummary>();
    }

    public class CategorySummary
    {
        public string Category { get; set; } = string.Empty;

        public decimal Total { get; set; }
    }

    public class MonthlySummary
    {
        public string Month { get; set; } = string.Empty;

        public decimal Total { get; set; }
    }
}
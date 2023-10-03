namespace WeeklyBudget.Models
{
	/// <summary>
	/// Budget class represents a blue print for the planned total expenditure during certain month
	/// </summary>
	public class Budget
    {
        public int BudgetId { get; set; }
        public DateTime BudgetDate { get; set; } = DateTime.Now;
        public decimal TotalBudget { get; set; } = default;
        public ICollection<BudgetDetail>? BudgetDetails { get; set; } = new List<BudgetDetail>();
    }
}

namespace WeeklyBudget.Models
{
	/// <summary>
	/// Budget class represents a blue print for the planned total expenditure during certain month
	/// </summary>
	public class Budget
	{
		public int BudgetId { get; set; }
		/// <summary>
		/// It is a day in each month from which budget starts, it is meant to be a day when the user
		/// receives the salary.
		/// Default value is 15th every month, this more or less common salary day I think :-), or 
		/// </summary>
		public DateTime BudgetDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);
		public decimal TotalBudget { get; set; } = default;
		public ICollection<BudgetDetail>? BudgetDetails { get; set; } = new List<BudgetDetail>();
	}
}

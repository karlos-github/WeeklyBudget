namespace WeeklyBudget.Models
{
	/// <summary>
	/// Budget class represents a blue print for the planned total expenditure during certain month
	/// </summary>
	public class Budget
	{
		public int BudgetId { get; set; }
		//public int SalaryDay { get; set; } = 15; //This default value is more or less common salary day I think :-)
		//public DateTime StartBudgetDate { get; set; }
		//public DateTime EndBudgetDate { get; set; }
		public DateTime BudgetDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15); //15th every month, this default value is more or less common salary day I think :-)
		public decimal TotalBudget { get; set; } = default;
		public ICollection<BudgetDetail>? BudgetDetails { get; set; } = new List<BudgetDetail>();
	}
}

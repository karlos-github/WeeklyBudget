using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeeklyBudget.Models
{
	/// <summary>
	/// BudgetDetail class represents a budget's one item expenditure type blue print. Simply saying represents how much
	/// gonna be planned to be spent during one month period for the certain expenditure type item.
	/// </summary>
	public class BudgetDetail
	{
		public int BudgetDetailId { get; set; }
		public int BudgetId { get; set; }
		public Budget? Budget { get; set; }
		public int ExpenditureTypeId { get; set; }
		public decimal TotalBudget { get; set; }
	}
}

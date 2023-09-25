using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeeklyBudget.Models
{
	public class Expenditure
	{
		public int ExpenditureId { get; set; }
		public DateTime SpentDate { get; set; } = DateTime.Now;
		public int ExpenditureTypeId { get; set; }
		public decimal SpentAmount { get; set; }
	}
}

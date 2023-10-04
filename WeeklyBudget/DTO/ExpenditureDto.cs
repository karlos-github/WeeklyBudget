using WeeklyBudget.Models;

namespace WeeklyBudget.DTO
{
	public class ExpenditureDto
	{
		public int ExpenditureId { get; set; }
		public ExpenditureType ExpenditureType { get; set; } = new ExpenditureType();
		public AmountDto? Amount { get; set; }
	}
}

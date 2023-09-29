using WeeklyBudget.Models;

namespace WeeklyBudget.DTO
{
	public class ExpenditureDto
	{
		public ExpenditureType ExpenditureType { get; set; } = new ExpenditureType();
		public AmountDto? Amount { get; set; }
		public int? ExpenditureId { get; set; }
	}
}

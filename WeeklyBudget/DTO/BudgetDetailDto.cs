namespace WeeklyBudget.DTO
{
	public class BudgetDetailDto
	{
		public int ExpenditureTypeId { get; set; }
		public string ExpenditureTypeName { get; set; } = string.Empty;
		public decimal TotalBudget { get; set; } = default;
	}
}

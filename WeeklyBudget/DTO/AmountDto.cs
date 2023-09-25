namespace WeeklyBudget.DTO
{
	public class AmountDto
	{		
		public decimal TotalBudget { get; set; } = default;
		public decimal LeftToSpentPercent { get; set; } = 100;
		public decimal SpentPercent { get; set; } = default;
		public decimal LeftToSpentAmount { get; set; } = default;
		public decimal SpentAmount { get; set; } = default;
	}
}

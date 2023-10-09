namespace WeeklyBudget.DTO
{
	public class BudgetDto
	{
		public string BudgetDate { get; set; } = string.Empty;
		public int SalaryDay { get; set; }
		public AmountDto? MonthlyAmount { get; set; }
		public IEnumerable<ExpenditureDto> MonthlyExpenditures { get; set;} = new List<ExpenditureDto>();
		public IEnumerable<WeeklyExpenditureDto> WeeklyExpenditures { get; set; } = new List<WeeklyExpenditureDto>();
	}
}

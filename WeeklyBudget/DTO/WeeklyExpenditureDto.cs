namespace WeeklyBudget.DTO
{
	public class WeeklyExpenditureDto
	{
		public string WeekDescription { get; set; } = string.Empty;
		public IEnumerable<ExpenditureDto> Expenditures { get; set; } = new List<ExpenditureDto>();
	}
}

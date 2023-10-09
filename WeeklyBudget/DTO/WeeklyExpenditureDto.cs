namespace WeeklyBudget.DTO
{
	public class WeeklyExpenditureDto
	{
		public string WeekDescription { get; set; } = string.Empty;
		public DateTime From {get; set;} = DateTime.MinValue;
		public DateTime To { get; set; } = DateTime.MinValue;
		public IEnumerable<ExpenditureDto> Expenditures { get; set; } = new List<ExpenditureDto>();
	}
}

using WeeklyBudget.DTO;

namespace WeeklyBudget.Contracts
{
	public interface IExpenditureService
	{
		Task<ExpenditureDto> GetExpenditureAsync();
		Task SaveExpenditureAsync(ExpenditureDto dto);
		Task DeleteExpenditureAsync(int id);
		Task ExpenditureAsync(int id,  ExpenditureDto dto);

	}
}

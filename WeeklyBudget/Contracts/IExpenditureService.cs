using WeeklyBudget.DTO;

namespace WeeklyBudget.Contracts
{
	public interface IExpenditureService
	{
		Task<IEnumerable<ExpenditureDto>> GetAllAsync();
		Task<bool> DeleteAsync(int id);
		Task<bool> SaveAsync(int expenditureTypeId, decimal amount);
	}
}

using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
	public interface IExpenditureTypeService
	{
		Task<ExpenditureType?> GetByIdAsync(int id);
		Task<IEnumerable<ExpenditureType>> GetAllAsync();
		Task SaveAsync(string expenditureTypeName);
		Task<bool> DeleteAsync(int id);
	}
}

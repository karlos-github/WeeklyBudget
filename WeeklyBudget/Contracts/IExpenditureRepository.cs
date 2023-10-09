using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
    public interface IExpenditureRepository
    {
		Task<IEnumerable<Expenditure>> GetAllAsync(DateTime dateTime);
		Task<Expenditure?> GetByIdAsync(int id);
		Task<bool> SaveAsync(Expenditure dto);
		Task<bool> DeleteAsync(Expenditure dto);
	}
}

using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
    public interface IExpenditureRepository
    {
		Task<IEnumerable<Expenditure>> GetAllAsync(DateTime dateTime);
		Task<bool> SaveAsync(Expenditure dto);
		Task<bool> UpdateAsync(Expenditure dto);
		Task<bool> DeleteAsync(Expenditure dto);
	}
}

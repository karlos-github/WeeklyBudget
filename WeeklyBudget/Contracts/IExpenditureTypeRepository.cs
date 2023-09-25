using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
    public interface IExpenditureTypeRepository
    {
        Task<ExpenditureType?> GetByIdAsync(int id);
        Task<IEnumerable<ExpenditureType>> GetAllAsync();
        Task<bool> SaveAsync(ExpenditureType dto);
        Task<bool> DeleteAsync(ExpenditureType dto);
    }
}

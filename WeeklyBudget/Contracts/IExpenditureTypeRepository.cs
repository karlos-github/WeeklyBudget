using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
    public interface IExpenditureTypeRepository
    {
        //Task<List<Budget>> GetAllBudgetsAsync(bool trackChanges);
        Task<ExpenditureType?> GetByIdAsync(int id);
        Task<List<ExpenditureType>> GetAllAsync();
        //void CreateBudget(Budget budget);
        //Task<Budget?> GetActualBudgetAsync();
        //Task<bool> ExistActualBudgetAsync();
    }
}

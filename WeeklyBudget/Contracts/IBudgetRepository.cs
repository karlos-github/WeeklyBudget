using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
    public interface IBudgetRepository
    {
        Task<List<Budget>> GetAllBudgetsAsync(bool trackChanges);
        Task<Budget?> GetByIdAsync(int id, bool trackChanges);
        Task CreateBudgetAsync(Budget budget);
        Task<Budget?> GetActualBudgetAsync();
        Task<bool> ExistActualBudgetAsync();
        Task<Budget> GetNewBudgetAsync();
        Budget DefaultBudget();
    }
}

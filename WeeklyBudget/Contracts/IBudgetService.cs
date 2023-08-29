using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
    public interface IBudgetService
    {
        Task<List<Budget>> GetAllBudgetsAsync(bool trackChanges);
        Task<Budget?> GetByIdAsync(int id, bool trackChanges);
        //void CreateBudget(Budget budget);
        Task<Budget?> GetActualBudgetAsync();
        Task<bool> ExistActualBudgetAsync();
        Task<BudgetDto?> GetActualBudgetAsync1();
        Task SaveBudgetDefinitionAsync(BudgetDefinitionDto budget);
    }
}

using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
    public interface IBudgetRepository
    {
        Task CreateBudgetAsync(Budget budget);
        Task<Budget?> GetActualBudgetAsync(bool deep);
        Task<bool> UpdateBudgetAsync(Budget budget);
        Task<bool> UpdateBudgetDetailAsync(BudgetDetail budgetDetail);
	}
}

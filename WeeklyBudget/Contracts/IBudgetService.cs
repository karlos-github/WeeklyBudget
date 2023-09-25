using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
    public interface IBudgetService
    {
		Task<BudgetDto_> GetActualBudgetAsync_();
		Task UpdateAsync(BudgetDefinitionDto budget);
	}
}

using WeeklyBudget.DTO;

namespace WeeklyBudget.Contracts
{
	public interface IBudgetService
    {
		Task<BudgetDto_> GetActualBudgetAsync_();
		Task<bool> UpdateAsync(BudgetDefinitionDto budget);
		Task<bool> UpdateAsync(decimal totalBudget);
	}
}

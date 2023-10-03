using WeeklyBudget.DTO;

namespace WeeklyBudget.Contracts
{
	public interface IBudgetService
    {
		Task<BudgetDto> GetActualBudgetAsync();
		//Task<bool> UpdateAsync(BudgetDefinitionDto budget);
		Task<bool> UpdateAsync(decimal totalBudget);
	}
}

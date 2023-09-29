using WeeklyBudget.DTO;

namespace WeeklyBudget.Contracts
{
	public interface IBudgetDetailService
	{
		Task<IEnumerable<BudgetDetailDto>> GetActualBudgetDetailsAsync();
		Task<bool> UpdateAsync(int expenditureId, decimal totalBudget);
	}
}

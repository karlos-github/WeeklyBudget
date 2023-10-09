using WeeklyBudget.DTO;

namespace WeeklyBudget.Contracts
{
	public interface IBudgetService
    {
		Task<BudgetDto> GetActualBudgetAsync();
		Task<bool> UpdateAsync(decimal totalBudget);
		Task<int> GetSalaryDateAsync();
		Task<bool> UpdateSalaryDayAsync(int salaryDay);
	}
}

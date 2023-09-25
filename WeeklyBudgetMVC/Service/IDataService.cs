using WeeklyBudget.DTO;

namespace WeeklyBudgetMVC.Service
{
	public interface IDataService
	{
		Task<BudgetDto> GetAllAsync();
	}
}

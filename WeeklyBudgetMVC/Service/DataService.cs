using WeeklyBudget.DTO;
using WeeklyBudgetMVC.Service;

namespace WeeklyBudgetMVC.DataService
{
	public class DataService : IDataService
	{
        WeeklyBudget.Contracts.IBudgetService _budgetService;
		public async Task<BudgetDto> GetAllAsync()
		{
			await _budgetService.GetAllBudgetsAsync(false);
			throw new NotImplementedException();
		}
	}
}

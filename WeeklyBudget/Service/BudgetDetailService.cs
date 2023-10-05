using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;

namespace WeeklyBudget.Service
{
	public class BudgetDetailService : IBudgetDetailService
	{
		readonly IRepositoryManager _repositoryManager;

		public BudgetDetailService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

		/// <summary>
		/// Method returns list of all BudgetDetails that belong to the current Budget
		/// </summary>
		public async Task<IEnumerable<BudgetDetailDto>> GetActualBudgetDetailsAsync()
		{
			var budgetDetails = new List<BudgetDetailDto>();
			var budget = await _repositoryManager.Budgets.GetActualBudgetAsync();
			if (budget != null)
			{
				var allExpenditureTypes = await _repositoryManager.ExpenditureTypes.GetAllAsync() ;
				foreach (var budgetDetail in budget.BudgetDetails!)
				{
					budgetDetails.Add(new BudgetDetailDto()
					{
						ExpenditureTypeName = allExpenditureTypes.FirstOrDefault(_ => _.ExpenditureTypeId == budgetDetail.ExpenditureTypeId)!.Name ?? string.Empty,
						ExpenditureTypeId = budgetDetail.ExpenditureTypeId,
						TotalBudget = budgetDetail.TotalBudget,
					});
				}
			}

			return budgetDetails;
		}

		/// <summary>
		/// Updates budget amount for the certain expenditure type item given for the current monthly budget.
		/// </summary>
		/// <param name="expenditureTypeId">Expenditure type identification</param>
		/// <param name="totalBudget">Total amount of budget planned to spent for the certain expenditure type</param>
		public async Task<bool> UpdateAsync(int expenditureTypeId, decimal totalBudget)
		{
			var budget = await _repositoryManager.Budgets.GetActualBudgetAsync();
			if (budget != null)
			{
				var budgetDetail = budget.BudgetDetails!.FirstOrDefault(_ => _.ExpenditureTypeId == expenditureTypeId);
				budgetDetail!.TotalBudget = totalBudget;
				return await _repositoryManager.Budgets.UpdateBudgetDetailAsync(budgetDetail);
			}
			return false;
		}
	}
}

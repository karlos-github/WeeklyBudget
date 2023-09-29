using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;

namespace WeeklyBudget.Service
{
	public class BudgetDetailService : IBudgetDetailService
	{
		readonly IRepositoryManager _repositoryManager;

		public BudgetDetailService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

		public async Task<IEnumerable<BudgetDetailDto>> GetActualBudgetDetailsAsync()
		{
			var budgetDetails = new List<BudgetDetailDto>();
			var budget = await _repositoryManager.Budget.GetActualBudgetAsync();
			if (budget != null)
			{
				var allExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync() ;
				foreach (var budgetDetail in budget.BudgetDetails!)
				{
					budgetDetails.Add(new BudgetDetailDto()
					{
						ExpenditureTypeName = allExpenditureTypes.FirstOrDefault(_ => _.ExpenditureTypeId == budgetDetail.ExpenditureTypeId)!.Name ?? string.Empty,
						ExpenditureId = budgetDetail.ExpenditureTypeId,
						TotalBudget = budgetDetail.TotalBudget,
					});
				}
			}

			return budgetDetails;
		}

		public async Task<bool> UpdateAsync(int expenditureId, decimal totalBudget)
		{
			var budget = await _repositoryManager.Budget.GetActualBudgetAsync();
			if (budget != null)
			{
				var budgetDetail = budget.BudgetDetails!.FirstOrDefault(_ => _.ExpenditureTypeId == expenditureId);
				budgetDetail!.TotalBudget = totalBudget;
				return await _repositoryManager.Budget.UpdateBudgetDetailAsync(budgetDetail);
			}
			return false;
		}
	}
}

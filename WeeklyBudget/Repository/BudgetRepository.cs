using Microsoft.EntityFrameworkCore;
using WeeklyBudget.Contracts;
using WeeklyBudget.Context;
using WeeklyBudget.Models;

namespace WeeklyBudget.Repository
{
	public class BudgetRepository : RepositoryBase<Budget>, IBudgetRepository
	{
		public BudgetRepository(WeeklyBudgetContext weeklyBudgetContext) : base(weeklyBudgetContext)
		{

		}

		public async Task CreateBudgetAsync(Budget budget)
		{
			await _budgetContext.Budgets.AddAsync(budget);
			await _budgetContext.SaveChangesAsync();
		}

		public async Task<bool> UpdateBudgetAsync(Budget budget)
		{
			_budgetContext.Entry<Budget>(budget).State = EntityState.Modified;
			return await _budgetContext.SaveChangesAsync() != default;
		}

		public async Task<bool> UpdateBudgetDetailAsync(BudgetDetail budgetDetail)
		{
			_budgetContext.BudgetDetails.Update(budgetDetail);
			return await _budgetContext.SaveChangesAsync() != default;
		}

		public async Task<Budget?> GetActualBudgetAsync()
			=> await _budgetContext.Budgets
				.Include(_ => _.BudgetDetails)
				.FirstOrDefaultAsync(_ => _.BudgetDate.Month == DateTime.Now.Month);
	}
}

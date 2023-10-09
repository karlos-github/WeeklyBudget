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

		/// <summary>
		/// The actual budget is a very first record in the Budget table which has BudgetDate included in the interval.
		/// Deep parameter = true => returns BudgetDetails, otherwise false
		/// </summary>
		/// <returns>Returns Budget record</returns>
		public async Task<Budget?> GetActualBudgetAsync(bool deep)
			=> 
			deep 
				? await _budgetContext.Budgets
					.Include(_ => _.BudgetDetails)
					.FirstOrDefaultAsync(_ => _.BudgetDate <= DateTime.Now.Date && DateTime.Now.Date <= _.BudgetDate.AddMonths(1).AddDays(-1))
				: await _budgetContext.Budgets
					.FirstOrDefaultAsync(_ => _.BudgetDate <= DateTime.Now.Date && DateTime.Now.Date <= _.BudgetDate.AddMonths(1).AddDays(-1));
	}
}

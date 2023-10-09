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


		//TODO-KS-how to check if the current budget exists?
		//void Test()
		//{
		//	var upperBoundery = _budgetContext.Budgets
		//		.Include(_ => _.BudgetDetails)
		//		.FirstOrDefaultAsync(_ => new DateTime(_.BudgetDate.Year, _.BudgetDate.Month, _.SalaryDay) <= DateTime.Now.Date && DateTime.Now.Date <= new DateTime(_.BudgetDate.Year, _.BudgetDate.Month, _.SalaryDay - 1));
		//}

		/// <summary>
		/// The actual budget is a very first record in the Budget table which has BudgetDate included in the interval 
		/// </summary>
		/// <returns>Returns Budget record</returns>
		public async Task<Budget?> GetActualBudgetAsync()
			=> await _budgetContext.Budgets
				.Include(_ => _.BudgetDetails)
				.FirstOrDefaultAsync(_ => _.BudgetDate <= DateTime.Now.Date && DateTime.Now.Date <= _.BudgetDate.AddMonths(1).AddDays(-1));


		//	=> await _budgetContext.Budgets
		//.Include(_ => _.BudgetDetails)
		//.FirstOrDefaultAsync(_ => new DateTime(_.BudgetDate.Year, _.BudgetDate.Month, _.SalaryDay) <= DateTime.Now.Date
		//	&& DateTime.Now.Date <= new DateTime(_.BudgetDate.Year, _.BudgetDate.Month, _.SalaryDay).AddMonths(1).AddDays(-1));
	}
}

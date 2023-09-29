using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using WeeklyBudget.Contracts;
using WeeklyBudget.Data;
using WeeklyBudget.Models;

namespace WeeklyBudget.Repositories
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

		public async Task<bool> UpdateBudgetDetailsAsync(IEnumerable<BudgetDetail> budgetDetails)
		{
			_budgetContext.BudgetDetails.UpdateRange(budgetDetails);
			//_budgetContext.Entry<Budget>(budget).State = EntityState.Modified;
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

		readonly Func<DateTime, bool> isCurrentWeek = d =>
		{
			var currentDayOfWeek = (int)d.DayOfWeek;
			var thisWeekStart = d.AddDays(-(int)d.DayOfWeek);
			return d.AddDays(-(--currentDayOfWeek)).Date <= d.Date && d.Date <= thisWeekStart.AddDays(7).AddSeconds(-1);
		};

		/*
        https://stackoverflow.com/questions/5376421/ef-including-other-entities-generic-repository-pattern/5376637#5376637
        https://stackoverflow.com/questions/6791591/how-to-brings-the-entity-framework-include-extention-method-to-a-generic-iquerya?noredirect=1&lq=1
        https://stackoverflow.com/questions/74140462/implementing-repositories-with-ef-core-without-creating-multiples-methods
        https://stackoverflow.com/questions/74315851/ef-core-generic-repository-include-nested-navigation-properties
        google search "net core repository pattern set include on dbset"
        */
	}
}

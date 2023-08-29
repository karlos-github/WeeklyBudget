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
            await base.Create(budget);
            //CreateBudget(budget);
        }
            

        public async Task<List<Budget>> GetAllBudgetsAsync(bool trackChanges) =>
            trackChanges
                ? await _budgetContext.Budgets.IgnoreAutoIncludes().ToListAsync()
                : await _budgetContext.Budgets.IgnoreAutoIncludes().ToListAsync();

        public async Task<Budget?> GetByIdAsync(int id, bool trackChanges) =>
            trackChanges
                ? await _budgetContext.Budgets.FirstOrDefaultAsync(_ => _.Id == id)
                : await _budgetContext.Budgets.IgnoreAutoIncludes().FirstOrDefaultAsync(_ => _.Id == id);

        public async Task<Budget?> GetActualBudgetAsync()
        {
            var budgets = await _budgetContext.Budgets.ToListAsync()/*.Where(_ => isCurrentWeek(_.BudgetDate))*/;
            //await _budgetContext.Budgets.FirstOrDefaultAsync(_ => isCurrentWeek(_.BudgetDate));
            var currentBudget = budgets.FirstOrDefault(_ => isCurrentWeek(_.BudgetDate));
            return budgets.FirstOrDefault(_ => isCurrentWeek(_.BudgetDate));
        }

        async Task<Budget?> GetPreviousBudgetAsync()
        {
            var budgets = await _budgetContext.Budgets.ToListAsync()/*.Where(_ => isCurrentWeek(_.BudgetDate))*/;
            //await _budgetContext.Budgets.FirstOrDefaultAsync(_ => isCurrentWeek(_.BudgetDate));
            var currentBudget = budgets.FirstOrDefault(_ => isCurrentWeek(_.BudgetDate));
            return budgets.FirstOrDefault(_ => isCurrentWeek(_.BudgetDate));
        }

        public async Task<bool> ExistActualBudgetAsync() =>
            await _budgetContext.Budgets.IgnoreAutoIncludes().AnyAsync(_ => isCurrentWeek(_.BudgetDate));

        public async Task<Budget> GetNewBudgetAsync()
        {
            //var budgets = await _budgetContext.Budgets.ToListAsync();
            //budgets?.FirstOrDefault(_ => _.BudgetDate.Date.Month == DateTime.Now.Month) ?? new Budget();
            return new Budget();
        }

        public Budget DefaultBudget()
        {
            return new Budget()
            {
                BudgetDate = DateTime.Now,
                TotalBudget = 0,
                BudgetDetails = new List<BudgetDetail>() 
                { 
                new BudgetDetail() 
                {
                
                }
                },
            };
        }

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

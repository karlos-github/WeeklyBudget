using WeeklyBudget.Contracts;
using WeeklyBudget.Context;

namespace WeeklyBudget.Repository
{
#pragma warning disable CS8618
	public class RepositoryManager : IRepositoryManager
    {
        readonly WeeklyBudgetContext _weeklyBudgetContext;
        IBudgetRepository _budgetRepository;
        IExpenditureTypeRepository _expenseTypeRepository;
        IExpenditureRepository _expenditureRepository;

        public RepositoryManager(WeeklyBudgetContext weeklyBudgetContext) => _weeklyBudgetContext = weeklyBudgetContext;
        public IBudgetRepository Budgets => _budgetRepository ??= new BudgetRepository(_weeklyBudgetContext);
        public IExpenditureTypeRepository ExpenditureTypes => _expenseTypeRepository ??= new ExpenditureTypeRepository(_weeklyBudgetContext);
        public IExpenditureRepository Expenditures => _expenditureRepository ??= new ExpenditureRepository(_weeklyBudgetContext);
    }
#pragma warning restore CS8618
}

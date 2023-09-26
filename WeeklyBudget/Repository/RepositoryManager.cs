using WeeklyBudget.Contracts;
using WeeklyBudget.Data;

namespace WeeklyBudget.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        readonly WeeklyBudgetContext _weeklyBudgetContext;
        IBudgetRepository _budgetRepository;
        IExpenditureTypeRepository _expenseTypeRepository;
        IExpenditureRepository _expenditureRepository;

        public RepositoryManager(WeeklyBudgetContext weeklyBudgetContext) => _weeklyBudgetContext = weeklyBudgetContext;

        public IBudgetRepository Budget => _budgetRepository ??= new BudgetRepository(_weeklyBudgetContext);
        public IExpenditureTypeRepository ExpenditureType => _expenseTypeRepository ??= new ExpenditureTypeRepository(_weeklyBudgetContext);
        public IExpenditureRepository ExpenditureRepository => _expenditureRepository ??= new ExpenditureRepository(_weeklyBudgetContext);
    }
}

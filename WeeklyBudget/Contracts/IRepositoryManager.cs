namespace WeeklyBudget.Contracts
{
    public interface IRepositoryManager
    {
        IBudgetRepository Budgets { get; }
        IExpenditureTypeRepository ExpenditureTypes { get; }
		IExpenditureRepository Expenditures { get; }
    }
}

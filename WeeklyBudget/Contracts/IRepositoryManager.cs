namespace WeeklyBudget.Contracts
{
    public interface IRepositoryManager
    {
        IBudgetRepository Budget { get; }
        IExpenditureTypeRepository ExpenditureType { get; }
		IExpenditureRepository ExpenditureRepository { get; }
    }
}

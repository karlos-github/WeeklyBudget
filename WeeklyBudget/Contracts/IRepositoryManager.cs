namespace WeeklyBudget.Contracts
{
    public interface IRepositoryManager
    {
        IBudgetRepository Budget { get; }
        IExpenditureTypeRepository ExpenditureType { get; }
        IUserRepository User { get; }
        
        void Save();
    }
}

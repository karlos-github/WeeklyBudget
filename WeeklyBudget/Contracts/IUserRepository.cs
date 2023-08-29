using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
    }
}

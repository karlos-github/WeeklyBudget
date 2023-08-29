using Microsoft.EntityFrameworkCore;
using WeeklyBudget.Contracts;
using WeeklyBudget.Data;
using WeeklyBudget.Models;

namespace WeeklyBudget.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(WeeklyBudgetContext weeklyBudgetContext) : base(weeklyBudgetContext)
        {

        }

        public Task<List<User>> GetAllAsync() => _budgetContext.Users.ToListAsync();

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

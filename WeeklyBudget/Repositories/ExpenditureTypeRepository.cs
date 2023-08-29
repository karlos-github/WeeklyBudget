using Microsoft.EntityFrameworkCore;
using WeeklyBudget.Contracts;
using WeeklyBudget.Data;
using WeeklyBudget.Models;

namespace WeeklyBudget.Repositories
{
    public class ExpenditureTypeRepository : RepositoryBase<ExpenditureType>, IExpenditureTypeRepository
    {
        public ExpenditureTypeRepository(WeeklyBudgetContext weeklyBudgetContext) : base(weeklyBudgetContext)
        {

        }

        public async Task<List<ExpenditureType>> GetAllAsync() => await _budgetContext.ExpenditureTypes.ToListAsync();

        public async Task<ExpenditureType?> GetByIdAsync(int id) => await _budgetContext.ExpenditureTypes.FirstOrDefaultAsync(_ => _.Id == id);
    }
}

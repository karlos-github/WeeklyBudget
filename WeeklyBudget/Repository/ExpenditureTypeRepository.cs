using Microsoft.EntityFrameworkCore;
using WeeklyBudget.Contracts;
using WeeklyBudget.Data;
using WeeklyBudget.Models;

namespace WeeklyBudget.Repository
{
	public class ExpenditureTypeRepository : RepositoryBase<ExpenditureType>, IExpenditureTypeRepository
	{
		public ExpenditureTypeRepository(WeeklyBudgetContext weeklyBudgetContext) : base(weeklyBudgetContext)
		{

		}

		public async Task<IEnumerable<ExpenditureType>> GetAllAsync() => await _budgetContext.ExpenditureTypes.ToListAsync() ?? new List<ExpenditureType>();

		public async Task<ExpenditureType?> GetByIdAsync(int id) => await _budgetContext.ExpenditureTypes.FirstOrDefaultAsync(_ => _.ExpenditureTypeId == id);

		public async Task<bool> SaveAsync(ExpenditureType dto)
		{
			await _budgetContext.ExpenditureTypes.AddAsync(dto);
			return await _budgetContext.SaveChangesAsync() != default;
		}

		public async Task<bool> DeleteAsync(ExpenditureType dto) 
		{ 
			_budgetContext.ExpenditureTypes.Entry(dto).State = EntityState.Deleted;
			return await _budgetContext.SaveChangesAsync() != default;
		}
	}
}

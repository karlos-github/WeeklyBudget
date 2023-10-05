using Microsoft.EntityFrameworkCore;
using WeeklyBudget.Contracts;
using WeeklyBudget.Context;
using WeeklyBudget.Models;

namespace WeeklyBudget.Repository
{
	public class ExpenditureRepository : RepositoryBase<Expenditure>, IExpenditureRepository
	{
		public ExpenditureRepository(WeeklyBudgetContext weeklyBudgetContext) : base(weeklyBudgetContext)
		{

		}

		public async Task<bool> DeleteAsync(Expenditure dto)
		{
			_budgetContext.Entry<Expenditure>(dto).State = EntityState.Deleted;
			return await _budgetContext.SaveChangesAsync() != default;
		}

		public async Task<IEnumerable<Expenditure>> GetAllAsync(DateTime dateTime) =>
			await _budgetContext
				.Expenditures
				.Where(_ => _.SpentDate.Month == dateTime.Month)
				.ToListAsync();

		public async Task<Expenditure?> GetByIdAsync(int id) => await _budgetContext.Expenditures.FirstOrDefaultAsync(_ => _.ExpenditureId == id);

		public async Task<bool> SaveAsync(Expenditure dto)
		{
			await _budgetContext.Expenditures.AddAsync(dto);
			return await _budgetContext.SaveChangesAsync() != default;
		}
	}
}

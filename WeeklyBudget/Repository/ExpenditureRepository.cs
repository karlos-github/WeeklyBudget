using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using WeeklyBudget.Contracts;
using WeeklyBudget.Data;
using WeeklyBudget.Models;

namespace WeeklyBudget.Repositories
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

		//public async Task<bool> UpdateAsync(Expenditure dto)
		//{
		//	throw new NotImplementedException();
		//}
	}
}

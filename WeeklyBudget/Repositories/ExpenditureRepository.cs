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

		public Task<bool> DeleteAsync(Expenditure dto)
		{
			throw new NotImplementedException();
		}

		public async Task<List<Expenditure>> GetAllAsync(DateTime dateTime) =>
			await _budgetContext
			.Expenditures
			.Where(_ => _.SpentDate.Month == dateTime.Month)
			.ToListAsync();

		public Task<bool> SaveAsync(Expenditure dto)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateAsync(Expenditure dto)
		{
			throw new NotImplementedException();
		}

		Task<IEnumerable<Expenditure>> IExpenditureRepository.GetAllAsync(DateTime dateTime)
		{
			throw new NotImplementedException();
		}
	}
}

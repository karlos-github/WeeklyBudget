using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeeklyBudget.Contracts;
using WeeklyBudget.Data;

namespace WeeklyBudget.Repositories
{
	public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected WeeklyBudgetContext _budgetContext;

        public RepositoryBase(WeeklyBudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
        }

        public async Task Create(T entity)
        {
            await _budgetContext.Set<T>().AddAsync(entity);
            await _budgetContext.SaveChangesAsync(true);
        }

        public void Delete(T entity)
        {
            _budgetContext.Set<T>().Remove(entity);
            _budgetContext.SaveChanges();
        }

        public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
          _budgetContext.Set<T>()
            .Include(x => x)
            .AsNoTracking() :
          _budgetContext.Set<T>()
            .Include(x => x);

        public IQueryable<T> FindAllDeep(bool trackChanges) =>
            !trackChanges ?
             _budgetContext.Set<T>()
               .AsNoTracking() :
             _budgetContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ?
              _budgetContext.Set<T>()
                .Where(expression)
                .AsNoTracking() :
              _budgetContext.Set<T>()
                .Where(expression);

        public void Update(T entity)
        {
            _budgetContext.Set<T>().Attach(entity);
            _budgetContext.Entry(entity).State = EntityState.Modified;
            _budgetContext.SaveChanges();
        }
    }
}

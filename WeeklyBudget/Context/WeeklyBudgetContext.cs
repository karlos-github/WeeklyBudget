using Microsoft.EntityFrameworkCore;
using WeeklyBudget.Models;

namespace WeeklyBudget.Context
{
	public class WeeklyBudgetContext : DbContext
	{
		public WeeklyBudgetContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<Budget> Budgets { get; set; }
		public DbSet<BudgetDetail> BudgetDetails { get; set; }
		public DbSet<Expenditure> Expenditures { get; set; }
		public DbSet<ExpenditureType> ExpenditureTypes { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Budget>()
				.HasMany(a => a.BudgetDetails)
				.WithOne(b => b.Budget)
				.HasForeignKey(b => b.BudgetId)
				.IsRequired();

			builder.Entity<ExpenditureType>()
				.HasMany<Expenditure>()
				.WithOne()
				.HasForeignKey(b => b.ExpenditureTypeId)
				.IsRequired();

			builder.Entity<ExpenditureType>()
			.HasMany<BudgetDetail>()
			.WithOne()
			.HasForeignKey(b => b.ExpenditureTypeId)
			.IsRequired();
		}
	}
}

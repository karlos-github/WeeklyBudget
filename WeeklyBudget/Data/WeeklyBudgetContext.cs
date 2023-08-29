using Microsoft.EntityFrameworkCore;
using WeeklyBudget.Models;
using WeeklyBudget.Models.Configurations;

namespace WeeklyBudget.Data
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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new BudgetConfiguration());
            builder.ApplyConfiguration(new BudgetDetailConfiguration());
            builder.ApplyConfiguration(new ExpenditureConfiguration());
            builder.ApplyConfiguration(new ExpenditureTypeConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());

            builder.Entity<Budget>().Navigation(_ => _.BudgetDetails).AutoInclude();
            builder.Entity<Budget>().Navigation(_ => _.Expenditures).AutoInclude();
        }
    }
}

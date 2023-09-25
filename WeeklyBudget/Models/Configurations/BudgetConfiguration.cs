using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WeeklyBudget.Models.Configurations
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.HasData(
                new Budget()
                {
                    BudgetId = 1,
                    BudgetDate = DateTime.Now,
                    TotalBudget = 18000.0m,
                },
                new Budget()
                {
                    BudgetId = 2,
                    BudgetDate = DateTime.Now,
                    TotalBudget = 12000.0m,
                });

            builder.Property(_ => _.BudgetDate).HasColumnType("date");
			//builder.Property(_ => _.TotalBudget).HasPrecision(6, 2);

			//builder.HasMany(d => d.BudgetDetails)
			//    .WithOne(b => b.Budget)
			//    .HasForeignKey(b => b.BudgetId)
			//    .OnDelete(DeleteBehavior.Cascade);


			
		}
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeklyBudget.Models.Configurations
{
    public class BudgetDetailConfiguration : IEntityTypeConfiguration<BudgetDetail>
    {
        public void Configure(EntityTypeBuilder<BudgetDetail> builder)
        {
            builder.HasData(
                new BudgetDetail()
                {
                    Id = 1,
                    BudgetId = 1,
                    ExpenditureTypeId = 1,
                    TotalBudget = 2000.0m
                },
                new BudgetDetail()
                {
                    Id = 2,
                    BudgetId = 1,
                    ExpenditureTypeId = 2,
                    TotalBudget = 2000.0m
                },
                new BudgetDetail()
                {
                    Id = 3,
                    BudgetId = 1,
                    ExpenditureTypeId = 3,
                    TotalBudget = 4000.0m
                },
                new BudgetDetail()
                {
                    Id = 4,
                    BudgetId = 2,
                    ExpenditureTypeId = 1,
                    TotalBudget = 3000.0m
                },
                new BudgetDetail()
                {
                    Id = 5,
                    BudgetId = 2,
                    ExpenditureTypeId = 2,
                    TotalBudget = 2500.0m
                },
                new BudgetDetail()
                {
                    Id = 6,
                    BudgetId = 2,
                    ExpenditureTypeId = 3,
                    TotalBudget = 5000.0m
                });

            //builder.Property(_ => _.TotalBudget).HasPrecision(6, 2);
        }
    }
}

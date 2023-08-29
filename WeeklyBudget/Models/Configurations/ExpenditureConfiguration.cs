using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeklyBudget.Models.Configurations
{
    public class ExpenditureConfiguration : IEntityTypeConfiguration<Expenditure>
    {
        public void Configure(EntityTypeBuilder<Expenditure> builder)
        {
            builder.HasData(
                new Expenditure()
                {
                    Id = 1,
                    UserId = 1,
                    ExpenditureTypeId = 1,
                    BudgetId = 1,
                    SpentDate = DateTime.Now,
                    SpentAmount = 1000.0m
                },
                new Expenditure()
                {
                    Id = 2,
                    UserId = 1,
                    ExpenditureTypeId = 2,
                    BudgetId = 1,
                    SpentDate = DateTime.Now,
                    SpentAmount = 2000.0m
                },
                new Expenditure()
                {
                    Id = 3,
                    UserId = 2,
                    ExpenditureTypeId = 1,
                    BudgetId = 1,
                    SpentDate = DateTime.Now,
                    SpentAmount = 3000.0m
                },
                new Expenditure()
                {
                    Id = 10,
                    UserId = 1,
                    ExpenditureTypeId = 1,
                    BudgetId = 1,
                    SpentDate = DateTime.Now,
                    SpentAmount = 100.0m
                },
                new Expenditure()
                {
                    Id = 11,
                    UserId = 2,
                    ExpenditureTypeId = 3,
                    BudgetId = 1,
                    SpentDate = DateTime.Now,
                    SpentAmount = 400.0m
                },
                new Expenditure()
                {
                    Id = 4,
                    UserId = 1,
                    BudgetId = 2,
                    SpentDate = DateTime.Now,
                    SpentAmount = 1000.0m
                },
                new Expenditure()
                {
                    Id = 5,
                    UserId = 1,
                    ExpenditureTypeId = 3,
                    BudgetId = 2,
                    SpentDate = DateTime.Now,
                    SpentAmount = 2000.0m
                },
                new Expenditure()
                {
                    Id = 6,
                    UserId = 2,
                    ExpenditureTypeId = 3,
                    BudgetId = 2,
                    SpentDate = DateTime.Now,
                    SpentAmount = 3000.0m
                },
                new Expenditure()
                {
                    Id = 7,
                    UserId = 2,
                    ExpenditureTypeId = 2,
                    BudgetId = 2,
                    SpentDate = DateTime.Now,
                    SpentAmount = 1000.0m
                },
                new Expenditure()
                {
                    Id = 8,
                    UserId = 2,
                    ExpenditureTypeId = 2,
                    BudgetId = 2,
                    SpentDate = DateTime.Now,
                    SpentAmount = 100.0m
                },
                new Expenditure()
                {
                    Id = 9,
                    UserId = 2,
                    ExpenditureTypeId = 1,
                    BudgetId = 2,
                    SpentDate = DateTime.Now,
                    SpentAmount = 150.0m
                });

            builder.Property(_ => _.SpentDate).HasColumnType("date");
            //builder.Property(_ => _.SpentAmount).HasPrecision(5, 2);
        }
    }
}

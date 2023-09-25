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
					BudgetDetailId = 1,
					Budget = new Budget() { BudgetId = 1, },
					ExpenditureTypeId = 1,
					TotalBudget = 2000.0m
				},
				new BudgetDetail()
				{
					BudgetDetailId = 2,
					Budget = new Budget() { BudgetId = 1, },
					ExpenditureTypeId = 2,
					TotalBudget = 2000.0m
				},
				new BudgetDetail()
				{
					BudgetDetailId = 3,
					Budget = new Budget() { BudgetId = 1, },
					ExpenditureTypeId = 3,
					TotalBudget = 4000.0m
				},
				new BudgetDetail()
				{
					BudgetDetailId = 4,
					Budget = new Budget() { BudgetId = 2, },
					ExpenditureTypeId = 1,
					TotalBudget = 3000.0m
				},
				new BudgetDetail()
				{
					BudgetDetailId = 5,
					Budget = new Budget() { BudgetId = 2, },
					ExpenditureTypeId = 2,
					TotalBudget = 2500.0m
				},
				new BudgetDetail()
				{
					BudgetDetailId = 6,
					Budget = new Budget() { BudgetId = 2, },
					ExpenditureTypeId = 3,
					TotalBudget = 5000.0m
				});
		}
	}
}

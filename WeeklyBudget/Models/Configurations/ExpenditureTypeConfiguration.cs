using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeklyBudget.Models.Configurations
{
    public class ExpenditureTypeConfiguration : IEntityTypeConfiguration<ExpenditureType>
    {
        public void Configure(EntityTypeBuilder<ExpenditureType> builder)
        {
            builder.HasData(
                new ExpenditureType()
                {
                    ExpenditureTypeId = 1,
                    Name = "Sex",
                },
                new ExpenditureType()
                {
                    ExpenditureTypeId = 2,
                    Name = "Drogy",
                },
                new ExpenditureType()
                {
                    ExpenditureTypeId = 3,
                    Name = "Alcohol",
                }
            );
        }
    }
}

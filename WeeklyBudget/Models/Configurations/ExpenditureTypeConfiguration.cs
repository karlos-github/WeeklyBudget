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
                    Id = 1,
                    Name = "Sex",
                },
                new ExpenditureType()
                {
                    Id = 2,
                    Name = "Drogy",
                },
                new ExpenditureType()
                {
                    Id = 3,
                    Name = "Alcohol",
                }
            );
        }
    }
}

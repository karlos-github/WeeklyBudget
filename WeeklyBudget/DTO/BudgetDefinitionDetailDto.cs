using WeeklyBudget.Models;

namespace WeeklyBudget.DTO
{
    public class BudgetDefinitionDetailDto
    {
        public ExpenditureType ExpenditureType { get; set; }
        public decimal TotalBudget { get; set; } = default;
    }
}

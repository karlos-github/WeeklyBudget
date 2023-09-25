using WeeklyBudget.Models;

namespace WeeklyBudget.DTO
{
    public class BudgetDefinitionDetailDto
    {
        public int ExpenditureTypeId { get; set; }
        public decimal TotalBudget { get; set; } = default;
    }
}

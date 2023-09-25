using WeeklyBudget.Models;

namespace WeeklyBudget.DTO
{
    public class BudgetDetailDto
    {
        public string ExpenditureTypeName { get; set; } = string.Empty;
        public decimal TotalBudget { get; set; } = default;
        public decimal LeftToSpentPercent { get; set; } = 100;
        public decimal SpentPercent { get; set; } = default;
        public decimal LeftToSpentAmount { get; set; } = default;
        public decimal SpentAmount { get; set; } = default;
    }
}

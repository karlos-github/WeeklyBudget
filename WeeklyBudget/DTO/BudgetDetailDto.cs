using WeeklyBudget.Models;

namespace WeeklyBudget.DTO
{
    public class BudgetDetailDto
    {
        public string UserName { get; set; } = string.Empty;
        public string ExpenditureTypeName { get; set; } = string.Empty;
        public decimal TotalBudget { get; set; } = default;
        public decimal LeftToSpentPercent { get; set; } = default;
        public decimal SpentPercent { get; set; } = default;
        public decimal LeftToSpentAmount { get; set; } = default;
        public decimal SpentAmount { get; set; } = default;
        public IDictionary<int, decimal> UserExpenditures { get; set; } = new Dictionary<int, decimal>();
    }
}

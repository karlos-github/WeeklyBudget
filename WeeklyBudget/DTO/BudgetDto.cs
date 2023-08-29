using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WeeklyBudget.Models;

namespace WeeklyBudget.DTO
{
    public class BudgetDto
    {
        public DateTime BudgetDate { get; set; } = DateTime.MinValue;
        public decimal TotalBudget { get; set; } = default;
        public decimal LeftToSpentPercent { get; set; } = default;
        public decimal SpentPercent { get; set; } = default;
        public decimal LeftToSpentAmount { get; set; } = default;
        public decimal SpentAmount { get; set; } = default;
        public Dictionary<int, List<BudgetDetailDto>>? BudgetDetails { get; set; } /*= new Dictionary<int, List<BudgetDetailDto>>() { { } };*/
    }
}

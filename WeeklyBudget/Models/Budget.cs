using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeeklyBudget.Models
{
    /// <summary>
    /// Budget class represents a blue print for the planned total expenditure during certain month
    /// </summary>
    public class Budget
    {
        public int BudgetId { get; set; }
        public DateTime BudgetDate { get; set; }
        public decimal TotalBudget { get; set; } = default;
        public ICollection<BudgetDetail>? BudgetDetails { get; set; } = new List<BudgetDetail>();
    }
}

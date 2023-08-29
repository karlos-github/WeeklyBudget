using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeeklyBudget.Models
{
    public class Budget
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("BudgetId")]
        public int Id { get; set; }

        [Required]
        public DateTime BudgetDate { get; set; }

        [Required]
        public decimal TotalBudget { get; set; } = default;

        public ICollection<BudgetDetail>? BudgetDetails { get; set; } = new List<BudgetDetail>();

        public ICollection<Expenditure>? Expenditures { get; set; } = new List<Expenditure>();
    }
}

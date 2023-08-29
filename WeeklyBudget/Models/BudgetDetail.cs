using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeeklyBudget.Models
{
    public class BudgetDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("BudgetDetailId")]
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public Budget? Budget { get; set; }

        [ForeignKey(nameof(ExpenditureType))]
        public int ExpenditureTypeId { get; set; }
        public ExpenditureType? ExpenditureType { get; set; }
        public decimal TotalBudget { get; set; }
    }
}

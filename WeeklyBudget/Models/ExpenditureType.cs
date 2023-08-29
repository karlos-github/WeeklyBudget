using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeeklyBudget.Models
{
    public class ExpenditureType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("ExpenditureTypeId")]
        public int Id { get; set; }

        //[Required]
        [MaxLength(50, ErrorMessage = "Length must be less then 50 characters")]
        [Column("ExpenditureTypeName")]
        public string? Name { get; set; } = string.Empty;
        //public int? BudgetDetailId { get; set; }
        //public BudgetDetail? BudgetDetail { get; set; } = null!;
        //public int ExpenditureId { get; set; }
        //public Expenditure Expenditure { get; set; } = null!;
    }
}

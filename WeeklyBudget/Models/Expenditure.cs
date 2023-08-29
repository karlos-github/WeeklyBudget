using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeeklyBudget.Models
{
    public class Expenditure
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("ExpenditureId")]
        public int Id { get; set; }

        [Required]
        public DateTime SpentDate { get; set; } = DateTime.Now;
        public int? BudgetId { get; set; }
        public Budget? Budget { get; set; }
        //public User? User { get; set; }
        //public ExpenditureType? ExpenditureType { get; set; }

        public int UserId { get; set; }
        public int ExpenditureTypeId { get; set; }

        [Required]
        public Decimal SpentAmount { get; set; }
    }
}

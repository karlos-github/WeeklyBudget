using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeeklyBudget.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("UserId")]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50, ErrorMessage = "Length must be less then 50 characters")]
        [Column("UserName")]
        public string Name { get; set; } = string.Empty;
        //public int ExpenditureId { get; set; }
        //public Expenditure Expenditure { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeeklyBudget.Models
{
    public class ExpenditureType
    {
        public int ExpenditureTypeId { get; set; }
        public string? Name { get; set; } = string.Empty;
	}
}

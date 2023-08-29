namespace WeeklyBudget.DTO
{
    public class BudgetDefinitionDto
    {
        public DateTime BudgetDate { get; set; } = DateTime.MinValue;
        public decimal TotalBudget { get; set; } = default;
        public List<BudgetDefinitionDetailDto>? BudgetDetails { get; set; }
    }
}

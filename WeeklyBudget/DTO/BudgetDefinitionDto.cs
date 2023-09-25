namespace WeeklyBudget.DTO
{
    public class BudgetDefinitionDto
    {
        public decimal TotalBudget { get; set; } = default;
        public IEnumerable<BudgetDefinitionDetailDto>? BudgetDetails { get; set; } = new List<BudgetDefinitionDetailDto>();
    }
}

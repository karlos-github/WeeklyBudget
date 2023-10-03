using Microsoft.AspNetCore.Mvc;
using WeeklyBudget.Contracts;

namespace WeeklyBudget.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService) => _budgetService = budgetService;

        [HttpGet("get")]
        public async Task<IActionResult> GetActualBudget() => Ok(await _budgetService.GetActualBudgetAsync());

        /// <summary>
        /// TEST
        /// </summary>
        /// <param name="totalBudget"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> Update(decimal totalBudget) => Ok(await _budgetService.UpdateAsync(totalBudget));
    }
}

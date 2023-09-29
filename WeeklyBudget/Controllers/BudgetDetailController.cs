using Microsoft.AspNetCore.Mvc;
using WeeklyBudget.Contracts;

namespace WeeklyBudget.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BudgetDetailController : ControllerBase
	{
		readonly IBudgetDetailService _budgetDetailService;

		public BudgetDetailController(IBudgetDetailService budgetDetailService) => _budgetDetailService = budgetDetailService;

		[HttpGet("getAll")]
		public async Task<IActionResult> GetActualBudgetDetails() => Ok(await _budgetDetailService.GetActualBudgetDetailsAsync());

		[HttpPost("update")]
		public async Task<IActionResult> Update(int expenditureTypeId, decimal totalBudget) => Ok(await _budgetDetailService.UpdateAsync(expenditureTypeId, totalBudget));
	}
}

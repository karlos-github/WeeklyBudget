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

		/// <summary>
		/// Gets all BudgetDetails for the current budget. If the current budget was created as a default one, for each expenditure type a budget detail is created
		/// with default TotalBudget amount value. A BudgetDetail serves as a blue-print how much the user is planning to spent for some specific expenditure type.
		/// </summary>
		[HttpGet("getAll")]
		public async Task<IActionResult> GetActualBudgetDetails() => Ok(await _budgetDetailService.GetActualBudgetDetailsAsync());

		/// <summary>
		/// Updates how much user is planning to spent for the certain expenditure type.
		/// </summary>
		/// <param name="expenditureTypeId">Type of expenditure</param>
		/// <param name="totalBudget">Amount of money planned to spent for certain expenditure type specified by "expenditureTypeId" parameter</param>
		[HttpPut("update")]
		public async Task<IActionResult> Update(int expenditureTypeId, decimal totalBudget) => Ok(await _budgetDetailService.UpdateAsync(expenditureTypeId, totalBudget));
	}
}

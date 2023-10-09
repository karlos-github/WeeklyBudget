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

		/// <summary>
		/// Gets the current budget. If there's no budget existing in database corresponding to the actual month, than 
		/// a default budget will be created.
		/// </summary>
		/// <returns>BudgetDto</returns>		
		[HttpGet("get")]
		public async Task<IActionResult> GetActualBudget() => Ok(await _budgetService.GetActualBudgetAsync());

		/// <summary>
		/// Updates the current budget's TotalBudget value for the actual month.
		/// </summary>
		/// <param name="totalBudget">Total monthly budget amount</param>
		[HttpPut("update")]
		public async Task<IActionResult> Update(decimal totalBudget) => Ok(await _budgetService.UpdateAsync(totalBudget));

		/// <summary>
		/// Returns the day of the month when the user receives a salary. Is more or less constant value althouhg each monthly budget can have different value. 
		/// Default value is 15th of each month. The default value is also used in case that a default budget is created. 
		/// </summary>
		[HttpGet("getSalaryDay")]
		public async Task<IActionResult> GetSalaryDay() => Ok(await _budgetService.GetSalaryDateAsync());

		/// <summary>
		/// The day in the mounth when the user receives his/her salary
		/// </summary>
		[HttpPut("updateSalaryDay")]
		public async Task<IActionResult> UpdateSalaryDay([FromBody] int salaryDay) => Ok(await _budgetService.UpdateSalaryDayAsync(salaryDay));
	}
}

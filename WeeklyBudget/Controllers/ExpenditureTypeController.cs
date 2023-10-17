using Microsoft.AspNetCore.Mvc;
using WeeklyBudget.Contracts;

namespace WeeklyBudget.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExpenditureTypeController : ControllerBase
	{
		readonly IExpenditureTypeService _expenditureTypeService;

		public ExpenditureTypeController(IExpenditureTypeService expenditureTypeService) => _expenditureTypeService = expenditureTypeService;

		/// <summary>
		/// Gets all expenditure types that user will use in all budgets.
		/// </summary>
		[HttpGet("getAll")]
		public async Task<IActionResult> GetAll() => Ok(await _expenditureTypeService.GetAllAsync());

		/// <summary>
		/// Saves a new expenditure type
		/// </summary>
		/// <param name="expenditureType">Description of expenditure type</param>
		[HttpPost("save/{expenditureType}")]
		public async Task<IActionResult> Save(string expenditureType) => Ok(await _expenditureTypeService.SaveAsync(expenditureType));

		/// <summary>
		/// Deletes an existing expenditure type from database
		/// </summary>
		/// <param name="id">Expenditure type identification.</param>
		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> Delete(int id) => Ok(await _expenditureTypeService.DeleteAsync(id));
	}
}

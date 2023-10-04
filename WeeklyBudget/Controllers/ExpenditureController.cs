using Microsoft.AspNetCore.Mvc;
using WeeklyBudget.Contracts;

namespace WeeklyBudget.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExpenditureController : ControllerBase
	{
		readonly IExpenditureService _expenditureService;

		public ExpenditureController(IExpenditureService expenditureService) => _expenditureService = expenditureService;

		/// <summary>
		/// Gets all expenditures that user saved to database so far for the actual month.
		/// </summary>
		[HttpGet("getAll")]
		public async Task<IActionResult> GetAll() => Ok(await _expenditureService.GetAllAsync());

		/// <summary>
		/// Saves a new expenditure
		/// </summary>
		/// <param name="expenditureTypeId">Specifies expenditure type</param>
		/// <param name="amount">How much money was spent for the expenditure type specified by "expenditureTypeId" parameter</param>
		[HttpPost("save/{expenditureTypeId}/{amount}")]
		public async Task<IActionResult> Save(int expenditureTypeId, decimal amount) => Ok(await _expenditureService.SaveAsync(expenditureTypeId, amount));

		/// <summary>
		/// Deletes expenditure saved to database.
		/// </summary>
		/// <param name="id">Specifies expenditure record to be deleted from database</param>
		[HttpDelete]
		[Route("delete/{id}")]
		public async Task<IActionResult> Delete(int id) => Ok(await _expenditureService.DeleteAsync(id));
	}
}

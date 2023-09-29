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

		[HttpGet("getAll")]
		public async Task<IActionResult> GetAll() => Ok(await _expenditureService.GetAllAsync());

		[HttpPost("save/{expenditureTypeId}/{amount}")]
		public async Task<IActionResult> Save(int expenditureTypeId, decimal amount) => Ok(await _expenditureService.SaveAsync(expenditureTypeId, amount));

		[HttpDelete]
		[Route("delete/{id}")]
		public async Task<IActionResult> Delete(int id) => Ok(await _expenditureService.DeleteAsync(id));
	}
}

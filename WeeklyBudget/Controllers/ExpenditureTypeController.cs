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

		[HttpGet("getAll")]
		public async Task<IActionResult> GetAll() => Ok(await _expenditureTypeService.GetAllAsync());

		[HttpPost("save/{expenditureType}")]
		public async Task<IActionResult> Save(string expenditureType) => Ok(await _expenditureTypeService.SaveAsync(expenditureType));

		[HttpDelete]
		[Route("delete/{id}")]
		public async Task<IActionResult> Delete(int id) => Ok(await _expenditureTypeService.DeleteAsync(id));
	}
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExpenditureTypeController : ControllerBase
	{
		readonly IExpenditureTypeService _expenditureTypeService;

		public ExpenditureTypeController(IExpenditureTypeService expenditureTypeService)
		{
			_expenditureTypeService = expenditureTypeService;
		}

		[HttpGet("getAll")]
		public async Task<IActionResult> GetAll() => Ok(await _expenditureTypeService.GetAllAsync());

		[HttpPost("save/{expenditureType}")]
		public async Task<IActionResult> Save(string expenditureType)
		{
			await _expenditureTypeService.SaveAsync(expenditureType);
			return Ok();
		}

		[HttpDelete]
		[Route("delete/{id}")]
		public async Task<IActionResult> Delete(int id) => Ok(await _expenditureTypeService.DeleteAsync(id));
	}
}

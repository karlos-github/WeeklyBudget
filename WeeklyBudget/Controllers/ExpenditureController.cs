using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;


namespace WeeklyBudget.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExpenditureController : ControllerBase
	{
		readonly IExpenditureService _expenditureService;

		public ExpenditureController(IExpenditureService expenditureService)
		{
			_expenditureService = expenditureService;
		}

		[HttpGet("getAll")]
		public async Task<IActionResult> GetAll() => Ok(await _expenditureService.GetExpenditureAsync());
	}
}

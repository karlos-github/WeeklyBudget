using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;

namespace WeeklyBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet("getCurrentBudget")]
        public async Task<IActionResult> GetActualBudget() => Ok(await _budgetService.GetActualBudgetAsync_());

        [HttpPost("saveDefinition")]
        public async Task<IActionResult> Update([FromBody] BudgetDefinitionDto budget)
        {
            await _budgetService.UpdateAsync(budget);
            return Ok();
        }

        //TODO- solve UI
        /*
         * 
            https://www.youtube.com/watch?v=O5hKoBV3vaU (web api, sql , react !!!!!!)
            https://www.youtube.com/watch?v=Nip4k4JPa3w (web api, sql, react !!!! 42 minute)
            https://www.youtube.com/watch?v=5N9FsYPsAso ( --||--)

            https://learn.microsoft.com/en-us/aspnet/core/tutorials/choose-web-ui?view=aspnetcore-7.0
            https://learn.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/razor-pages-start?view=aspnetcore-7.0&tabs=visual-studio

            https://stackoverflow.com/questions/55992885/how-to-use-separate-asp-net-core-web-api-in-asp-net-core-razor-page-in-same-solu  (using Web API & Razor pages in one)

            https://www.mikesdotnetting.com/article/261/integrating-web-api-with-asp-net-razor-web-pages  (theory on REST API also)

            https://elanderson.net/2019/12/new-razor-pages-application-backed-with-an-api/ (probably solution of connecting existin web api to razor pages project)
         */
    }
}

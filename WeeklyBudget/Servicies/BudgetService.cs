using System.Diagnostics.CodeAnalysis;
using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Servicies
{
    public class BudgetService : IBudgetService
    {
        private const int _weeksPerMounth = 4;
        readonly IRepositoryManager _repositoryManager;

        public BudgetService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<BudgetDto?> GetActualBudgetAsync1()
        {
            var budget = await _repositoryManager.Budget.GetActualBudgetAsync();
            if (budget is null) return await DefaultBudgetAsync();

            var uiBudget = new BudgetDto
            {
                BudgetDate = budget.BudgetDate,
                TotalBudget = budget.TotalBudget
            };

            uiBudget.SpentAmount = budget.Expenditures?.Sum(_ => _.SpentAmount) ?? default;
            uiBudget.SpentPercent = Math.Round(uiBudget.TotalBudget != default ? (uiBudget.SpentAmount / uiBudget.TotalBudget) * 100 : default);
            uiBudget.LeftToSpentAmount = uiBudget.TotalBudget - uiBudget.SpentAmount;
            uiBudget.LeftToSpentPercent = 100 - uiBudget.SpentPercent;


            var allExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();
            var allUsers = await _repositoryManager.User.GetAllAsync() ?? Enumerable.Empty<User>();
            var uiBudgetDetails = new Dictionary<int, List<BudgetDetailDto>>();
            /*
                0 => 1 - 7
                1 => 8 - 15
                2 => 16 - 24
                3 => 25 - last day
            */

            for (int week = 0; week < 4; week++)//each budget is divided into 4 groups, each group per 7 days (last group from 25th day to the end of the month)
            {
                var details = new List<BudgetDetailDto>();
                foreach (var expenditureType in allExpenditureTypes)
                {
                    var uiUserExpenditures = new Dictionary<int, decimal>(); //TODO- create endpoint to return all expenditures per ExpenditureType , per user, per budget
                    var budgetDetails = budget?.BudgetDetails?.FirstOrDefault(_ => _.ExpenditureTypeId == expenditureType.Id);
                    var budgetExpenditures = budget?.Expenditures?.Where(_ => BudgetIndex(_.SpentDate, week) && _.ExpenditureTypeId == expenditureType.Id);
                    var detail = new BudgetDetailDto
                    {
                        ExpenditureType = expenditureType,
                        SpentAmount = budgetExpenditures?.Sum(_ => _.SpentAmount) ?? 0m,
                        TotalBudget = budget.TotalBudget / 4,
                    };
                    detail.SpentPercent = (budgetDetails is not null && budgetDetails!.TotalBudget != 0m) ? (detail.SpentAmount / budgetDetails?.TotalBudget ?? 0m) * 100 : 0m;
                    detail.LeftToSpentAmount = (budgetDetails is not null && budgetDetails!.TotalBudget != 0m) ? budgetDetails!.TotalBudget - detail.SpentAmount : 0m;
                    detail.LeftToSpentPercent = 100 - detail.SpentPercent;

                    allUsers.ToList().ForEach(user => uiUserExpenditures.TryAdd(user.Id, budgetExpenditures?.Where(_ => _.UserId == user.Id)?.Sum(_ => _.SpentAmount) ?? 0m));
                    detail.UserExpenditures = uiUserExpenditures;
                    details.Add(detail);
                }
                uiBudgetDetails[week] = details;
            }

            uiBudget.BudgetDetails = uiBudgetDetails;

            return uiBudget;
        }

        readonly Func<DateTime, int, bool> BudgetIndex = (spentDay, week) =>
        {
            return week switch
            {
                0 => 1 <= spentDay.Day && spentDay.Day <= 7,
                1 => 8 <= spentDay.Day && spentDay.Day <= 15,
                2 => 16 <= spentDay.Day && spentDay.Day <= 23,
                3 => 24 <= spentDay.Day,
                _ => false,
            };
        };

        public async Task<List<Budget>> GetAllBudgetsAsync(bool trackChanges) => await _repositoryManager.Budget.GetAllBudgetsAsync(trackChanges);

        public async Task<Budget?> GetByIdAsync(int id, bool trackChanges) => await _repositoryManager.Budget.GetByIdAsync(id, trackChanges);

        //public void CreateBudget(Budget budget) => _repositoryManager.Budget.CreateBudgetAsync(budget);

        async Task<Budget?> IBudgetService.GetActualBudgetAsync() => await _repositoryManager.Budget.GetActualBudgetAsync();

        public async Task<bool> ExistActualBudgetAsync() => await _repositoryManager.Budget.ExistActualBudgetAsync();

        async Task<BudgetDto> DefaultBudgetAsync()
        {
            var allExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();
            var budgetDetails = new Dictionary<int, List<BudgetDetailDto>>();

            for (int week = 0; week < _weeksPerMounth; week++)
            {
                var details = new List<BudgetDetailDto>();
                foreach (var expenditureType in allExpenditureTypes)
                {
                    details.Add(new BudgetDetailDto()
                    {
                        ExpenditureType = expenditureType,
                        TotalBudget = 0,
                        LeftToSpentAmount = 0,
                        LeftToSpentPercent = 100,
                        SpentAmount = 0,
                        SpentPercent = 0,

                    });
                }
                budgetDetails.Add(week, details);
            }

            return new BudgetDto()
            {
                BudgetDate = DateTime.Now,
                TotalBudget = 0,
                BudgetDetails = budgetDetails,
                LeftToSpentAmount = 0,
                LeftToSpentPercent = 100,
                SpentAmount = 0,
                SpentPercent = 0,
            };
        }

        /// <summary>
        /// Saves a new definition of a budget to database
        /// </summary>
        public async Task SaveBudgetDefinitionAsync(BudgetDefinitionDto budget)
        {
            var newBudget = new Budget();
            //TODO-KS- když budget na UI nemá definovaný BudgetDetail pro některý ExpenditureType => vytvoř defaultní BudgetDetail
            //Budget.BudgetDetails.Count == 4 !!!! vždy, měsíc je rozdělen na 4 podskupiny, hodnota BudgetDetail.TotalBudget = 0 pro defaultní BudgetDetail
            //na UI definované BudgetDetail pro daný ExpenditureType je rozdělen na 4 podskupiny, TotalBudget / 4 pro danou skupinu vždycky
            //ExpenditureType se budou definovat samostatne na UI

            newBudget.TotalBudget = budget.TotalBudget;
            newBudget.BudgetDate = budget.BudgetDate;
            var budgetDetails = new List<BudgetDetail>();

            var allExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();

            foreach (var expenditureType in allExpenditureTypes)
            {
                var detail = new BudgetDetail();
                var detailDefiniton = budget?.BudgetDetails?.FirstOrDefault(_ => _.ExpenditureType.Id == expenditureType.Id);

                detail = (detailDefiniton is not null) 
                    ? new BudgetDetail() { ExpenditureType = expenditureType, TotalBudget = detailDefiniton.TotalBudget, } 
                    : new BudgetDetail() { ExpenditureType = expenditureType, TotalBudget = 0, };

                budgetDetails.Add(detail);
            }

            await _repositoryManager.Budget.CreateBudgetAsync(newBudget);
        }

        //IEnumerable<ExpenditureType> _allExpenditureTypes = new List<ExpenditureType>();
    }
}

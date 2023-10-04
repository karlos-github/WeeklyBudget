using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Service
{
	public class BudgetService : IBudgetService
	{
		const int _weeksPerMounth = 4;
		readonly IRepositoryManager _repositoryManager;

		public BudgetService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;



		/// <summary>
		/// Returns the current budget. The current budget is the budget that is created for actual month.
		/// If the current budget already exists in db, returns it otherwise a new blank budget for the current month is created
		/// and returned.
		/// </summary>
		/// <returns></returns>
		public async Task<BudgetDto> GetActualBudgetAsync()
		{
			var allExpenditureTypes = await _repositoryManager.ExpenditureTypes.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();
			var budget = await _repositoryManager.Budgets.GetActualBudgetAsync();

			if (budget is null)
			{
				var budgetDetails = new List<BudgetDetail>();
				budget = new Budget();
				allExpenditureTypes?.ToList()?.ForEach(_ => { budgetDetails.Add(new BudgetDetail() { ExpenditureTypeId = _.ExpenditureTypeId, }); });
				budget.BudgetDetails = budgetDetails;
				await _repositoryManager.Budgets.CreateBudgetAsync(budget);

				return DefaultBudgetAsync(allExpenditureTypes ?? new List<ExpenditureType>());
			}

			var allExpenditures = await _repositoryManager.Expenditures.GetAllAsync(DateTime.Now);
			var totalExpenditurePerMonth = allExpenditures?.Sum(_ => _.SpentAmount) ?? default;
			var uiBadget = new BudgetDto()
			{
				BudgetDate = $"{budget!.BudgetDate.Month}/{budget.BudgetDate.Year}",
				MonthlyAmount = new AmountDto()
				{
					SpentAmount = totalExpenditurePerMonth,
					TotalBudget = budget.TotalBudget,
					SpentPercent = Math.Round(budget.TotalBudget != default ? (totalExpenditurePerMonth / budget.TotalBudget) * 100 : default),
					LeftToSpentAmount = budget.TotalBudget - totalExpenditurePerMonth,
					LeftToSpentPercent = 100 - Math.Round(budget.TotalBudget != default ? (totalExpenditurePerMonth / budget.TotalBudget) * 100 : default),
				},
			};

			var monthlyExpenditures = new List<ExpenditureDto>();
			foreach (var expenditureType in allExpenditureTypes)
			{
				var totalExpendituresPerType = allExpenditures?.Where(_ => _.ExpenditureTypeId == expenditureType.ExpenditureTypeId).Sum(_ => _.SpentAmount) ?? default;
				var totalPlannedExpenditurePerType = budget.BudgetDetails?.FirstOrDefault(_ => _.ExpenditureTypeId == expenditureType.ExpenditureTypeId)?.TotalBudget ?? default;
				monthlyExpenditures.Add(new ExpenditureDto()
				{
					Amount = new AmountDto()
					{
						SpentAmount = totalExpendituresPerType,
						TotalBudget = totalPlannedExpenditurePerType,
						SpentPercent = Math.Round(totalPlannedExpenditurePerType != default ? (totalExpendituresPerType / totalPlannedExpenditurePerType) * 100 : default),
						LeftToSpentAmount = totalPlannedExpenditurePerType - totalExpendituresPerType,
						LeftToSpentPercent = 100 - Math.Round(totalPlannedExpenditurePerType != default ? (totalExpendituresPerType / totalPlannedExpenditurePerType) * 100 : default),
					},
					ExpenditureType = expenditureType
				});
			}
			uiBadget.MonthlyExpenditures = monthlyExpenditures;

			var weeklyExpenditures = new List<WeeklyExpenditureDto>(_weeksPerMounth);//always have 4 items
			for (int week = 0; week < _weeksPerMounth; week++)//each budget is divided into 4 groups, each group per 7 days (last group from 25th day to the end of the month)
			{
				var expenditures = new List<ExpenditureDto>();
				foreach (var plannedExpenditureType in allExpenditureTypes)
				{
					var totalPlannedExpenditurePerType = budget.BudgetDetails!.FirstOrDefault(_ => _.ExpenditureTypeId == plannedExpenditureType.ExpenditureTypeId)?.TotalBudget / _weeksPerMounth ?? 0;
					var totalExpendituresPerType = allExpenditures?.Where(_ => WeekIndex(_.SpentDate, week) && _.ExpenditureTypeId == plannedExpenditureType.ExpenditureTypeId)?.Sum(_ => _.SpentAmount) ?? default;
					expenditures.Add(new ExpenditureDto()
					{
						Amount = new AmountDto()
						{
							SpentAmount = totalExpendituresPerType,
							TotalBudget = totalPlannedExpenditurePerType,
							SpentPercent = Math.Round(totalPlannedExpenditurePerType != default ? (totalExpendituresPerType / totalPlannedExpenditurePerType) * 100 : default),
							LeftToSpentAmount = totalPlannedExpenditurePerType - totalExpendituresPerType,
							LeftToSpentPercent = 100 - Math.Round(totalPlannedExpenditurePerType != default ? (totalExpendituresPerType / totalPlannedExpenditurePerType) * 100 : default),
						},
						ExpenditureType = plannedExpenditureType
					});
				}

				weeklyExpenditures.Add(new WeeklyExpenditureDto()
				{
					WeekDescription = $"week {week + 1}",
					Expenditures = expenditures,
				});
			}
			uiBadget.WeeklyExpenditures = weeklyExpenditures;

			return uiBadget;
		}

		/// <summary>
		/// Returns bool if some day belongs to some week 
		/// </summary>
		readonly Func<DateTime, int, bool> WeekIndex = (spentDay, week) =>
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

		/// <summary>
		/// Creates Default budget for UI, the budget always consist from 4 weeks planned expenditures
		/// </summary>
		BudgetDto DefaultBudgetAsync(IEnumerable<ExpenditureType> expenditureTypes)
		{
			var monthlyExpenditures = new List<ExpenditureDto>();
			foreach (var expenditureType in expenditureTypes)
			{
				monthlyExpenditures.Add(new ExpenditureDto
				{
					ExpenditureType = expenditureType,
					Amount = new AmountDto(),
				});
			}

			var weekExpenditures = new List<WeeklyExpenditureDto>();
			for (int week = 0; week < _weeksPerMounth; week++)
			{
				var expenditures = new List<ExpenditureDto>();
				foreach (var expenditureType in expenditureTypes)
				{
					expenditures.Add(new ExpenditureDto
					{
						ExpenditureType = expenditureType,
						Amount = new AmountDto(),
					});
				}
				weekExpenditures.Add(new WeeklyExpenditureDto()
				{
					WeekDescription = $"week {week + 1}",
					Expenditures = expenditures,
				});
			}

			return new BudgetDto()
			{
				BudgetDate = $"{DateTime.Now.Month}/{DateTime.Now.Year}",
				MonthlyAmount = new AmountDto(),
				MonthlyExpenditures = monthlyExpenditures,
				WeeklyExpenditures = weekExpenditures,
			};
		}

		/// <summary>
		/// Creates a default current budget and saves it to db. BudgetDetails collection contains an item for each existing 
		/// ExpenditureType record with default TotalBudget value
		/// </summary>
		/// <returns>Budget</returns>
		async Task<Budget> CreateDefaultBudgetAsync()
		{
			var budgetDetails = new List<BudgetDetail>();
			var expenditureTypes = await _repositoryManager.ExpenditureTypes.GetAllAsync();
			foreach (var expenditureType in expenditureTypes)
			{
				budgetDetails.Add(new BudgetDetail()
				{
					ExpenditureTypeId = expenditureType.ExpenditureTypeId,
				});
			}
			var defaultBudget = new Budget()
			{
				BudgetDate = DateTime.Now,
				BudgetDetails = budgetDetails,
			};

			await _repositoryManager.Budgets.CreateBudgetAsync(defaultBudget);

			return defaultBudget;
		}

		/// <summary>
		/// Saves a new definition of a budget to database, only TotalBudget value is possible to update
		/// </summary>
		public async Task<bool> UpdateAsync(decimal totalBudget)
		{
			var currentBudget = await _repositoryManager.Budgets.GetActualBudgetAsync() ?? await CreateDefaultBudgetAsync();
			currentBudget!.TotalBudget = totalBudget;
			return await _repositoryManager.Budgets.UpdateBudgetAsync(currentBudget);
		}
	}
}

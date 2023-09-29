using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Service
{
	public class BudgetService : IBudgetService
	{
		const int _weeksPerMounth = 4;
		readonly IRepositoryManager _repositoryManager;
		BudgetDto_? _defaultBudget;

		public BudgetService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

	

		/// <summary>
		/// Returns the current budget. If the current budget already exists in db, returns it otherwise a new blank budget for the current month is created
		/// and returned.
		/// </summary>
		/// <returns></returns>
		public async Task<BudgetDto_> GetActualBudgetAsync_()
		{
			var budget = await _repositoryManager.Budget.GetActualBudgetAsync();
			if (budget is null)
			{
				var currentBudgetDto = await DefaultBudgetAsync_();

				//TODO-KS- repeated code!!! Get rid!!!!, same in SaveBudgetDefinitionAsync(BudgetDefinitionDto budget)
				var currentBudget = new Budget();
				currentBudget.TotalBudget = currentBudgetDto.MonthlyAmount!.TotalBudget;
				currentBudget.BudgetDate = currentBudget.BudgetDate;
				var budgetDetails = new List<BudgetDetail>();
				var allExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();
				foreach (var expenditureType in allExpenditureTypes)
				{
					var detail = new BudgetDetail();
					var detailDefiniton = budget?.BudgetDetails?.FirstOrDefault(_ => _.ExpenditureTypeId == expenditureType.ExpenditureTypeId);

					detail = (detailDefiniton is not null)
						? new BudgetDetail() { ExpenditureTypeId = expenditureType.ExpenditureTypeId, TotalBudget = detailDefiniton.TotalBudget, }
						: new BudgetDetail() { ExpenditureTypeId = expenditureType.ExpenditureTypeId, TotalBudget = 0, };

					budgetDetails.Add(detail);
				}
				currentBudget.BudgetDetails = budgetDetails;

				await _repositoryManager.Budget.CreateBudgetAsync(currentBudget);
				//TODO-KS- get id of current budget saved to db
				//BudgetDto_.BudgetId ... is used for updating Budget.TotalBudget & Budget.BudgetDetails[] .... need to know the BudgetId in db
				//edit update in method UpdateAsync(BudgetDefinitionDto )
				//currentBudgetDto.BudgetId = await _repositoryManager.Budget.GetCurrentBudgetIdAsync();

				return currentBudgetDto;
			}

			//total expenditures to calculate total amounts per the whole month
			var allExpenditures = await _repositoryManager.ExpenditureRepository.GetAllAsync(DateTime.Now);//all expenditures input in db (no connection to any budget), filtered for given month
			var allPlannedExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();//all planned expenditure types that exist in db
			var totalExpenditurePerMonth = allExpenditures?.Sum(_ => _.SpentAmount) ?? default;
			var uiBadget = new BudgetDto_()
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
			foreach (var expenditureType in allExpenditures?.GroupBy(_ => _.ExpenditureTypeId).ToList()/* ?? Enumerable.Empty<Expenditure>()*/)
			{
				var totalExpendituresPerType = expenditureType.Sum(_ => _.SpentAmount);
				var totalPlannedExpenditurePerType = budget.BudgetDetails.FirstOrDefault(_ => _.ExpenditureTypeId == expenditureType.Key).TotalBudget;
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
					ExpenditureType = allPlannedExpenditureTypes?.FirstOrDefault(_ => _.ExpenditureTypeId == expenditureType.Key) ?? new ExpenditureType()
				});
			}
			uiBadget.MonthlyExpenditures = monthlyExpenditures;

			var weeklyExpenditures = new List<WeeklyExpenditureDto>(_weeksPerMounth);//always have 4 items
			for (int week = 0; week < _weeksPerMounth; week++)//each budget is divided into 4 groups, each group per 7 days (last group from 25th day to the end of the month)
			{
				var expenditures = new List<ExpenditureDto>();
				foreach (var plannedExpenditureType in allPlannedExpenditureTypes)
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
		/// Creates Default budget, the budget always consist from 4 weeks planned expenditures
		/// </summary>
		/// <returns></returns>
		async Task<BudgetDto_> DefaultBudgetAsync_()
		{
			var weekExpenditures = new List<WeeklyExpenditureDto>();
			var expenditures = new List<ExpenditureDto>();
			var expenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync();
			for (int week = 0; week < _weeksPerMounth; week++)
			{
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

			return new BudgetDto_()
			{
				BudgetDate = $"{DateTime.Now.Month}/{DateTime.Now.Year}",
				MonthlyAmount = new AmountDto(),
				MonthlyExpenditures = expenditures,
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
			var expenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync();
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

			await _repositoryManager.Budget.CreateBudgetAsync(defaultBudget);

			return defaultBudget;
		}

		/// <summary>
		/// Saves a new definition of a budget to database, only TotalBudget value is possible to update
		/// </summary>
		public async Task<bool> UpdateAsync(decimal totalBudget)
		{
			var currentBudget = await _repositoryManager.Budget.GetActualBudgetAsync() ?? await CreateDefaultBudgetAsync();
			currentBudget!.TotalBudget = totalBudget;
			return await _repositoryManager.Budget.UpdateBudgetAsync(currentBudget);
		}

		public async Task<bool> UpdateAsync(BudgetDefinitionDto budget)
		{
			var currentBudget = await _repositoryManager.Budget.GetActualBudgetAsync();
			var budgetDetails = new List<BudgetDetail>();
			var allExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();

			foreach (var expenditureType in allExpenditureTypes)
			{
				var detail = new BudgetDetail();
				var detailDefiniton = budget?.BudgetDetails?.FirstOrDefault(_ => _.ExpenditureTypeId == expenditureType.ExpenditureTypeId);

				detail = (detailDefiniton is not null)
					? new BudgetDetail() { ExpenditureTypeId = expenditureType.ExpenditureTypeId, TotalBudget = detailDefiniton.TotalBudget, BudgetId = currentBudget!.BudgetId }
					: new BudgetDetail() { ExpenditureTypeId = expenditureType.ExpenditureTypeId, TotalBudget = 0, BudgetId = currentBudget!.BudgetId };

				budgetDetails.Add(detail);
			}
			currentBudget!.BudgetDetails = budgetDetails;

			return await _repositoryManager.Budget.UpdateBudgetDetailsAsync(budgetDetails);
		}

		//TODO-KS-Create method that updates BudgetDetails collection, no other parameters of the budget will be updated at all!!!!
		//public async Task UpdateAsync(BudgetDefinitionDto budget)
		//{
		//	//TODO-KS- když budget na UI nemá definovaný BudgetDetail pro některý ExpenditureType => vytvoř defaultní BudgetDetail
		//	//Budget.BudgetDetails.Count == 4 !!!! vždy, měsíc je rozdělen na 4 podskupiny, hodnota BudgetDetail.TotalBudget = 0 pro defaultní BudgetDetail
		//	//na UI definované BudgetDetail pro daný ExpenditureType je rozdělen na 4 podskupiny, TotalBudget / 4 pro danou skupinu vždycky
		//	//ExpenditureType se budou definovat samostatne na UI
		//	var currentBudget = await _repositoryManager.Budget.GetActualBudgetAsync();
		//	currentBudget!.TotalBudget = budget.TotalBudget;
		//	//currentBudget.BudgetDetails = 
		//	var budgetDetails = new List<BudgetDetail>();
		//	var allExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();

		//	foreach (var expenditureType in allExpenditureTypes)
		//	{
		//		var detail = new BudgetDetail();
		//		var detailDefiniton = budget?.BudgetDetails?.FirstOrDefault(_ => _.ExpenditureTypeId == expenditureType.ExpenditureTypeId);

		//		detail = (detailDefiniton is not null)
		//			? new BudgetDetail() { ExpenditureTypeId = expenditureType.ExpenditureTypeId, TotalBudget = detailDefiniton.TotalBudget, BudgetId = currentBudget.BudgetId }
		//			: new BudgetDetail() { ExpenditureTypeId = expenditureType.ExpenditureTypeId, TotalBudget = 0, BudgetId = currentBudget.BudgetId };

		//		budgetDetails.Add(detail);
		//	}
		//	currentBudget.BudgetDetails = budgetDetails;

		//	await _repositoryManager.Budget.UpdateBudgetAsync(currentBudget);
		//}
	}
}

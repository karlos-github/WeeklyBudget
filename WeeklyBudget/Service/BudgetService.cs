using System.Diagnostics.CodeAnalysis;
using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Servicies
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
				return await DefaultBudgetAsync_();
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

		//Depreciated code!!!!!!
		//public async Task<BudgetDto> GetActualBudgetAsync()
		//{
		//	//			var budget = await _repositoryManager.Budget.GetActualBudgetAsync();
		//	//			if (budget is null) return _defaultBudget ??= await DefaultBudgetAsync();

		//	//#pragma warning disable IDE0017 // Simplify object initialization
		//	//			var uiBudget = new BudgetDto
		//	//			{
		//	//				BudgetDate = budget.BudgetDate,
		//	//				TotalBudget = budget.TotalBudget
		//	//			};
		//	//#pragma warning restore IDE0017 // Simplify object initialization

		//	//			uiBudget.SpentAmount = budget.Expenditures?.Sum(_ => _.SpentAmount) ?? default;
		//	//			uiBudget.SpentPercent = Math.Round(uiBudget.TotalBudget != default ? (uiBudget.SpentAmount / uiBudget.TotalBudget) * 100 : default);
		//	//			uiBudget.LeftToSpentAmount = uiBudget.TotalBudget - uiBudget.SpentAmount;
		//	//			uiBudget.LeftToSpentPercent = 100 - uiBudget.SpentPercent;

		//	//			var allExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();
		//	//			var uiBudgetDetails = new Dictionary<int, List<BudgetDetailDto>>();
		//	//			/*
		//	//                week 0 => days ... 1 - 7 
		//	//                week 1 => days ... 8 - 15
		//	//                week 2 => days ... 16 - 24
		//	//                week 3 => days ... 25 - last day
		//	//            */

		//	//			for (int week = 0; week < _weeksPerMounth; week++)//each budget is divided into 4 groups, each group per 7 days (last group from 25th day to the end of the month)
		//	//			{
		//	//				var details = new List<BudgetDetailDto>();
		//	//				foreach (var expenditureType in allExpenditureTypes)
		//	//				{
		//	//					var budgetDetails = budget?.BudgetDetails?.FirstOrDefault(_ => _.ExpenditureType.Id == expenditureType.Id);
		//	//					var budgetExpenditures = budget?.Expenditures?.Where(_ => BudgetIndex(_.SpentDate, week) && _.ExpenditureType.Id == expenditureType.Id);
		//	//					var detail = new BudgetDetailDto
		//	//					{
		//	//						ExpenditureTypeName = expenditureType.Name!,
		//	//						SpentAmount = budgetExpenditures?.Sum(_ => _.SpentAmount) ?? 0m,
		//	//						TotalBudget = budget!.TotalBudget / _weeksPerMounth,
		//	//					};
		//	//					detail.SpentPercent = (budgetDetails is not null && budgetDetails!.TotalBudget != 0m) ? (detail.SpentAmount / budgetDetails?.TotalBudget ?? 0m) * 100 : 0m;
		//	//					detail.LeftToSpentAmount = (budgetDetails is not null && budgetDetails!.TotalBudget != 0m) ? budgetDetails!.TotalBudget - detail.SpentAmount : 0m;
		//	//					detail.LeftToSpentPercent = 100 - detail.SpentPercent;

		//	//					details.Add(detail);
		//	//				}
		//	//				uiBudgetDetails[week] = details;
		//	//			}

		//	//			uiBudget.BudgetDetails = uiBudgetDetails;

		//	//return uiBudget;

		//	return new BudgetDto();
		//}

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


		//Depreciated code!!!!!!
		//async Task<BudgetDto> DefaultBudgetAsync()
		//{
		//	var budgetDetails = new Dictionary<int, List<BudgetDetailDto>>();
		//	for (int week = 0; week < _weeksPerMounth; week++)
		//	{
		//		var details = new List<BudgetDetailDto>();
		//		foreach (var expenditureType in await _repositoryManager.ExpenditureType.GetAllAsync())
		//		{
		//			details.Add(new BudgetDetailDto()
		//			{
		//				ExpenditureTypeName = expenditureType.Name!,
		//				TotalBudget = 0,
		//				LeftToSpentAmount = 0,
		//				LeftToSpentPercent = 100,
		//				SpentAmount = 0,
		//				SpentPercent = 0,

		//			});
		//		}
		//		budgetDetails.Add(week, details);
		//	}

		//	return new BudgetDto()
		//	{
		//		BudgetDate = DateTime.Now,
		//		TotalBudget = 0,
		//		BudgetDetails = budgetDetails,
		//		LeftToSpentAmount = 0,
		//		LeftToSpentPercent = 100,
		//		SpentAmount = 0,
		//		SpentPercent = 0,
		//	};
		//}

		/// <summary>
		/// Saves a new definition of a budget to database
		/// </summary>
		public async Task UpdateAsync(BudgetDefinitionDto budget)
		{
			//TODO-KS- když budget na UI nemá definovaný BudgetDetail pro některý ExpenditureType => vytvoř defaultní BudgetDetail
			//Budget.BudgetDetails.Count == 4 !!!! vždy, měsíc je rozdělen na 4 podskupiny, hodnota BudgetDetail.TotalBudget = 0 pro defaultní BudgetDetail
			//na UI definované BudgetDetail pro daný ExpenditureType je rozdělen na 4 podskupiny, TotalBudget / 4 pro danou skupinu vždycky
			//ExpenditureType se budou definovat samostatne na UI
			var currentBudget = await _repositoryManager.Budget.GetActualBudgetAsync();
			currentBudget!.TotalBudget = budget.TotalBudget;
			//currentBudget.BudgetDetails = 
			var budgetDetails = new List<BudgetDetail>();
			var allExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();

			foreach (var expenditureType in allExpenditureTypes)
			{
				var detail = new BudgetDetail();
				var detailDefiniton = budget?.BudgetDetails?.FirstOrDefault(_ => _.ExpenditureTypeId == expenditureType.ExpenditureTypeId);

				detail = (detailDefiniton is not null)
					? new BudgetDetail() { ExpenditureTypeId = expenditureType.ExpenditureTypeId, TotalBudget = detailDefiniton.TotalBudget, BudgetId = currentBudget.BudgetId }
					: new BudgetDetail() { ExpenditureTypeId = expenditureType.ExpenditureTypeId, TotalBudget = 0, BudgetId = currentBudget.BudgetId };

				budgetDetails.Add(detail);
			}
			currentBudget.BudgetDetails = budgetDetails;

			await _repositoryManager.Budget.UpdateBudgetAsync(currentBudget);
		}
	}
}

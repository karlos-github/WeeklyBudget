using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Service
{
	public class BudgetService : IBudgetService
	{
		const int _weeksPerMounth = 4;
		const int _defaultSalaryDay = 15;
		readonly IRepositoryManager _repositoryManager;

		public BudgetService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

		/// <summary>
		/// Sets the default budget day based on _defaultSalaryDay constant
		/// </summary>
		static DateTime SetDefaultBudgetDate()
			=> DateTime.Now.Date.Day < _defaultSalaryDay
				? new DateTime(DateTime.Now.Year, DateTime.Now.Month, _defaultSalaryDay).AddMonths(-1)
				: new DateTime(DateTime.Now.Year, DateTime.Now.Month, _defaultSalaryDay);

		/// <summary>
		/// Returns the current budget. The current budget is the budget that is created for actual month.
		/// If the current budget already exists in db, returns it otherwise a new blank budget for the current month is created
		/// and returned.
		/// </summary>
		public async Task<BudgetDto> GetActualBudgetAsync()
		{
			var allExpenditureTypes = await _repositoryManager.ExpenditureTypes.GetAllAsync() ?? Enumerable.Empty<ExpenditureType>();
			var budget = await _repositoryManager.Budgets.GetActualBudgetAsync(deep: true);

			//No budget exists for the current month => a default one gonna be created!!!
			if (budget is null)
			{
				var budgetDetails = new List<BudgetDetail>();
				budget = new Budget()
				{
					BudgetDate = SetDefaultBudgetDate()//TODO-KS- copy BudgetDate from the previous budget if exists any, otherwise the default value will be used
				};
				allExpenditureTypes?.ToList()?.ForEach(_ => { budgetDetails.Add(new BudgetDetail() { ExpenditureTypeId = _.ExpenditureTypeId, }); });
				budget.BudgetDetails = budgetDetails;
				await _repositoryManager.Budgets.CreateBudgetAsync(budget);

				return DefaultBudgetAsync(allExpenditureTypes ?? new List<ExpenditureType>());
			}

			//There already is a budget for the current month => transform Budget to BudgetDto (UI)
			var allExpenditures = await _repositoryManager.Expenditures.GetAllAsync(DateTime.Now);
			var totalExpenditurePerMonth = allExpenditures?.Sum(_ => _.SpentAmount) ?? default;
			var uiBadget = new BudgetDto()
			{
				BudgetDate = $"{budget!.BudgetDate.Month}/{budget.BudgetDate.Year}",
				SalaryDay = budget.BudgetDate.Day,
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
				//TODO-KS- if no BudgetDetails defined and total budget amount is defined => TotalBudget in MonthlyExpenditure and TotalBudgets in all WeekExpenditures equally devide
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
			for (int week = 0; week < _weeksPerMounth; week++)//each budget is divided into 4 blocks, per 7 days each, starting from BudgetDate (last group from 25th day to the next month, next SalaryDay)
			{
				//TODO-KS- Create dynamic predicate by creating an Expression
				var lowerBoundDateTime = budget.BudgetDate.AddDays(7 * week);
				var upperBoundDateTime = (week == _weeksPerMounth - 1) ? budget.BudgetDate.AddMonths(1) : budget.BudgetDate.AddDays(7 * (week + 1));

				var expenditures = new List<ExpenditureDto>();
				foreach (var plannedExpenditureType in allExpenditureTypes)
				{
					var totalPlannedExpenditurePerType = budget.BudgetDetails!.FirstOrDefault(_ => _.ExpenditureTypeId == plannedExpenditureType.ExpenditureTypeId)?.TotalBudget / _weeksPerMounth ?? 0;

					//to filter Expenditure.SpentDate 0 - 7, 7 - 14, 14 - 21, 21 - BudgetDate (next month)
					var totalExpendituresPerType = allExpenditures
						?.Where(_ => _.SpentDate >= lowerBoundDateTime
							&& _.SpentDate < upperBoundDateTime
							&& _.ExpenditureTypeId == plannedExpenditureType.ExpenditureTypeId)
						?.Sum(_ => _.SpentAmount)
						?? default;

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
					From = lowerBoundDateTime,
					To = upperBoundDateTime,
					Expenditures = expenditures,
				});
			}
			uiBadget.WeeklyExpenditures = weeklyExpenditures;

			return uiBadget;
		}

		/// <summary>
		/// Creates Default budget for UI, the budget always consist from 4 weeks planned expenditures
		/// </summary>
		static BudgetDto DefaultBudgetAsync(IEnumerable<ExpenditureType> expenditureTypes)
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
				BudgetDate = $"{SetDefaultBudgetDate().Month}/{SetDefaultBudgetDate().Year}",
				SalaryDay = SetDefaultBudgetDate().Day,
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
			var currentBudget = await _repositoryManager.Budgets.GetActualBudgetAsync(deep: false) ?? await CreateDefaultBudgetAsync();
			currentBudget!.TotalBudget = totalBudget;
			return await _repositoryManager.Budgets.UpdateBudgetAsync(currentBudget);
		}

		/// <summary>
		/// Returns the day of the month when the user receives a salary. Is more or less constant value. Default value is 15th of each month.
		/// The default value is used in case that a default budget is created.
		/// </summary>
		public async Task<int> GetSalaryDateAsync()
		{
			var currentBudget = await _repositoryManager.Budgets.GetActualBudgetAsync(deep: false) ?? await CreateDefaultBudgetAsync();
			return currentBudget.BudgetDate.Day;
		}

		/// <summary>
		/// Updates the day when a user receives salary every month
		/// </summary>
		/// <param name="salaryDay">The day of a month</param>
		/// <returns>True if update wase successful otherwise false</returns>
		public async Task<bool> UpdateSalaryDayAsync(int salaryDay)
		{
			//TODO-KS- check input salaryDay for the existing day in the current month!!!
			var currentBudget = await _repositoryManager.Budgets.GetActualBudgetAsync(deep: false) ?? await CreateDefaultBudgetAsync();
			currentBudget.BudgetDate = new DateTime(currentBudget.BudgetDate.Year, currentBudget.BudgetDate.Month, salaryDay);
			return await _repositoryManager.Budgets.UpdateBudgetAsync(currentBudget);
		}
	}
}

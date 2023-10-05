using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Service
{
	public class ExpenditureService : IExpenditureService
	{
		readonly IRepositoryManager _repositoryManager;

		public ExpenditureService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

		/// <summary>
		/// Gets expenditure by given id
		/// </summary>
		/// <param name="id">Expenditure Id</param>
		public async Task<Expenditure?> GetByIdAsync(int id) => await _repositoryManager.Expenditures.GetByIdAsync(id);

		/// <summary>
		/// Deletes the expenditure
		/// </summary>
		/// <param name="id">Expenditure Id</param>
		public async Task<bool> DeleteAsync(int id)
		{
			var dto = await GetByIdAsync(id);
			return dto != null && await _repositoryManager.Expenditures.DeleteAsync(dto);
		}

		/// <summary>
		/// Creates a new expenditure for the current monthly budget for the certain expenditure type
		/// </summary>
		/// <param name="expenditureTypeId">Expenditure type Id</param>
		/// <param name="amount">Money spent for the certain expenditure type</param>
		public async Task<bool> SaveAsync(int expenditureTypeId, decimal amount)
		{
			var expenditureType = await _repositoryManager.ExpenditureTypes.GetByIdAsync(expenditureTypeId);
			return expenditureType != null && await _repositoryManager.Expenditures.SaveAsync(new Expenditure()
			{
				ExpenditureTypeId = expenditureTypeId,
				SpentAmount = amount,
				SpentDate = DateTime.Now,
			});
		}

		/// <summary>
		/// Returns all expenditures for the current monthly budget
		/// </summary>
		public async Task<IEnumerable<ExpenditureDto>> GetAllAsync()
		{
			var expenditures = new List<ExpenditureDto>();
			var allPlannedExpenditureTypes = await _repositoryManager.ExpenditureTypes.GetAllAsync();
			if (allPlannedExpenditureTypes == null) return expenditures;

			foreach (var expenditure in await _repositoryManager.Expenditures.GetAllAsync(DateTime.Now))
			{
				expenditures.Add(new ExpenditureDto()
				{
					ExpenditureType = allPlannedExpenditureTypes.FirstOrDefault(_ => _.ExpenditureTypeId == expenditure.ExpenditureTypeId)!,
					Amount = new AmountDto()
					{
						SpentAmount = expenditure.SpentAmount
					},
					ExpenditureId = expenditure.ExpenditureId,
				});
			}
			return expenditures;
		}
	}
}

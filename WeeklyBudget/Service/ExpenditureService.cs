using WeeklyBudget.Contracts;
using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Service
{
	public class ExpenditureService : IExpenditureService
	{
		readonly IRepositoryManager _repositoryManager;

		public ExpenditureService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

		public async Task<Expenditure?> GetByIdAsync(int id) => await _repositoryManager.ExpenditureRepository.GetByIdAsync(id);

		public async Task<bool> DeleteAsync(int id)
		{
			var dto = await GetByIdAsync(id);
			return dto != null && await _repositoryManager.ExpenditureRepository.DeleteAsync(dto);
		}

		public async Task<bool> SaveAsync(int expenditureTypeId, decimal amount)
		{
			var expenditureType = await _repositoryManager.ExpenditureType.GetByIdAsync(expenditureTypeId);
			return expenditureType != null && await _repositoryManager.ExpenditureRepository.SaveAsync(new Expenditure()
			{
				ExpenditureTypeId = expenditureTypeId,
				SpentAmount = amount,
				SpentDate = DateTime.Now,
			});
		}

		public async Task<IEnumerable<ExpenditureDto>> GetAllAsync()
		{
			var expenditures = new List<ExpenditureDto>();
			var allPlannedExpenditureTypes = await _repositoryManager.ExpenditureType.GetAllAsync();
			if (allPlannedExpenditureTypes == null) return expenditures;

			foreach (var expenditure in await _repositoryManager.ExpenditureRepository.GetAllAsync(DateTime.Now))
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

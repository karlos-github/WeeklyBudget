using WeeklyBudget.Contracts;
using WeeklyBudget.Models;

namespace WeeklyBudget.Service
{
	public class ExpenditureTypeService : IExpenditureTypeService
	{
		readonly IRepositoryManager _repositoryManager;

		public ExpenditureTypeService(IRepositoryManager repositoryManager)
			=> _repositoryManager = repositoryManager;

		public async Task<ExpenditureType?> GetByIdAsync(int id)
			=> await _repositoryManager.ExpenditureType.GetByIdAsync(id);

		public async Task<bool> DeleteAsync(int id)
		{
			var dto = await GetByIdAsync(id);
			return dto != null && await _repositoryManager.ExpenditureType.DeleteAsync(dto);
		}

		public async Task<IEnumerable<ExpenditureType>> GetAllAsync()
			=> await _repositoryManager.ExpenditureType.GetAllAsync();

		public async Task<bool> SaveAsync(string expenditureTypeName) => 
			string.IsNullOrEmpty(expenditureTypeName) 
			? false 
			: await _repositoryManager.ExpenditureType.SaveAsync(new ExpenditureType() { Name = expenditureTypeName });
	}
}

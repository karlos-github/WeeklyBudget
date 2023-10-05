using WeeklyBudget.Contracts;
using WeeklyBudget.Models;

namespace WeeklyBudget.Service
{
	public class ExpenditureTypeService : IExpenditureTypeService
	{
		readonly IRepositoryManager _repositoryManager;

		public ExpenditureTypeService(IRepositoryManager repositoryManager)
			=> _repositoryManager = repositoryManager;

		/// <summary>
		/// Get expenditure type by given Id
		/// </summary>
		/// <param name="id">Expenditure type Id</param>
		public async Task<ExpenditureType?> GetByIdAsync(int id)
			=> await _repositoryManager.ExpenditureTypes.GetByIdAsync(id);

		/// <summary>
		/// Deletes Expenditure type given by its Id
		/// </summary>
		/// <param name="id">Expenditure type Id</param>
		public async Task<bool> DeleteAsync(int id)
		{
			var dto = await GetByIdAsync(id);
			return dto != null && await _repositoryManager.ExpenditureTypes.DeleteAsync(dto);
		}

		/// <summary>
		/// Gets all Expenditure types
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<ExpenditureType>> GetAllAsync()
			=> await _repositoryManager.ExpenditureTypes.GetAllAsync();

		public async Task<bool> SaveAsync(string expenditureTypeName)
			=> !string.IsNullOrEmpty(expenditureTypeName) && await _repositoryManager.ExpenditureTypes.SaveAsync(new ExpenditureType() { Name = expenditureTypeName });
	}
}

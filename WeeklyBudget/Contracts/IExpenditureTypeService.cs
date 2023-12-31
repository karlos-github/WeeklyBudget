﻿using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
	public interface IExpenditureTypeService
	{
		Task<ExpenditureType?> GetByIdAsync(int id);
		Task<IEnumerable<ExpenditureType>> GetAllAsync();
		Task<bool> SaveAsync(string expenditureTypeName);
		Task<bool> DeleteAsync(int id);
	}
}

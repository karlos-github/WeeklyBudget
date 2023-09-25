﻿using WeeklyBudget.Models;

namespace WeeklyBudget.Contracts
{
    public interface IBudgetRepository
    {
        Task CreateBudgetAsync(Budget budget);
        Task<Budget?> GetActualBudgetAsync();
        Task<bool> UpdateBudgetAsync(Budget budget);
	}
}

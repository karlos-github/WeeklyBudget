using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Servicies
{
    public static class BudgetHelper
    {
        public static BudgetDto MapBudget(this Budget budget)
        {
            return new BudgetDto();
        }

        //TODO-KS- creates default budget in case that current budget not exists yet
        // takes setting of previous budget if exists or creates default with zero amounts using all expenditure types that exist
        public static BudgetDto MapDefaultBudget()
        {

            return new BudgetDto();
        }
    }
}

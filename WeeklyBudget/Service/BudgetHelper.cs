using WeeklyBudget.DTO;
using WeeklyBudget.Models;

namespace WeeklyBudget.Service
{
    public static class BudgetHelper
    {
        public static BudgetDto_ MapBudget(this Budget budget)
        {
            return new BudgetDto_();
        }

        //TODO-KS- creates default budget in case that current budget not exists yet
        // takes setting of previous budget if exists or creates default with zero amounts using all expenditure types that exist
        public static BudgetDto_ MapDefaultBudget()
        {

            return new BudgetDto_();
        }
    }
}

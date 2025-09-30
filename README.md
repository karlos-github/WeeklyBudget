
# Monthly Budget API

An ASP.NET Core Web API for managing monthly expenses. The application is designed to help users track their monthly spending and save money.

Users can define common expense categories (e.g., Food, Petrol, Rent, etc.) and set a planned budget for each category for the month. The planned budget for each expense type is divided into four weekly budgets, making it easier for users to monitor their spending on a weekly basis.

Throughout the month, users record their expenses. They can then see how much has already been spent in each category and how much remains for the current month or week. Users can also review the detailed expenses recorded under each category.

At the beginning of each new month, a new budget is created automatically.


## API Reference

### Budget Controller

#### Get Current Budget 

This endpoint returns the current budget. If no budget exists in the database for the current month, a default budget is created. The default budget has a total amount of 0 CZK, and all existing expense categories are included with their amounts set to 0 CZK.
```http
  GET /api/Budget/get
```

#### Update total budget amount of money
This endpoint updates the TotalBudget value of the current month’s budget. The default value is 0 CZK.

```http
  PUT /api/Budget/update
```

|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `totalBudget`      | `double` | **Required**. Total monthly budget amount |

#### Get salary day
This endpoint returns the day of the month when the user receives their salary. Budget expenditures are calculated from this day until the same day of the following month, either on a monthly or weekly basis.

The salary day is generally a fixed value, although each monthly budget can have a different one. By default, it is set to the 15th of each month. This default value is also applied when a default budget is created.

```http
  GET /api/Budget/getSalaryDay
```

#### Update salary day
This endpoint changes the day from which the current monthly budget is calculated. The budget then runs until the same day of the following month, when it ends.

The salary day represents the day the user receives their salary.

```http
  PUT /api/Budget/updateSalaryDay
```
|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `salaryDay`      | `int` | **Required**. Day of the month when salary is recieved |


### Budget Detail Controller

#### Get all budget details
This endpoint retrieves all BudgetDetails for the current budget. A BudgetDetail acts as a blueprint for how much the user plans to spend on a specific expense category.

If the current budget was created as a default one, a budget detail is generated for each expense category with the TotalBudget amount set to the default value.

Based on the TotalBudget value and the expenditures stored in the database, the system calculates how much the user can still spend on a monthly or weekly basis.

```http
  GET /api/BudgetDetail/getAll
```

#### Update budget detail's total budget amount
This endpoint updates how much user is planning to spent for the certain expenditure type. The default value is 0 CZK.

```http
  PUT /api/BudgetDetail/update
```
|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `expenditureTypeId` | `int` | **Required**. Type of expenditure |
| `totalBudget` | `double` | **Required**. Amount of money planned to spent for certain expenditure type |

### Expenditure Controller

#### Get all expenditures
This endpoint retrieves all expenditures saved by the user for the current month. The current budget runs from the salary day of the current month until the same day of the following month.

All expenditures recorded in the database during this period, and assigned to an expense category that matches one defined in the budget details of the current monthly budget, are included in the results.

```http
  GET /api/Expenditure/getAll
```

#### Save expenditure
This endpoint saves a new expenditure to database.

```http
  POST /api/Expenditure/save/{expenditureTypeId}/{amount}
```
|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `expenditureTypeId` | `int` | **Required**. Type of expenditure |
| `amount` | `double` | **Required**. Amount of money spent for certain expenditure type |

#### Delete expenditure
This endpoint deletes the existing expenditure.

```http
  DELETE /api/Expenditure/delete/{id}
```
|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `id` | `int` | **Required**. Type of expenditure |

#### Get all expenditures types
This endpoint retrieves all expenditure types saved by the user in the database. An expenditure type represents a specific category of spending that the user wants to track in the monthly budget.

```http
  GET /api/ExpenditureType/getAll
```
#### Save a new expenditure type
This endpoint saves a new expenditure type. All expenditure types are automatically included in the monthly budget.

A monthly budget consists of several budget details, each linked to a single expenditure type. A budget detail defines the TotalBudget amount that the user plans to spend for that expenditure type in the current budget.

```http
  POST /api/ExpenditureType/save/{expenditureType}
```
|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `expenditureType` | `string` | **Required**. Description of expenditure type |

#### Delete expenditure type
This endpoint deletes an existing expenditure type.

```http
  DELETE /api/ExpenditureType/delete/{id}
```
|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `id` | `int` | **Required**. Expenditure type identification. |

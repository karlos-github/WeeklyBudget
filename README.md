
# Monthly Budget API

An ASP.NET Core Web API for managing monthly expences. Application is designed to help follow monthly expenses and by this way save down money. 

The user can set some typical types of expences (for example like Food, Petrol, Rent, ...) and planned amount of money assumed to by spent for each expence type during the  month. Planned amount of money for each expense type is devided in a 4 week budgets, so the user can more easily follow his/her expences on week bases. 

User writes down his/her expences during the month. The user can follow how much has already spent for each expence type and how much is left to spent for actual month or week of the month. User can also check expences that were count in each expence types. 

With the begining of a new month a new budget is created automatically.



## API Reference

### Budget Controller

#### Get Current Budget 

This endpoint returns the current budget. If there's no budget existing in database for the corresponding current month than a default budget will be created. The default budget is created with total amount of money set to 0 CZK. All existing expenditure types are set in the default budget with 0 CZK amount of money.
```http
  GET /api/Budget/get
```

#### Update total budget amount of money
This endpoint updates the current budget's TotalBudget value for the actual month. Default value is 0 CZK.

```http
  PUT /api/Budget/update
```

|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `totalBudget`      | `double` | **Required**. Total monthly budget amount |

#### Get salary day
Returns the day of the month when the user receives a salary. From this day to the same day next month budget expenditures are calculated on monthly or weekly bases. Is more or less constant value althouhg each monthly budget can have different value. Default value is 15th of each month. The default value is also used in case that a default budget is created. 

```http
  GET /api/Budget/getSalaryDay
```

#### Update salary day
This endpoint is used to change the day from which the current monthly budget will be calculated and followed to the same day next month when the current budget ends. The salary day is the day when the user receives his/her salary.

```http
  PUT /api/Budget/updateSalaryDay
```
|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `salaryDay`      | `int` | **Required**. Day of the month when salary is recieved |


### Budget Detail Controller

#### Get all budget details
Gets all BudgetDetails for the current budget. A BudgetDetail serves as a blue-print how much the user is planning to spent for some specific expenditure type. If the current budget was created as a default one, for each expenditure type a budget detail is created with default TotalBudget amount value. Based on TotalBudget amount value and expenditures saved to database is calculated how much user can still spent on monthly or weekly bases.

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
Gets all expenditures that user saved to database so far for the current month. The current budget starts from the salary day of the current month to the same day next month. All expenditures saved to dabase for this time period and assigned with the certain expenditure type that corresponds with expenditure type used in budget details of the current monthly budget are included in the current month expenditures.

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
Gets all expenditures types that user saved to database. The ependiture type represents a certain type of expenditure that user plans to follow in the monthly budget.

```http
  GET /api/ExpenditureType/getAll
```
#### Save a new expenditure type
This endpoint saves a new expenditure type. All existing expenditures are automatically used in the monthly budget. The monthly budget contains several budget details. Each budget detail holds only one expenditure type and defines a total budget amount value for this expenditure type that user plans to spent for the current budget.

```http
  POST /api/ExpenditureType/save/{expenditureType}
```
|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `expenditureType` | `string` | **Required**. Description of expenditure type |

#### Delete expenditure type
This endpoint deletes the existing expenditure type.

```http
  DELETE /api/ExpenditureType/delete/{id}
```
|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `id` | `int` | **Required**. Expenditure type identification. |
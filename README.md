
# Monthly Budget API

An ASP.NET Core Web API for managing monthly expences. Application is designed to help follow monthly expenses and by this way save down money. 

The user can set some typical types of expences (for example like Food, Petrol, Rent, ...) and planned amount of money assumed to by spent for each expence type during the  month. Planned amount of money for each expense type is devided in a 4 week budgets, so the user can more easily follow his/her expences on week bases. 

User writes down his/her expences during the month. The user can follow how much has already spent for each expence type and how much is left to spent for actual month or week of the month. User can also check expences that were count in each expence types. 

With the begining of a new month a new budget is created automatically.



## API Reference

### Budget Controller

#### GetActualBudget()  

Gets the current budget. If there's no budget in database for corresponding the actual month a default budget will be created.
```http
  GET /api/Budget/get
```

#### Update(decimal totalBudget)
Updates the current budget's TotalBudget value for the actual month. Default value is 0 CZK. User can update this default value by this endpoint.

```http
  PUT /api/Budget/update
```

|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `totalBudget`      | `double` | **Required**. Total monthly budget amount |

#### GetSalaryDay()
Returns the day of the month when the user receives a salary. Is more or less constant value althouhg each monthly budget can have different value. Default value is 15th of each month. The default value is also used in case that a default budget is created. 

```http
  GET /api/Budget/getSalaryDay
```

#### UpdateSalaryDay(int salaryDay)
The day in the mounth when the user receives his/her salary. From this day monthly budget is calculated and followed.

```http
  PUT /api/Budget/updateSalaryDay
```
|   Parameter  | Type     | Description                       |
| :--------    | :------- | :-------------------------------- |
| `salaryDay`      | `int` | **Required**. Day of the month when salary is recieved |



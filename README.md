
# Monthly Budget API

An ASP.NET Core Web API for managing monthly expences. Application is designed to help follow monthly expenses and by this way save down money. 

The user can set some typical types of expences (for example like Food, Petrol, Rent, ...) and planned amount of money assumed to by spent for each expence type during the  month. Planned amount of money for each expense type is devided in a 4 week budgets, so the user can more easily follow his/her expences on week bases. 

User writes down his/her expences during the month. The user can follow how much has already spent for each expence type and how much is left to spent for actual month or week of the month. User can also check expences that were count in each expence types. 

With the begining of a new month a new budget is created automatically.



## API Reference

### Budget Controller

#### Get Current Budget
Gets the current budget. If there's no budget in database for corresponding the actual month a default budget will be created.
```http
  GET /api/Budget/get
```

#### Get item

```http
  GET /api/items/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to fetch |

#### add(num1, num2)

Takes two numbers and returns the sum.


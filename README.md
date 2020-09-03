# Class Activities

- - -

## 4. CRUD and REST - Better Together

| Activity Time:       1:00 |  Elapsed Time:      1:30  |
|---------------------------|---------------------------|

<details>
  <summary><strong>📣 4.1 Everyone Do: CRUD with REST (1:00)</strong></summary>

* **Files**:

  * [Financials.sln](Unsolved/Financials.sln)

* Ask a student (or: the class) to describe what [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete) means.

* Explain that, we can use different routes and HTTP Methods in our [REST](https://en.wikipedia.org/wiki/Representational_state_transfer)ful API to handle each of the CRUD operations: **C**reate, **R**ead, **U**pdate, **D**elete.
  * For **C**reate we'll use a route with HTTP POST
  * For **R**ead we'll use a route with HTTP GET
  * For **U**pdate we'll use a route with HTTP PUT
  * For **D**elete we'll use a route with HTTP DELETE

* Open the [Financials.sln](Unsolved/Financials.sln).

* Show the [Transaction](Unsolved/Financials.Data/Models/Transaction.cs) class that we will be providing CRUD.

* Open the [TransactionsController](Unsolved/Financials/Controllers/TransactionsController.cs) and walk through the provided code.

* Point out that to keep things organized we'll create a separate controller for each class that our application uses.
  * For example, a Transaction class will have a TransactionsController. The TransactionsController will handle all CRUD operations for the Transaction class.
  * If we add a new Receipt class we would create a new ReceiptsController to handle CRUD for the Receipt class.

* Highlight the `Route` attribute and explain that this is telling ASP.NET Core to route all requests for `/api/transactions` to this controller class to handle. It gets the `api` portion from the `Route` attribute and `[controller]` gets replaced using the name of the class minus the word `Controller`.

```code
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
```

* Point out that we are also getting a context for EFCore.

```code
    private readonly FinancialsContext _context;

    public TransactionsController(FinancialsContext context)
    {
        _context = context;
    }
```

* Run the `Financials` project to demonstrate that it builds successfully and runs.
  * Explain that this is a Web API solution so there is no UI.
  * Highlight that this means we'll need a separate tool to test our application. We'll use [Postman](https://www.postman.com/).
    * You can either create the requests in Postman as you live code the routes...
    * OR import the request collection into Postman using the [instructions](Supplemental/README.md) found in the Supplemental folder.
  * Remind the class that we'll also need to create some routes for our application to do useful work.

* Create a method to get all transactions. Highlight the `[HttpGet]` attribute. This is telling ASP.NET Core that GET requests will match this route. The `GetTransactions` method doesn't have any parameters. So this will match GET requests to api/Transactions.
  * Within the method we'll use our EFCore context to return all transactions.
  * We've added a comment to remind us which route this matches. This is very helpful especially as we add more routes to this controller.

```code
    // GET: api/Transactions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        return await _context.Transactions.ToListAsync();
    }
```

* Run the `Financials` project and test the `api/financials` route with a GET request using Postman.
  * Explain that we are getting an empty array because there is no data yet.
  * Highlight the fact that this application is using an in-memory database so each time we stop and start again we'll lose any data that we have. For real-world applications we would use a persistent database instead.

* Create a method to create a new transaction. Highlight the `[HttpPost]` attribute. This is telling ASP.NET Core that POST requests will match this route. The `PostTransactions` method has a `Transaction` object as a parameter. So this will match POST requests to api/Transactions and the body of the request will be used to populate the `transaction` parameter.
  * Put a breakpoint in the method and test the route through Postman. Highlight the fact that ASP.NET Core is populating the object from the request body.

```code
    [HttpPost]
    public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return transaction;
    }
```

* Run the `Financials` project and test the `api/financials` route with a POST request using Postman.
  * Test the `api/financials` route with a GET request using Postman. We should now see the transaction we created in the response.
  * Real-world we would also want to test with bad data as well.

* Create a method to update an existing transaction. Highlight the `[HttpPut]` attribute. This is telling ASP.NET Core that PUT requests will match this route. The `[HttpPut]` attribute also has an id in curly braces. ASP.NET Core will look for data after the `api/Transactions/` and place the value it finds in the id parameter. The `PutTransaction` method has paramters for both an id and a `Transaction` object. We've told ASP.NET Core to get the id parameter from the route and the body of the request will be used to populate the `transaction` parameter.

```code
    // PUT: api/Transactions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
    {
        if (id != transaction.TransactionId)
        {
            return BadRequest();
        }

        _context.Entry(transaction).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }
```

* Run the `Financials` project and test the `api/financials` route with a PUT request using Postman.
  * Explain that we need to pass the id of the transaction we want to update as part of the route.
  * Test the `api/financials` route with a GET request using Postman. We should now see the updated transaction included in the response.

* Create a method to delete a transaction. Highlight the `[HttpDelete]` attribute. This is telling ASP.NET Core that DELETE requests will match this route. The `[HttpDelete]` attribute has an id in curly braces just like the `[HttpPut]` attribute. The `DeleteTransaction` method will receive this id as a parameter.
  * Explain that to delete a transaction we just need its id.
  * Point out that we are only soft deleting the transaction. A soft delete marks the transaction as deleted but keeps it in the database. A hard delete would remove it from the database completely.

```code
    // DELETE: api/Transactions/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Transaction>> DeleteTransaction(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }

        // mark the transaction as deleted
        transaction.SoftDelete = true;

        // NOTE: for a hard delete use the commented code below.
        //_context.Transactions.Remove(transaction);

        await _context.SaveChangesAsync();

        return transaction;
    }
```

* Run the `Financials` project and test the `api/financials` route with a DELETE request using Postman.
  * Test the `api/financials` route with a GET request using Postman. The `deleted` transaction will still be included in the response but its `SoftDelete` property will be set to `true`.

* Create a method to get the total value of all transaction. Highlight the `[HttpGet]` attribute. We're passing a string value of "Total". This is telling ASP.NET Core that GET requests to `api/Transactions/Total` will match this route.
  * Explain that we're using LINQ to exclude soft deleted transactions and return the sum of the remaining transactions.

```code
    // GET: api/Transactions/Total
    [HttpGet("Total")]
    public async Task<decimal> GetTransactionTotal()
    {
        return await _context.Transactions
            .Where(t => t.SoftDelete == false) // don't include transactions that have been soft deleted
            .Select(t => t.TransactionValue)
            .SumAsync();
    }
```

* Run the `Financials` project and test the `api/financials/total` route with a GET request using Postman.

* Take time to answer any remaining questions before moving on.

</details>

- - -


namespace Financials.Controllers
{
    using Financials.Data;
    using Financials.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly FinancialsContext _context;

        public TransactionsController(FinancialsContext context)
        {
            _context = context;
        }

        #region Create

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        #endregion

        #region Read

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/Total
        [HttpGet("Total")]
        public async Task<decimal> GetTransactionTotal()
        {
            return await _context.Transactions
                .Where(t => t.SoftDelete == false) // don't include transactions that have been soft deleted
                .Select(t => t.TransactionValue)
                .SumAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        #endregion

        #region Update

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

        #endregion

        #region Delete

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

        #endregion
    }
}

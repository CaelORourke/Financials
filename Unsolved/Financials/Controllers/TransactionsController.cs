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
        #endregion

        #region Read
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion
    }
}

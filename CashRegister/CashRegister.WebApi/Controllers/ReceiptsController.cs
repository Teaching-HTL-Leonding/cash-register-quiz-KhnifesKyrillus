using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CashRegister.Data;
using CashRegister.SharedResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashRegister.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptsController : ControllerBase
    {
        private readonly CashRegisterDataContext _dataContext;

        public ReceiptsController(CashRegisterDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<ReceiptLineDto> receiptLineDto)
        {
            if (receiptLineDto.Count == 0) return BadRequest("Missing receipt lines");

            var products = new Dictionary<int, Product>();
            foreach (var rl in receiptLineDto)
                products[rl.ProductID] = await _dataContext.Products.FirstOrDefaultAsync(p => p.ID == rl.ProductID);

            var r = new Receipt
            {
                ReceiptTimestamp = DateTime.UtcNow,
                ReceiptLines = receiptLineDto.Select(rl => new ReceiptLine
                {
                    ID = 0,
                    Product = products[rl.ProductID],
                    Amount = rl.Amount,
                    TotalPrice = rl.Amount * products[rl.ProductID].UnitPrice
                }).ToList()
            };
            r.TotalPrice = r.ReceiptLines.Sum(rl => rl.TotalPrice);

            await _dataContext.Receipts.AddAsync(r);
            await _dataContext.SaveChangesAsync();

            return StatusCode((int) HttpStatusCode.Created, r);
        }
    }
}
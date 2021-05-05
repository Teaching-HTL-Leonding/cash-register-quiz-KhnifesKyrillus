using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashRegister.Data;
using CashRegister.SharedResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashRegister.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly CashRegisterDataContext _dataContext;

        public ProductsController(CashRegisterDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get([FromQuery] string? nameFilter = null)
        {
            IQueryable<Product> products = _dataContext.Products;

            if (!string.IsNullOrEmpty(nameFilter)) products = products.Where(p => p.ProductName.Contains(nameFilter));

            return await products.ToListAsync();
        }
    }
}
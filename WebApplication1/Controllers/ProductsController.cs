using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1Front.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _context.Products.Include(x => x.Genre).ToListAsync());
        }

        //GET: api/ProductsByGenre
        [HttpGet("{genreID}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByGenre(int genreID)
        {
            return Ok(await _context.Products.Include(x => x.Genre).Where(x => x.GenreID == genreID).ToListAsync());
        }

        //GET: api/ProductsBySearchParameters
        [HttpGet("{name}/{minprice}/{maxprice}/{genreID}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsBySearchParameters(string name, int? minprice, int? maxprice, int genreID)
        {
            var products = _context.Products.Include(x => x.Genre).AsQueryable();
            if (name != null && name != string.Empty)
                products = products.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            if (minprice != null)
                products = products.Where(x => x.Price >= minprice);
            if (maxprice != null)
                products = products.Where(x => x.Price <= maxprice);
            if (maxprice != null && minprice != null)
                products = products.Where(x => x.Price <= minprice && x.Price >= maxprice);
            if (genreID > 0)
                products = products.Where(x => x.GenreID == genreID);

            return Ok(await products.ToListAsync());
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(x => x.Genre).FirstOrDefaultAsync(x => x.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductViewModel model)
        {
            if (model == null)
                return BadRequest();
            if (model.Email == null || model.Email == string.Empty)
                return BadRequest();
            if (model.Price <= 0)
                return BadRequest();
            if (model.GenreID <= 0)
                return BadRequest();

            var product = new Product
            {
                Name = model.Email,
                Price = model.Price,
                ImagePath = model.ImagePath,
                GenreID = model.GenreID
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}

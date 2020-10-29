using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1Front.Services;
using WebApplication1Front.ViewModels;

namespace WebApplication1Front.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IGenreService _genreService;
        private readonly IOrderService _orderService;
        public ProductsController(IProductService productService, IGenreService genreService, IOrderService orderService)
        {
            _productService = productService;
            _genreService = genreService;
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();

            await LoadGenres();

            return View(products);
        }
        public async Task<IActionResult> Create(string name, decimal price, int stock, int genreid, IFormFile file)
        {
            var model = new ProductViewModel
            {
                Email = name,
                Price = price,
                Stock = stock,
                GenreID = genreid
            };

            await _productService.CreateProduct(model, file);

            return View(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProduct(id);
            var stock = new List<int>();
            for (int i = 1; i <= product.Stock; i++)
            {
                stock.Add(i);
            }
            ViewBag.Amount = new SelectList(stock);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Details(int id, int amount)
        {
            await _orderService.PlaceOrder(HttpContext.User.Identity.Name, id, amount);
            return View(nameof(Details), id);
        }
        private async Task LoadGenres()
        {
            var genres = await _genreService.GetGenres();
            ViewBag.Genres = new SelectList(genres, "GenreID", "Name");
        }
    }
}

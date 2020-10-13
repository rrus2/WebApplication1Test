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
        public ProductsController(IProductService productService, IGenreService genreService)
        {
            _productService = productService;
            _genreService = genreService;
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
        private async Task LoadGenres()
        {
            var genres = await _genreService.GetGenres();
            ViewBag.Genres = new SelectList(genres, "GenreID", "Name");
        }
    }
}

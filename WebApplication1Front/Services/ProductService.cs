using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplication1Front.Models;
using WebApplication1Front.ViewModels;

namespace WebApplication1Front.Services
{
    public class ProductService : IProductService
    {
        private readonly IWebHostEnvironment _env;
        public ProductService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<Product> CreateProduct(ProductViewModel model, IFormFile file)
        {
            if (file != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images");
                var fileName = Guid.NewGuid().ToString() + file.FileName;
                using (var stream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    var imagePath = "images/" + fileName;
                    model.ImagePath = imagePath;
                }
            }
            using (var client = new HttpClient())
            {
                var serial = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var content = new StringContent(serial, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44387/api/Products", content);
                if (response.IsSuccessStatusCode)
                {
                    var str = await response.Content.ReadAsStringAsync();
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(str);
                    return obj;
                }
                else
                    return new Product();
            }
        }

        public Task<Product> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProduct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44387/api/Products");
                var str = await response.Content.ReadAsStringAsync();
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(str);
                return obj;
            }
        }

        public Task<Product> UpdateProduct(int id, ProductViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}

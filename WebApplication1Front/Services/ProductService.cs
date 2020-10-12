using System;
using System.Collections.Generic;
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
        public async Task<Product> CreateProduct(ProductViewModel model)
        {
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

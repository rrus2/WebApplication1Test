using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1Front.Models;
using WebApplication1Front.ViewModels;

namespace WebApplication1Front.Services
{
    public interface IProductService
    {
        Task<Product> CreateProduct(ProductViewModel model);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task<Product> UpdateProduct(int id, ProductViewModel model);
        Task<Product> DeleteProduct(int id);
    }
}

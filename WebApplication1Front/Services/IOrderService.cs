using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1Front.Models;

namespace WebApplication1Front.Services
{
    public interface IOrderService
    {
        Task<Order> PlaceOrder(string username, int productid, int amount);
        Task<Order> DeleteOrder(string username, int productid);
        Task<IEnumerable<Order>> GetOrdersByUser(string username);
        Task<IEnumerable<Order>> GetAllOrders();
    }
}

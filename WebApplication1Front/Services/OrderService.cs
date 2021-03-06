﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplication1Front.Models;

namespace WebApplication1Front.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        public OrderService(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }
        public Task<Order> DeleteOrder(string username, int productid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrdersByUser(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> PlaceOrder(string userid, int productid, int amount)
        {
            using (var client = new HttpClient())
            {
                var user = await _userService.GetUser(userid);
                var product = await _productService.GetProduct(productid);
                var order = new Order()
                {
                    ApplicationUser = user,
                    Product = product,
                    Amount = amount
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:44387/api/orders", content);
                if (response.IsSuccessStatusCode)
                {
                    var str = await response.Content.ReadAsStringAsync();
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(str);
                    return obj;
                }
                else
                    return null;
            };
        }
    }
}

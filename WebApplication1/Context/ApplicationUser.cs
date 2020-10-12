using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime Birthdate { get; set; }
        public int OrderID { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public ApplicationUser()
        {
            Orders = new HashSet<Order>();
        }
    }
}

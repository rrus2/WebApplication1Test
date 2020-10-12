using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class ConfigClass
    {
        public ConfigClass(ModelBuilder builder)
        {
            // ProductsConfig
            builder.Entity<Product>().HasKey(x => x.ProductID);
            builder.Entity<Product>().Property(x => x.Name).IsRequired();
            builder.Entity<Product>().Property(x => x.Price).IsRequired();
            builder.Entity<Product>().Property(x => x.ImagePath);
            builder.Entity<Product>().HasMany(x => x.Orders).WithOne(x => x.Product).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Product>().HasOne(x => x.Genre).WithOne().IsRequired();

            //GenresConfig
            builder.Entity<Genre>().HasKey(x => x.GenreID);
            builder.Entity<Genre>().Property(x => x.Name).IsRequired();

            //ApplicationUsersConfig
            builder.Entity<ApplicationUser>().HasKey(x => x.Id);
            builder.Entity<ApplicationUser>().Property(x => x.Email).IsRequired();
            builder.Entity<ApplicationUser>().Property(x => x.UserName).IsRequired();
            builder.Entity<ApplicationUser>().Property(x => x.Birthdate).IsRequired();

            //OrdersConfig
            builder.Entity<Order>().HasKey(x => x.OrderID);
            builder.Entity<Order>().HasOne(x => x.Product).WithMany(x => x.Orders).HasForeignKey(x => x.ProductID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Order>().HasOne(x => x.ApplicationUser).WithMany(x => x.Orders).HasForeignKey(x => x.ApplicationUserID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Order>().Property(x => x.Amount).IsRequired();

            //ShoppingCartConfig
            builder.Entity<ShoppingCart>().HasKey(x => x.ShoppingCartID);
            builder.Entity<ShoppingCart>().HasOne(x => x.Product).WithMany().IsRequired();
            builder.Entity<ShoppingCart>().HasOne(x => x.ApplicationUser).WithMany().IsRequired();
            builder.Entity<ShoppingCart>().Property(x => x.Amount).IsRequired();
        }
    }
}

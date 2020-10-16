using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Context;

namespace WebApplication1Tests.APItests.Fixture
{
    public class DbFixture
    {
        public ServiceProvider Provider { get; set; }
        public DbFixture()
        {
            var service = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(x => x.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()))
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            Provider = service.Services.AddLogging().BuildServiceProvider();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApplication1.Context;
using WebApplication1.Controllers;
using WebApplication1.ViewModels;
using WebApplication1Front.ViewModels;
using WebApplication1Tests.APItests.Fixture;
using Xunit;

namespace WebApplication1Tests.APItests.UserAPITests
{
    public class UserAPITests : IDisposable, IClassFixture<DbFixture>
    {
        private readonly ServiceProvider _provider;
        private readonly UsersController _controller;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserAPITests()
        {
            _provider = new DbFixture().Provider;

            _signInManager = _provider.GetService<SignInManager<ApplicationUser>>();
            _userManager = _provider.GetService<UserManager<ApplicationUser>>();
            _roleManager = _provider.GetService<RoleManager<IdentityRole>>();

            _controller = new UsersController(_userManager, _signInManager);

            SeedRoles();
            SeedUsers();
        }

        [Fact]
        public async void CreateUserWorks()
        {
            var model = new ApplicationUserViewModel
            {
                Email = "test@test.com",
                Birthdate = Convert.ToDateTime("30/07/1991"),
                Password = "testT_12345!",
                RepeatPassword = "testT_12345!"
            };

            var actionresult = await _controller.PostUser(model);
            var value = actionresult.Result as OkObjectResult;
            var user = value.Value as ApplicationUser;
            var newUser = await _userManager.FindByNameAsync("test@test.com");

            Assert.NotNull(actionresult);
            Assert.Equal(user.Email, newUser.Email);
        }
        [Theory]
        [InlineData("", "30/07/1991", "testT_12345!", "testT_12345!")]
        [InlineData(null, "30/07/1991", "testT_12345!", "testT_12345!")]
        [InlineData("ok", "30/07/1991", "testT_12345!", "testT_12345!")]
        [InlineData("ok3@ok3.com", "30/07/1991", "", "testT_12345!")]
        [InlineData("ok3@ok3.com", "30/07/1991", "testT_12345!", "")]
        [InlineData("ok3@ok3.com", "30/07/1991", null, "")]
        [InlineData("ok3@ok3.com", "30/07/1991", "testT_12345!", null)]
        public async void CreateUserFailsWithBadData(string email, string birthdate, string password, string repeatpassword)
        {
            var model = new ApplicationUserViewModel
            {
                Email = email,
                Birthdate = Convert.ToDateTime(birthdate),
                Password = password,
                RepeatPassword = repeatpassword
            };

            var result = await _controller.PostUser(model);
            var r = result.Result as BadRequestResult;

            Assert.Equal(r.StatusCode, StatusCodes.Status400BadRequest);
        }
        [Fact]
        public async void CreateUserFailsWithFailModel()
        {
            var failmodel = new ApplicationUserViewModel();

            var result = await _controller.PostUser(failmodel);
            var r = result.Result as BadRequestResult;

            Assert.Equal(r.StatusCode, StatusCodes.Status400BadRequest);
        }
        [Fact]
        public async void GetUsersWorks()
        {
            var result = await _controller.GetUsers();
            var ok = result.Result as OkObjectResult;
            var users = ok.Value as IEnumerable<ApplicationUser>;

            Assert.Equal(2, users.Count());
        }
        [Fact]
        public async void LoginUserWorks()
        {
            var loginVM = new LoginViewModel
            {
                Email = "ok@ok.com",
                Password = "testT_12345!"
            };

            var result = await _controller.LoginUser(loginVM);
            var ok = result.Result as OkObjectResult;
            var user = ok.Value as ApplicationUser;

            Assert.NotNull(user);
            Assert.Equal(loginVM.Email, user.Email);
        }
        [Theory]
        [InlineData("troll", "troll")]
        [InlineData(null, "troll")]
        [InlineData("troll", null)]
        [InlineData(null, null)]
        public async void LoginUserFailsWithFailLogin(string email, string password)
        {
            var result = await _controller.LoginUser(new LoginViewModel { Email = email, Password = password });
            var ok = result.Result as BadRequestResult;

            Assert.Equal(ok.StatusCode, StatusCodes.Status400BadRequest);
        }
        [Fact]
        public async void GetUsersRoleVerification()
        {
            var result = await _controller.GetUsers();
            var ok = result.Result as OkObjectResult;
            var users = ok.Value as IEnumerable<ApplicationUser>;

            var firstUser = await _userManager.GetRolesAsync(users.First());
            var secondUser = await _userManager.GetRolesAsync(users.Last());

            Assert.Equal("Admin", firstUser.First());
            Assert.Equal("User", secondUser.First());
        }
        private async void SeedRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
        }
        private async void SeedUsers()
        {
            var user1 = new ApplicationUserViewModel { Email = "ok@ok.com", Birthdate = Convert.ToDateTime("30/07/1991"), Password = "testT_12345!", RepeatPassword = "testT_12345!" };
            var user2 = new ApplicationUserViewModel { Email = "ok2@ok2.com", Birthdate = Convert.ToDateTime("30/07/1991"), Password = "testT_12345!", RepeatPassword = "testT_12345!" };

            await _controller.PostUser(user1);
            await _controller.PostUser(user2);
        }
        public void Dispose()
        {
            _provider.Dispose();
        }
    }
}

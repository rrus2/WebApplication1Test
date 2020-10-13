using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using WebApplication1Front.ViewModels;
using WebApplication1Front.Services;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1Front.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGenreService _genreService;
        private HomeController _homeController;
        public UserController(IUserService userService, IGenreService genreService)
        {
            _userService = userService;
            _genreService = genreService;
            _homeController = new HomeController(null);
        }
        public IActionResult Index()
        {
            return View(_homeController.IndexAsync());
        }
        public async Task<IActionResult> Register(string email, string birthdate, string password, string repeatpassword)
        {
            var model = new ApplicationUserViewModel
            {
                Email = email,
                Birthdate = Convert.ToDateTime(birthdate),
                Password = password,
                RepeatPassword = repeatpassword
            };

            var result = await _userService.CreateUser(model);
            if (result == null)
                return View(nameof(Index));

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, result.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims);
            var principle = new ClaimsPrincipal(claimsIdentity);

            HttpContext.User = principle;

            return View(nameof(Index));
        }
        public async Task<IActionResult> Login(string email, string password)
        {
            var model = new LoginViewModel { Email = email, Password = password };

            var user = await _userService.LoginUser(model);
            if (user == null)
                return View(nameof(Index));

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims);
            var principle = new ClaimsPrincipal(claimsIdentity);

            HttpContext.User = principle;

            return View(nameof(Index));
        }
        public IActionResult Logout()
        {
            HttpContext.User = null;

            return View(nameof(Index));
        }
    }
}

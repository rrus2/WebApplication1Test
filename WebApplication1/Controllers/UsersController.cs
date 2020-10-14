using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.ViewModels;
using WebApplication1Front.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("getrolesbyuser")]
        public async Task<ActionResult<List<string>>> GetRolesByUser(ApplicationUser user)
        {
            if (user == null)
                return BadRequest();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles.ToList());
        }

        [HttpPost("loginuser")]
        public async Task<ActionResult<ApplicationUser>> LoginUser(LoginViewModel model)
        {
            if (model == null)
                return BadRequest();
            if (model.Email == null || model.Email == string.Empty)
                return BadRequest();
            if (model.Password == null || model.Password == string.Empty)
                return BadRequest();

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
                return BadRequest();
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
                return Ok(user);
            else
                return BadRequest();
        } 
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return Ok(await _userManager.Users.ToListAsync());
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            return Ok(await _userManager.FindByIdAsync(id));
        }

        // POST api/<UsersController>
        [HttpPost("postuser")]
        public async Task<ActionResult<ApplicationUser>> PostUser(ApplicationUserViewModel model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                Birthdate = model.Birthdate
            };

            var result = await _userManager.CreateAsync(user, model.RepeatPassword);
            if (result.Succeeded)
            {
                var userToReturn = await _userManager.FindByNameAsync(user.UserName);
                if (_userManager.Users.Count() == 0)
                    await _userManager.AddToRoleAsync(userToReturn, "Admin");
                else
                    await _userManager.AddToRoleAsync(userToReturn, "User");

                return Ok(userToReturn);
            }
            else
                return BadRequest();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApplicationUser>> PutUser(string id, ApplicationUserViewModel model)
        {
            if (id == string.Empty || id == null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest();

            var result = await _userManager.ResetPasswordAsync(user, "", model.RepeatPassword);
            if (!result.Succeeded)
                return BadRequest();

            user.Email = model.Email;
            user.UserName = model.Email;
            user.Birthdate = model.Birthdate;

           var reset = await _userManager.AddPasswordAsync(user, model.RepeatPassword);
           if (!reset.Succeeded)
                return BadRequest();
            
            return NoContent();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

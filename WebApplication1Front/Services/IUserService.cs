using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1Front.ViewModels;
using WebApplication1Front.Models;

namespace WebApplication1Front.Services
{
    public interface IUserService
    {
        Task<ApplicationUserViewModel> CreateUser(ApplicationUserViewModel model);
        Task<ApplicationUser> LoginUser(LoginViewModel model);
        Task<ApplicationUserViewModel> DeleteUser(ApplicationUserViewModel model);
        Task<IEnumerable<ApplicationUserViewModel>> GetUsers();
        Task<List<Claim>> GetClaimsByUser(ApplicationUser user);
    }
}

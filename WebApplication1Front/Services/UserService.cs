using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebApplication1Front.Models;
using WebApplication1Front.ViewModels;

namespace WebApplication1Front.Services
{
    public class UserService : IUserService
    {
        public async Task<ApplicationUserViewModel> CreateUser(ApplicationUserViewModel model)
        {
            using (var client = new HttpClient())
            {
                var content = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44387/api/users/postuser", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var obj = await response.Content.ReadAsStringAsync();
                    var user = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationUserViewModel>(obj);
                    return user;
                }
                else
                {
                    return new ApplicationUserViewModel();
                }
            }
        }

        public Task<ApplicationUserViewModel> DeleteUser(ApplicationUserViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Claim>> GetClaimsByUser(ApplicationUser user)
        {
            using (var client = new HttpClient())
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44387/api/users/getclaims", content);
                if (response.IsSuccessStatusCode)
                {
                    var str = await response.Content.ReadAsStringAsync();
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Claim>>(str);
                    return obj;
                }
                else
                {
                    return new List<Claim>();
                }
            }
        }

        public Task<IEnumerable<ApplicationUserViewModel>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> LoginUser(LoginViewModel model)
        {
            using (var client = new HttpClient())
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44387/api/users/loginuser", content);
                if (response.IsSuccessStatusCode)
                {
                    var str = await response.Content.ReadAsStringAsync();
                    var user = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationUser>(str);
                    return user;
                }
                else
                    return new ApplicationUser();
            }
        }
    }
}

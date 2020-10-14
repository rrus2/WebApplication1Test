using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1Front.Views.Shared
{
    public class IndexModel : PageModel
    {
        public string Role { get; set; }
        private readonly IHttpContextAccessor _httpContext;
        public IndexModel(IHttpContextAccessor _httpContext)
        {
            this._httpContext = _httpContext;
        }
        public void OnGet()
        {
            if (HttpContext.User.Identity.Name != null)
                Role = "Admin";
            if (_httpContext.HttpContext.User.Identity.Name != null)
                Role = "Admin";
        }
    }
}

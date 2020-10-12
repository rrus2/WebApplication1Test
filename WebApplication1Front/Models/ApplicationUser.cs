using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1Front.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
    }
}

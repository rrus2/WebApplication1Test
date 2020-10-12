using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1Front.Models;

namespace WebApplication1Front.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetGenres();
    }
}

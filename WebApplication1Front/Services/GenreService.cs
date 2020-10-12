using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication1Front.Models;

namespace WebApplication1Front.Services
{
    public class GenreService : IGenreService
    {
        public async Task<IEnumerable<Genre>> GetGenres()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync("https://localhost:44387/api/Genres");
                var str = await result.Content.ReadAsStringAsync();
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Genre>>(str);
                return obj;
            }
        }
    }
}

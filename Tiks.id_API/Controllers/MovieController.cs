using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tiks.id_API.Models;

namespace Tiks.id_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        TiksIdContext ctx = new TiksIdContext();
        public MovieController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpGet]
        public IActionResult GetAllMovies()
        {
            var data = ctx.Movies.OrderByDescending(s => s.ReleaseDate).Select(s => new
            {
                s.Id,
                s.Title,
                s.Duration,
                s.ReleaseDate,
                genre = ctx.MovieGenres.OrderBy(x => x.Genre.Name).First(x => x.MovieId == s.Id).Genre.Name
            });
            return Ok(data);
        }

        [HttpGet("{movieId}")]
        public IActionResult GetDetailsMovie(int movieId)
        {
            var data = ctx.Movies.Select(s => new
            {
                s.Id,
                s.Title,
                s.Description,
                s.Duration,
                s.ReleaseDate,
                genre = ctx.MovieGenres.OrderBy(x => x.Genre.Name).Where(x => x.MovieId == s.Id).Select(s=>s.Genre.Name).ToList(),
            }).First(s=>s.Id == movieId);
            return Ok(data);
        }

        [HttpGet("{movieId}/Poster")]
        public IActionResult GetMoviePoster(int movieId)
        {
            var poster = ctx.Movies.Find(movieId);
            if (poster == null) return NotFound();
            var path = Path.Combine(env.WebRootPath, "images", "poster", poster.Poster);
            var photo = System.IO.File.OpenRead(path);
            return File(photo, "image/*");
        }

        [HttpGet("Popular")]
        public IActionResult GetPopularMovie()
        {
            var data = ctx.Transactions.Where(s=>s.TransactionDate >= DateTime.Now.AddDays(-7) && s.TransactionDate <= DateTime.Now).Select(s => new
            {
                s.Schedule.Movie.Id,
                s.Schedule.Movie.Title,
                s.Schedule.Movie.Duration,
                s.Schedule.Movie.ReleaseDate,
                genre = ctx.MovieGenres.OrderBy(x => x.Genre.Name).First(x => x.MovieId == s.Schedule.MovieId).Genre.Name,
                soldTicket = ctx.TransactionDetails.Count(x=>x.Transaction.Schedule.MovieId == s.Schedule.MovieId),
                status = "Popular this week"
            }).Distinct().OrderByDescending(s=>s.soldTicket).ToList();
            if (data.Count != 1)
            {
                var recentlyReleased = ctx.Movies.OrderByDescending(s => s.ReleaseDate).Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Duration,
                    s.ReleaseDate,
                    genre = ctx.MovieGenres.OrderBy(x => x.Genre.Name).First(x => x.MovieId == s.Id).Genre.Name,
                    soldTicket = ctx.TransactionDetails.Count(x=>x.Transaction.Schedule.MovieId == s.Id),
                    status = "Recently released"
                }).First();
                if (data.Count < 1) return Ok(recentlyReleased);
                if (data[0].soldTicket == data[1].soldTicket) return Ok(recentlyReleased);
            }
            return Ok(data.First());
        }
    }
}

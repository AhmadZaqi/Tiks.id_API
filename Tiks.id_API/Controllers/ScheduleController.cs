using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tiks.id_API.Models;

namespace Tiks.id_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        TiksIdContext ctx = new TiksIdContext();

        [HttpGet("{movieId}")]
        public IActionResult getMovieSchedule(int movieId)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            var timeNow = TimeOnly.FromDateTime(DateTime.Now);
            var schedule = ctx.Schedules.Where(x => x.MovieId == movieId && x.Date > dateNow && x.Time > timeNow)
                .GroupBy(x => new { x.TheaterId, x.Theater.Name })
                .Select(x => new
                {
                    theaterName = x.Key.Name,
                    ctx.Theaters.First(a => a.Id == x.Key.TheaterId).Section,
                    ctx.Theaters.First(a => a.Id == x.Key.TheaterId).Column,
                    ctx.Theaters.First(a => a.Id == x.Key.TheaterId).Row,
                    availableDate = x.GroupBy(a => new { a.Date }).OrderBy(s=>s.Key.Date).Select(a => new
                    {
                        a.Key.Date,
                        availableTime = a.OrderBy(s=>s.Time).Select(c => new
                        {
                            scheduleId = c.Id,
                            c.Time,
                            c.Price,
                            filledSeat = ctx.TransactionDetails.Where(f => f.Transaction.ScheduleId == c.Id).Select(f => f.Seat).ToList()
                        })
                    }),
                });
            return Ok(schedule);
        }
    }
}

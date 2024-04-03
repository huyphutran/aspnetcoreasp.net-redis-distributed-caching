using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using static net_redis.ServiceOneClass;

namespace net_redis.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDistributedCache _cache;

        public IActionResult Index()
        {
            return View();
        }

        public HomeController(IDistributedCache cache)
        {
             _cache = cache;
        }


        [HttpGet("/one")]
        public async Task<IActionResult> One([FromServices] ServiceOneClass one, [FromQuery] int n = 1)
        {
            var value = await _cache.GetStringAsync(n.ToString());

            if (string.IsNullOrEmpty(value))
            {
                var car = await one.GetCarAsync(n);
                await _cache.SetStringAsync(n.ToString(), JsonSerializer.Serialize(car));
                return Ok(
                    new
                    {
                        cached = false,
                        data = car
                    });
            }
            return Ok(new
            {
                cached = false,
                data = JsonSerializer.Deserialize<Car>(value)
            });
        }
    }
}

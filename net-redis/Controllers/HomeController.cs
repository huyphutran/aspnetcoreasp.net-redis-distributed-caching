using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static net_redis.ServiceOneClass;
using Microsoft.Extensions.Caching.Distributed;


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
            var res = _cache.GetorAddAsync(n, one.GetCarAsync);
            return Ok(res.Result);
        }



        [HttpGet("/two")]
        public async Task<IActionResult> Two([FromServices] ServiceTwoClass two, [FromQuery] string key = "foo")
        {
            var res = _cache.GetorAddAsync(key, two.GetNameAsync);
            return Ok(res.Result);
        }


        [HttpGet("/three")]
        public async Task<IActionResult> Three([FromServices] ServiceThreeClass three)
        {
            Dude dude = new Dude(1, "Paul");
             await _cache.SetAsync(dude);
            await three.SaveDude(dude);
            return Ok();
        }
    }
}

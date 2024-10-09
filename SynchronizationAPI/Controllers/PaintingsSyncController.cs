using Microsoft.AspNetCore.Mvc;
using PaintingLibrary.Models;

namespace SynchronizationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaintingsSyncController : ControllerBase
    {
        private readonly ILogger<PaintingsSyncController> _logger;

        public PaintingsSyncController(ILogger<PaintingsSyncController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<Painting> Get()
        {
            return new List<Painting>();
        }
    }
}

using CoreAngularDevTest.Server.Interfaces;
using CoreAngularDevTest.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAngularDevTest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        private readonly IGoogleService _googleService;
        public GoogleController(IGoogleService googleService)
        {
            _googleService = googleService ?? throw new ArgumentNullException(nameof(googleService));
        }

        [HttpGet]
        public async Task<IActionResult> GetLocationInfo([FromQuery] string lat, [FromQuery] string lng, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(lat) || string.IsNullOrWhiteSpace(lng))
                return BadRequest("Latitude and longitude are required.");

            var info = await _googleService.GetLocationInfo(lat, lng, token);

            if (info == null)
                return NotFound("Location not found");

            return Ok(info);
        }

        [HttpGet("{location}")]
        public async Task<IActionResult> GetGeoInfo(string location, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(location))
                return BadRequest("Latitude and longitude are required.");

            var info = await _googleService.GetGeoInfoByLocationName(location, token);

            if (info == null)
                return NotFound("Location not found");

            return Ok(info);
        }
    }
}

using CoreAngularDevTest.Server.Models.DTO.FourSquare.SearchDTO;
using CoreAngularDevTest.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAngularDevTest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoursquareController : ControllerBase
    {
        private readonly IFoursquareService _foursquareService;

        public FoursquareController(IFoursquareService foursquareService)
        {
            _foursquareService = foursquareService;
        }

        [HttpGet("{place}")]
        public async Task<IActionResult> GetCoordinates(string place, CancellationToken token)
        {
            var result = await _foursquareService.GetPlaceCoordinatesAsync(place, token);
            if (result == null)
                return NotFound("Location not found");

            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetFourSquareAttractionbyPlace([FromQuery] string lat, [FromQuery] string lng, CancellationToken token)
        {
            var result = await _foursquareService.GetAttractionListByCoordinateAsync(lat, lng, token);
            if (result == null)
                return NotFound("Location not found");

            return Ok(result);

        }
    }
}

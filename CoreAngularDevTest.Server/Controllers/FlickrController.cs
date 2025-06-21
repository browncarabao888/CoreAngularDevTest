using CoreAngularDevTest.Server.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAngularDevTest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlickrController : ControllerBase
    {
        private readonly IFlickr _flickrService;

        #region Constructor
        public FlickrController(IFlickr flickrService)
        {
            _flickrService = flickrService;
            
        }
        #endregion

        [HttpGet("{placeName}")]
        public async Task<IActionResult> PhotosSearch(string placeName, CancellationToken token)
        {
            var result = await _flickrService.PhotoSearch(placeName, token);
            if (result == null)
                return NotFound("Location not found");

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPhotoInfo([FromQuery] List<string?>? ids, CancellationToken token)
        {
            var result = await _flickrService.GetImageInfoAsync(ids, token);
            if (result == null)
                return NotFound("Location not found");

            return Ok(result);
        }
      
    }
}

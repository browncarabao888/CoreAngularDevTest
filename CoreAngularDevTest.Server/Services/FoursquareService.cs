using CoreAngularDevTest.Server.Interfaces;
using CoreAngularDevTest.Server.Models;
using CoreAngularDevTest.Server.Models.DTO.FlickPhotoSearch;
using CoreAngularDevTest.Server.Models.DTO.FourSquare;
using CoreAngularDevTest.Server.Models.DTO.FourSquare.SearchDTO;
using RestSharp;
using System.Text.Json;

namespace CoreAngularDevTest.Server.Services
{
    public class FoursquareService : IFoursquareService
    {
        private readonly string _apiKey = "fsq3oUYBkRyYEtR5RhXInMrt/Zjbz9mSHvN3CeXoeTOQ2+Y=";
        private readonly IFlickr _flickrService;

        public FoursquareService(IFlickr flickr)
        {
            _flickrService = flickr ?? throw new ArgumentNullException(nameof(flickr));
        }

        public async Task<FourSquareSearchDTO?> GetAttractionListByCoordinateAsync(string lat, string lng, CancellationToken token)
        {
            List<FlickrPhotoSearchDTO> photoListInfo = new();
            var result = new FourSquareSearchDTO();

            var url = $"https://api.foursquare.com/v3/places/search?ll={lat},{lng}&categories=10027&limit=5";
            var client = new RestClient(new RestClientOptions(url));
            var request = new RestRequest();

            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", _apiKey);
            token.ThrowIfCancellationRequested();

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                return null;

            token.ThrowIfCancellationRequested();
            result = JsonSerializer.Deserialize<FourSquareSearchDTO>(response.Content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var places = result?.results?
                .Select(s => s.name)
                .Distinct()
                .ToList();

            if (places == null || !places.Any())
                return result;

            photoListInfo = await _flickrService.PhotoSearchByListAsync(places , token) ?? new List<FlickrPhotoSearchDTO>();

            return result;

        }

        public async Task<FourSquareSearchDTO?> GetPlaceCoordinatesAsync(string place, CancellationToken token)
        {
            List<FlickrPhotoSearchDTO>? photoListInfo = null;
            var result = new FourSquareSearchDTO();
            double lng = 0;
            double lat = 0;

            if (place.Contains("undefined"))
                return result;

            var geoinfo = await GetCoordinatebyName(place, token);
            if (geoinfo?.context == null)
                return null;

            token.ThrowIfCancellationRequested();

            if (geoinfo != null && geoinfo?.context.geo_bounds?.circle?.center?.latitude != 0 && geoinfo?.context.geo_bounds?.circle?.center?.longitude != 0)
            {
                lat = geoinfo?.context.geo_bounds?.circle?.center?.latitude ?? 0.0;
                lng = geoinfo?.context.geo_bounds?.circle?.center?.longitude ?? 0.0; ;
            }

             
            var url = $"https://api.foursquare.com/v3/places/search?ll={lat},{lng}&categories=10027&limit=5";
            token.ThrowIfCancellationRequested();

            var client = new RestClient(new RestClientOptions(url));
            var request = new RestRequest();
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", _apiKey);

            token.ThrowIfCancellationRequested();
            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                return null;

            result = JsonSerializer.Deserialize<FourSquareSearchDTO>(response.Content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var places = result?.results?
                .Select(s => s.name)
                .Distinct()
                .ToList();

            if (places == null || !places.Any())
                return result;

            token.ThrowIfCancellationRequested();
            photoListInfo = await _flickrService.PhotoSearchByListAsync(places, token);

            return result;
        }

        private async Task<SearchByNameResultDTO?> GetCoordinatebyName(string place, CancellationToken token)
        {
            var result = new SearchByNameResultDTO();
            try
            {
                var encodedPlace = Uri.EscapeDataString(place);
                var url = $"https://api.foursquare.com/v3/places/search?query={encodedPlace}&limit=1";

                var client = new RestClient(new RestClientOptions(url));
                var request = new RestRequest();
                token.ThrowIfCancellationRequested();

                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", _apiKey);

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                    return null;

                token.ThrowIfCancellationRequested();
                result = JsonSerializer.Deserialize<SearchByNameResultDTO>(response.Content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null)
                    throw new InvalidOperationException("Deserialise fails."); 

            }
            catch (Exception)
            {
                throw;
            }
            return result;
             
        }

       
    }
}

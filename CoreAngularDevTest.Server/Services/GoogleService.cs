using CoreAngularDevTest.Server.Interfaces;
using CoreAngularDevTest.Server.Models;
using CoreAngularDevTest.Server.Models.DTO.FlickPhotoSearch;
using CoreAngularDevTest.Server.Models.DTO.FourSquare;
using CoreAngularDevTest.Server.Models.DTO.Google;
using RestSharp;
using System.Text.Json;

namespace CoreAngularDevTest.Server.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly string _apiKey = "AIzaSyAe5G5jq4BDS0dA6YD4DkLQdejY9kYwLhs";

        public async Task<GoogleGeoResponseDTO?> GetGeoInfoByLocationName(string loc, CancellationToken token)
        {
            var result = new GoogleGeoResponseDTO();
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={loc}&key={_apiKey}";
            token.ThrowIfCancellationRequested();

            var client = new RestClient(new RestClientOptions(url));
            var request = new RestRequest();
            token.ThrowIfCancellationRequested();

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                return null;

            result = JsonSerializer.Deserialize<GoogleGeoResponseDTO>(
                     response.Content ?? string.Empty,
                     new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

            if (result == null)
                throw new InvalidOperationException("Deserialize erropr — result is null.");

            return result;
        }

        public async Task<LocationInfo?> GetLocationInfo(string lat, string lng, CancellationToken token)
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lng}&key={_apiKey}";
            var client = new RestClient(new RestClientOptions(url));
            var request = new RestRequest();
            token.ThrowIfCancellationRequested();
            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                return null;

            var jsonResponse = response.Content;
            var locationInfo = ExtractCityAndState(jsonResponse);
            return locationInfo;
        }


        #region Helpers
        private LocationInfo ExtractCityAndState(string jsonResponse)
        {
            var location = new LocationInfo();

            using var doc = JsonDocument.Parse(jsonResponse);
            var results = doc.RootElement.GetProperty("results");

            foreach (var result in results.EnumerateArray())
            {
                if (result.TryGetProperty("address_components", out var components))
                {
                    foreach (var component in components.EnumerateArray())
                    {
                        if (component.TryGetProperty("types", out var types))
                        {
                            foreach (var type in types.EnumerateArray())
                            {
                                var value = type.GetString();
                                if (value == "locality" && location.City == null)
                                {
                                    location.City = component.GetProperty("long_name").GetString();
                                }
                                else if (value == "administrative_area_level_1" && location.State == null)
                                {
                                    location.State = component.GetProperty("long_name").GetString();
                                }

                                if (location.City != null && location.State != null)
                                    return location;
                            }
                        }
                    }
                }
            }

            return location;
        }

        private string? ExtractCity(string jsonResponse)
        {
            var doc = JsonDocument.Parse(jsonResponse);

            var results = doc.RootElement.GetProperty("results");

            foreach (var result in results.EnumerateArray())
            {
                if (result.TryGetProperty("address_components", out var components))
                {
                    foreach (var component in components.EnumerateArray())
                    {
                        if (component.TryGetProperty("types", out var types))
                        {
                            foreach (var type in types.EnumerateArray())
                            {
                                if (type.GetString() == "locality")
                                {
                                    return component.GetProperty("long_name").GetString();
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
        #endregion

    }
}


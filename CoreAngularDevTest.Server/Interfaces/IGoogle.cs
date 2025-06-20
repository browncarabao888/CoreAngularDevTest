
using CoreAngularDevTest.Server.Models;
using CoreAngularDevTest.Server.Models.DTO.Google;

namespace CoreAngularDevTest.Server.Interfaces
{
    public interface IGoogleService
    {
        Task<GoogleGeoResponseDTO?> GetGeoInfoByLocationName(string loc, CancellationToken token);
        Task<LocationInfo?> GetLocationInfo(string lat, string lng, CancellationToken token);
    }
}

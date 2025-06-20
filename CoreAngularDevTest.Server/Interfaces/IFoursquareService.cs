using CoreAngularDevTest.Server.Models.DTO.FourSquare;
using CoreAngularDevTest.Server.Models.DTO.FourSquare.SearchDTO;

namespace CoreAngularDevTest.Server.Services
{
    public interface IFoursquareService
    {
        Task<FourSquareSearchDTO?> GetAttractionListByCoordinateAsync(string lat, string lng, CancellationToken token);
        Task<FourSquareSearchDTO?> GetPlaceCoordinatesAsync(string place, CancellationToken token);
    }
}

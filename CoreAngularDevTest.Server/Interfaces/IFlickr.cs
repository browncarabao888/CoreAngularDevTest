using CoreAngularDevTest.Server.Models.DTO.FlickPhotoSearch;
using CoreAngularDevTest.Server.Models.DTO.Flickr;

namespace CoreAngularDevTest.Server.Interfaces
{
    public interface IFlickr
    {
        Task<List<FlickrImageInfoDTO>?> GetImageInfoAsync(List<string?>? photoId, CancellationToken token);
        Task<FlickrPhotoSearchDTO?> PhotoSearch(string placeName, CancellationToken token);
        Task<List<FlickrPhotoSearchDTO>?> PhotoSearchByListAsync(List<string?>? places, CancellationToken token);

        
    }
}

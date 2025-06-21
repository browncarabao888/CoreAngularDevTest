using CoreAngularDevTest.Server.Interfaces;
using CoreAngularDevTest.Server.Models;
using CoreAngularDevTest.Server.Models.DTO.FlickPhotoSearch;
using CoreAngularDevTest.Server.Models.DTO.Flickr;
using CoreAngularDevTest.Server.Models.DTO.FourSquare;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moq;
using RestSharp;
using System.Text.Json;
using System.Threading;
using Photo = CoreAngularDevTest.Server.Models.DTO.FlickPhotoSearch.Photo;

namespace CoreAngularDevTest.Server.Services
{
    public class FlickrService : IFlickr
    {
        #region Variables
        private readonly string _apiKey = "e108ebb0e668a6b1ccdbb17f56e0aafb";
        private readonly ApplicationDbContext _context;
        private readonly IServiceScopeFactory _scopeFactory;
        List<FlickrImageInfoDTO> flickrImageInfoList = new List<FlickrImageInfoDTO>();
        #endregion

        #region Constructor
        public FlickrService(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _scopeFactory = scopeFactory;
        }

        protected FlickrService()
        {
            _context = new Mock<ApplicationDbContext>().Object;
            _scopeFactory = new Mock<IServiceScopeFactory>().Object;
        }
        #endregion


        public virtual async Task<List<FlickrImageInfoDTO>?> GetImageInfoAsync(List<string?>? photoIds, CancellationToken token)
        {
            if (photoIds == null || !photoIds.Any())
                return null;

            var flickrImageInfoList = new List<FlickrImageInfoDTO>();
            var client = new RestClient(); // reuse one instance
            token.ThrowIfCancellationRequested();

            foreach (var id in photoIds)
            {
                token.ThrowIfCancellationRequested();
                var url = $"https://www.flickr.com/services/rest/?method=flickr.photos.getInfo&api_key={_apiKey}&photo_id={id}&format=json&nojsoncallback=1";

                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Accept", "application/json");

                try
                {
                    var response = await client.ExecuteAsync(request);

                    if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                    {
                        Console.WriteLine($"Failed to retrieve info for photo ID: {id}");
                        continue;
                    }

                    var imageInfo = JsonSerializer.Deserialize<FlickrImageInfoDTO>(response.Content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (imageInfo != null)
                        flickrImageInfoList.Add(imageInfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception while processing photo ID: {id}. Error: {ex.Message}");
                    continue;
                }
            }
            return flickrImageInfoList.Count > 0 ? flickrImageInfoList : null;
        }



        public virtual async Task<FlickrPhotoSearchDTO?> PhotoSearch(string placeName, CancellationToken token)
        {
            var result = new FlickrPhotoSearchDTO();
            try
            {
                var client = new RestClient();

                var url = $"https://www.flickr.com/services/rest/?method=flickr.photos.search&api_key={_apiKey}&text={placeName}&format=json&nojsoncallback=1&per_page=10&extras=url_m";
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Accept", "application/json");

                token.ThrowIfCancellationRequested();
                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                    Console.WriteLine($"Photos were not available for this Place : {placeName}");

                token.ThrowIfCancellationRequested();


                if (string.IsNullOrWhiteSpace(response.Content))
                    return null;

                result = JsonSerializer.Deserialize<FlickrPhotoSearchDTO>(response.Content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                result = await GetPhotoInfo(result, token);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while processing photo search. Error: {ex.Message}");
                throw;
            }
            return result;
        }

        public virtual async Task<FlickrPhotoSearchDTO?> GetPhotoInfo(FlickrPhotoSearchDTO? data, CancellationToken token)
        {
            var result = new FlickrPhotoSearchDTO();

            if (data == null || data.photos?.photo == null || !data.photos.photo.Any())
                return null;

            var flickrImageInfoList = new List<FlickrImageInfoDTO>();
            var client = new RestClient();

            foreach (var img in data.photos.photo)
            {

                token.ThrowIfCancellationRequested();
                var photoid = img.id;
                var url = $"https://www.flickr.com/services/rest/?method=flickr.photos.getInfo&api_key={_apiKey}&photo_id={photoid}&format=json&nojsoncallback=1";

                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Accept", "application/json");

                try
                {
                    token.ThrowIfCancellationRequested();
                    var response = await client.ExecuteAsync(request);

                    if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                        continue;

                    var imageInfo = JsonSerializer.Deserialize<FlickrImageInfoDTO>(response.Content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var imageUrl = img.url_m != null ? Path.GetFileName(img.url_m) : "";
                    img.title = imageInfo?.photo?.title?._content ?? string.Empty;
                    img.description = imageInfo?.photo?.description?._content ?? string.Empty;
                    img.owner = imageInfo?.photo?.owner?.username ?? string.Empty;
                    img.OwnerRealName = imageInfo?.photo?.owner?.realname ?? string.Empty;
                    img.dateuploaded = imageInfo?.photo?.dateuploaded ?? string.Empty;
                    img.filename = imageUrl;

                    token.ThrowIfCancellationRequested();
                    var success = await this.ProcessImage(img, img.url_m ?? "", token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception while processing photo ID: {img.id}. Error: {ex.Message}");
                    // Optionally log and continue
                    continue;
                }
            }

            return data;
        }

        public virtual async Task<bool> ProcessImage(Photo img, string url_m, CancellationToken token)
        {

            if (img == null || string.IsNullOrWhiteSpace(url_m))
                return false;

            var imgurl = url_m;
            var imgId = img?.id;
            var id = Convert.ToDecimal(imgId);
            token.ThrowIfCancellationRequested();

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var ImageExists = db.ImageInfos.Where(t => t.PhotoId == id).FirstOrDefault();
            token.ThrowIfCancellationRequested();

            if (ImageExists != null)
                return true;

            var imgData = !string.IsNullOrWhiteSpace(url_m)
                       ? await SaveImageFromUrlAsync(url_m, token)
                       : null;

            var imgObj = new ImageInfo
            {
                PhotoId = long.TryParse(imgId, out var parsedId) ? parsedId : (long?)null,
                Imagedata = imgData,
                Title = img?.title,
                Description = img?.description,
                Location = "",
                Region = "",
                Owner = img?.owner,
                Ownerrealname = img?.OwnerRealName,
                Filename = img?.filename
            };
            db.ImageInfos.Add(imgObj);
            await db.SaveChangesAsync();

            return true;
        }


        public virtual async Task<Byte[]?> SaveImageFromUrlAsync(string imageUrl, CancellationToken token)
        {
            var options = new RestClientOptions();
            var client = new RestClient(options);
            token.ThrowIfCancellationRequested();
            byte[]? imageBytes = null;

            try
            {
                var request = new RestRequest(imageUrl, Method.Get);
                var response = await client.ExecuteAsync(request);
                token.ThrowIfCancellationRequested();

                if (!response.IsSuccessful || response.RawBytes == null)
                    throw new Exception("Failed to download image from URL.");

                imageBytes = response.RawBytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while processing save photo url: {imageUrl}. Error: {ex.Message}");
                throw;
            }
            return imageBytes;
        }

        #region Helpers
        private async Task<List<ImageInfo>> LoadImages(CancellationToken token) // persist data
        {
            var result = new List<ImageInfo>();
            try
            {
                token.ThrowIfCancellationRequested();
                result = await _context.ImageInfos.Where(d => d.Deleted == 0).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public ImageDTO? ConvertToBase64(ImageEntity entity)
        {
            if (entity == null || entity.ImageData == null || entity.ImageData.Length == 0)
                return null;


            return new ImageDTO
            {
                FileName = entity.FileName,
                Base64Image = $"data:image/png;base64,{Convert.ToBase64String(entity.ImageData)}"
            };
        }

        public virtual async Task<List<FlickrPhotoSearchDTO>?> PhotoSearchByListAsync(List<string?>? places, CancellationToken token)
        {
            var result = new List<FlickrPhotoSearchDTO>();
            try
            {
                if (places == null || !places.Any()) return result;

                foreach (var place in places)
                {
                    if (string.IsNullOrWhiteSpace(place)) continue;

                    token.ThrowIfCancellationRequested();
                    var encValue = Uri.EscapeDataString(place);
                    var photoSearchResult = await this.PhotoSearch(encValue, token);

                    if (photoSearchResult != null)
                    {
                        result.Add(photoSearchResult);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while processing photo search list. Error: {ex.Message}");
                throw;
            }

            return result;
        }

        #endregion
    }
}


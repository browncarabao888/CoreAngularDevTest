using CoreAngularDevTest.Server.Interfaces;
using CoreAngularDevTest.Server.Models;
using CoreAngularDevTest.Server.Models.DTO.FlickPhotoSearch;
using CoreAngularDevTest.Server.Models.DTO.Flickr;
using CoreAngularDevTest.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo = CoreAngularDevTest.Server.Models.DTO.Flickr.Photo;
using PhotoSearch = CoreAngularDevTest.Server.Models.DTO.FlickPhotoSearch.Photo;

namespace DevTestUnitTest
{
    public class FlickrServiceTestUnit
    {


        [Fact]
        public async Task GetImageInfoAsync_WithValidPhotoIds_ReturnsExpectedResults()
        {

            var context = new Mock<ApplicationDbContext>();
            var scopeFactory = new Mock<IServiceScopeFactory>();
            var flickrService = new Mock<FlickrService>(context.Object, scopeFactory.Object)
            {
                CallBase = true
            };

            var cancellationToken = CancellationToken.None;
            var photoIdList = new List<string?> { "54505653038", "54408955429", "54445584376" };


            var fakeResponse = photoIdList.Select(id =>
                               new FlickrImageInfoDTO
                               {
                                   stat = "ok",
                                   photo = new Photo
                                   {
                                       id = id!,
                                       title = new Title { _content = $"Title for {id}" }
                                   }
                               }).ToList();

            flickrService
                    .Setup(f => f.GetImageInfoAsync(photoIdList, cancellationToken))
                    .ReturnsAsync(fakeResponse);

            var result = await flickrService.Object.GetImageInfoAsync(photoIdList, cancellationToken);


            Assert.NotNull(result);
            Assert.Equal(photoIdList.Count, result?.Count);
            Assert.All(result!, r => Assert.Equal("ok", r.stat));
            Assert.All(result!, r => Assert.False(string.IsNullOrEmpty(r.photo?.id)));
        }


        [Fact]
        public async Task SaveImageFromUrlAsync_WithValidImages_ReturnsExpectedResults()
        {
            var context = new Mock<ApplicationDbContext>();
            var scopeFactory = new Mock<IServiceScopeFactory>();
            var flickrService = new FlickrService(context.Object, scopeFactory.Object);

            var cancellationToken = CancellationToken.None;
            var photoList = new List<string?>();
            List<byte[]> result = new();

            photoList.Add("https://picsum.photos/800/600");
            photoList.Add("https://picsum.photos/800/800");
            photoList.Add("https://picsum.photos/800/360");

            foreach (var pix in photoList)
            {
                if (pix == null) continue;
                result.Add(await flickrService.SaveImageFromUrlAsync(pix, cancellationToken));
            }

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }


        [Fact]
        public async Task PhotoSearchByListAsync_WithValidPlaces_ReturnsExpectedResults()
        {

            var ctx = new Mock<ApplicationDbContext>();
            var factory = new Mock<IServiceScopeFactory>();

            var mockService = new Mock<FlickrService>(ctx.Object, factory.Object) { CallBase = true };

            var cancellationToken = CancellationToken.None;

            var placeList = new List<string> { "Manila", "Cebu", "Durban" };

            foreach (var place in placeList)
            {
                var encoded = Uri.EscapeDataString(place);

                mockService.Setup(s => s.PhotoSearch(encoded, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new FlickrPhotoSearchDTO
                    {
                        PlaceName = place,
                        stat = "ok",
                        photos = new Photos
                        {
                            page = 1,
                            perpage = 10,
                            total = 1,
                            photo = new List<PhotoSearch> {
                        new PhotoSearch {
                            id = Guid.NewGuid().ToString(),
                            title = $"Test Photo of {place}"
                        }
                            }
                        }
                    });
            }


            var result = await mockService.Object.PhotoSearchByListAsync(placeList, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.PlaceName)));
            Assert.Contains(result, r => r.PlaceName == "Manila");
            Assert.Contains(result, r => r.PlaceName == "Cebu");
            Assert.Contains(result, r => r.PlaceName == "Durban");
        }

    }

}


using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using AutoMapper;
using Newtonsoft.Json;
using Tournament.Common.Models;
using Tournament.DAL;
using Tournament.API.Mappers;

namespace Tournament.API.Tests
{
    public class PlaceApiTests : IAsyncDisposable
    {
        private readonly TournamentApiApplicationFactory _application;
        private readonly Lazy<HttpClient> _client;
        private readonly Mapper _mapper;

        public PlaceApiTests()
        {
            _application = new TournamentApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task GetPlace()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Place");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var places = await response.Content.ReadFromJsonAsync<ICollection<PlaceListModel>>();
            Assert.NotNull(places);
            Assert.NotEmpty(places);
        }

        [Fact]
        public async Task GetAllPlaces_Test()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Place");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var places = await response.Content.ReadFromJsonAsync<ICollection<PlaceListModel>>();
            Assert.NotNull(places);
            Assert.NotEmpty(places);
        }

        [Fact]
        public async Task GetPlaceById_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Place/{DbSeed.Place1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var place = await response.Content.ReadFromJsonAsync<PlaceDetailModel>();

            Assert.Equal(_mapper.Map<PlaceDetailModel>(DbSeed.Place1), place);
        }

        [Fact]
        public async Task GetPlaceById_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Place/52fb5d63-b308-4bc2-bb03-0e5ca54ac4e3");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetPlaceById_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Place/abc123");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreatePlace_SuccessTest()
        {
            // Arrange
            var newPlace = new PlaceDetailModel()
            {
                Name = "Place01",
                Description = "This is a new place"
            };
            var placeJson = JsonConvert.SerializeObject(newPlace);
            var data = new StringContent(placeJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Place", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreatePlace_BadContentTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Place", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePlace_SuccessTest()
        {
            // Arrange
            var oldPlace = _mapper.Map<PlaceDetailModel>(DbSeed.Place2);
            var newPlace = new PlaceDetailModel()
            {
                Id = oldPlace.Id,
                Name = "NewBoulevard",
                Description = oldPlace.Description
            };
            var placeJson = JsonConvert.SerializeObject(newPlace);
            var data = new StringContent(placeJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Place", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newPlace.Id, updatedId);
        }

        [Fact]
        public async Task UpdatePlace_BadIdTest()
        {
            // Arrange
            var newPlace = new PlaceDetailModel()
            {
                Id = Guid.Parse("8dcfdc18-1f92-46ae-bfbc-80e2d58006aa"),
                Name = "New name",
                Description = "New description"
            };
            var placeJson = JsonConvert.SerializeObject(newPlace);
            var data = new StringContent(placeJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Place", data);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePlace_BadContentsTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Place", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeletePlace_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Place/{DbSeed.Place1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeletePlace_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync("/api/Place/996247a6-3283-48c0-971a-3083fd383e93");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SearchPlace_SuccessTest1()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Place/Search?search=Ave");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var places = await response.Content.ReadFromJsonAsync<ICollection<PlaceListModel>>();
            Assert.NotNull(places);
            Assert.NotEmpty(places);
        }

        [Fact]
        public async Task SearchPlace_SuccessTest2()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Place/Search?search=place");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var places = await response.Content.ReadFromJsonAsync<ICollection<PlaceListModel>>();
            Assert.NotNull(places);
            Assert.NotEmpty(places);
        }


        [Fact]
        public async Task SearchPlace_BadRequestTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Place/Search");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SearchPlace_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Place/Search?search=spirit");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var places = await response.Content.ReadFromJsonAsync<ICollection<PlaceListModel>>();
            Assert.NotNull(places);
            Assert.Empty(places);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}

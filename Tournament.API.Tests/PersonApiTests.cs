using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
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
    public class PersonApiTests : IAsyncDisposable
    {
        private readonly TournamentApiApplicationFactory _application;
        private readonly Lazy<HttpClient> _client;
        private readonly Mapper _mapper;

        public PersonApiTests()
        {
            _application = new TournamentApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task GetAllPersons_Test()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Person");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var persons = await response.Content.ReadFromJsonAsync<ICollection<PersonListModel>>();
            Assert.NotNull(persons);
            Assert.NotEmpty(persons);
        }

        [Fact]
        public async Task GetPersonById_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Person/{DbSeed.Person1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var person = await response.Content.ReadFromJsonAsync<PersonDetailModel>();
            Assert.NotNull(person);
        }

        [Fact]
        public async Task GetPersonById_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Person/11f3a641-0404-40cf-83b5-80e293062eb1");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetPersonById_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Person/abc123");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreatePerson_SuccessTest()
        {
            // Arrange
            var newPerson = new PersonDetailModel()
            {
                FirstName = "New",
                LastName = "Person",
                Description = "This is a new person"
            };
            var personJson = JsonConvert.SerializeObject(newPerson);
            var data = new StringContent(personJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Person", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreatePerson_BadContentTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Person", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePerson_SuccessTest()
        {
            // Arrange
            var oldPerson = _mapper.Map<PersonDetailModel>(DbSeed.Person1);
            var newPerson = new PersonDetailModel()
            {
                Id = oldPerson.Id,
                FirstName = "New",
                LastName = "Name",
                PhotoURL = oldPerson.PhotoURL,
                Description = oldPerson.Description,
                TeamId = oldPerson.TeamId,
                TeamName = oldPerson.TeamName,
                TeamLogoURL = oldPerson.TeamLogoURL
            };
            var personJson = JsonConvert.SerializeObject(newPerson);
            var data = new StringContent(personJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Person", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newPerson.Id, updatedId);
        }

        [Fact]
        public async Task UpdatePerson_BadIdTest()
        {
            // Arrange
            var newPerson = new PersonDetailModel()
            {
                Id = Guid.Parse("8dcfdc18-1f92-46ae-bfbc-80e2d58006aa"),
                FirstName = "New",
                LastName = "Name"
            };
            var personJson = JsonConvert.SerializeObject(newPerson);
            var data = new StringContent(personJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Person", data);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePerson_BadContentsTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Person", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeletePerson_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Person/{DbSeed.Person2.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeletePerson_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Person/{DbSeed.Place1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SearchPerson_SuccessTest1()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Person/Search?search=John");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var persons = await response.Content.ReadFromJsonAsync<ICollection<PersonListModel>>();
            Assert.NotNull(persons);
            Assert.NotEmpty(persons);
        }

        [Fact]
        public async Task SearchPerson_SuccessTest2()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Person/Search?search=named");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var persons = await response.Content.ReadFromJsonAsync<ICollection<PersonListModel>>();
            Assert.NotNull(persons);
            Assert.NotEmpty(persons);
        }

        [Fact]
        public async Task SearchPerson_BadRequestTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Person/Search");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SearchPerson_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Person/Search?search=Dave");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var persons = await response.Content.ReadFromJsonAsync<ICollection<PersonListModel>>();
            Assert.NotNull(persons);
            Assert.Empty(persons);
        }


        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}

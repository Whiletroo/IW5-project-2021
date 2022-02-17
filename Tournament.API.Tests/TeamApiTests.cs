using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Netizine.Enums;
using Newtonsoft.Json;
using Tournament.API.Mappers;
using Tournament.Common.Models;
using Tournament.DAL;
using Tournament.DAL.Enums;
using Xunit;

namespace Tournament.API.Tests
{
    public class TeamApiTests :IAsyncDisposable
    {
        private readonly TournamentApiApplicationFactory _application;
        private readonly Lazy<HttpClient> _client;
        private readonly Mapper _mapper;

        public TeamApiTests()
        {
            _application = new TournamentApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task GetAllTeams_Test()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Team");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var teams = await response.Content.ReadFromJsonAsync<ICollection<TeamListModel>>();
            Assert.NotNull(teams);
            Assert.NotEmpty(teams);
        }

        [Fact]
        public async Task GetTeamById_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Team/{DbSeed.Team1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var team = await response.Content.ReadFromJsonAsync<TeamDetailModel>();
            Assert.NotNull(team);
        }

        [Fact]
        public async Task GetTeamById_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Team/df2e39d9-f691-4f9b-8533-61c2474c23f7");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetTeamById_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Person/abc123");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateTeam_SuccessTest()
        {
            // Arrange
            var newTeam = new TeamDetailModel()
            {
                TeamName = "Team Spirit",
                Description = "This is a new team",
                RegistrationCountry = Country.RussianFederation
            };
            var teamJson = JsonConvert.SerializeObject(newTeam);
            var data = new StringContent(teamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Team", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreateTeam_BadContentTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Team", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTeam_SuccessTest()
        {
            // Arrange
            var oldTeam = _mapper.Map<TeamDetailModel>(DbSeed.Team1);
            var newTeam = new TeamDetailModel()
            {
                Id = oldTeam.Id,
                TeamName = "PSG.LGD",
                Description = oldTeam.Description,
                RegistrationCountry = Country.China,
                LogoURL = oldTeam.LogoURL
            };
            var teamJson = JsonConvert.SerializeObject(newTeam);
            var data = new StringContent(teamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Team", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newTeam.Id, updatedId);
        }

        [Fact]
        public async Task UpdateTeam_BadIdTest()
        {
            // Arrange
            var newTeam = new TeamDetailModel()
            {
                Id = Guid.Parse("8dcfdc18-1f92-46ae-bfbc-80e2d58006aa"),
                TeamName = "New Name",
                Description = "New description"
            };
            var teamJson = JsonConvert.SerializeObject(newTeam);
            var data = new StringContent(teamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Team", data);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTeam_BadContentsTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Team", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTeam_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Team/{DbSeed.Team2.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTeam_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Team/{DbSeed.Person1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SearchTeam_SuccessTest1()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Team/Search?search=Woman");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var teams = await response.Content.ReadFromJsonAsync<ICollection<TeamListModel>>();
            Assert.NotNull(teams);
            Assert.NotEmpty(teams);
        }

        [Fact]
        public async Task SearchTeam_SuccessTest2()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Team/Search?search=This");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var teams = await response.Content.ReadFromJsonAsync<ICollection<TeamListModel>>();
            Assert.NotNull(teams);
            Assert.NotEmpty(teams);
        }


        [Fact]
        public async Task SearchTeam_BadRequestTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Team/Search");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SearchTeam_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Team/Search?search=spirit");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var teams = await response.Content.ReadFromJsonAsync<ICollection<TeamListModel>>();
            Assert.NotNull(teams);
            Assert.Empty(teams);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}

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
using Tournament.DAL.Enums;
using Tournament.Common.Models;
using Tournament.DAL;
using Tournament.API.Mappers;

namespace Tournament.API.Tests
{
    public class MatchApiTests : IAsyncDisposable
    {
        private readonly TournamentApiApplicationFactory _application;
        private readonly Lazy<HttpClient> _client;
        private readonly Mapper _mapper;

        public MatchApiTests()
        {
            _application = new TournamentApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task GetAllMatches_Test()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Match");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var matches = await response.Content.ReadFromJsonAsync<ICollection<MatchListModel>>();
            Assert.NotNull(matches);
            Assert.NotEmpty(matches);
        }

        [Fact]
        public async Task GetMatchById_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Match/{DbSeed.Match1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var match = await response.Content.ReadFromJsonAsync<MatchDetailModel>();
            Assert.NotNull(match);
        }

        [Fact]
        public async Task GetMatchById_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Match/f53a3865-8344-4a89-b57a-c58149bcf150");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetMatchById_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync("/api/Mathc/fdakfhad784234327");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateMatch_SuccessTest()
        {
            // Arrange
            var newMatch = new MatchDetailModel()
            {
                Team1Id = DbSeed.Team1.Id,
                Team1Name = DbSeed.Team1.TeamName,
                Team1LogoURL = DbSeed.Team1.LogoURL,
                Team2Id = DbSeed.Team2.Id,
                Team2Name = DbSeed.Team2.TeamName,
                Team2LogoURL = DbSeed.Team2.LogoURL,
                PlaceName = DbSeed.Place1.Name,
                PlaceId = DbSeed.Place1.Id,
                DateTime = new DateTime(2021, 10, 18, 17, 56, 20, 501, DateTimeKind.Local).AddTicks(2002),
                Result = Results.Team2
            };
            var matchJson = JsonConvert.SerializeObject(newMatch);
            var data = new StringContent(matchJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Match", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreateMatch_BadContentTest()
        {
            // Arrange
            var newData = new { somedata = "dsa21312d", nextvalue = 12.023991};
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Match", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateMatch_SuccessTest()
        {
            // Arrange
            var oldMatch = _mapper.Map<MatchDetailModel>(DbSeed.Match2);
            var newMatch = new MatchDetailModel()
            {
                Id = oldMatch.Id,
                Team1Id = oldMatch.Team1Id,
                Team1Name = oldMatch.Team1Name,
                Team1LogoURL = oldMatch.Team1LogoURL,
                Team2Id = oldMatch.Team2Id,
                Team2Name = oldMatch.Team2Name,
                Team2LogoURL = oldMatch.Team2LogoURL,
                PlaceName = oldMatch.PlaceName,
                PlaceId = oldMatch.PlaceId,
                DateTime = oldMatch.DateTime,
                Result = Results.Team2                  // only result changed
            };
            var matchJson = JsonConvert.SerializeObject(newMatch);
            var data = new StringContent(matchJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Match", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newMatch.Id, updatedId);
        }

        [Fact]
        public async Task UpdateMatch_BadIdTest()
        {
            // Arrange
            var newMatch = new MatchDetailModel()
            {
                Id = Guid.Parse("a837d6db-e7b3-4e9b-a533-d4fed5a335c8"),
                PlaceName = DbSeed.Place1.Name,
                PlaceId = DbSeed.Place1.Id,
                DateTime = DbSeed.Match2.DateTime,
                Result = Results.None
            };
            var matchJson = JsonConvert.SerializeObject(newMatch);
            var data = new StringContent(matchJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Match", data);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateMatch_BadContentsTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Match", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteMatch_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Match/{DbSeed.Match2.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteMatch_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Team/9d65d042-5ba0-4ba1-acf3-ca0ed99084a6");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}

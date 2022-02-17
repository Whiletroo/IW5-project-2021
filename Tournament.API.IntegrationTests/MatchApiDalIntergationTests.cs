using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Xunit;
using Tournament.DAL;
using Tournament.DAL.Repositories;
using Tournament.Common.Models;
using Tournament.API.Mappers;
using Tournament.API.Controllers;

namespace Tournament.API.IntegrationTests
{
    public class MatchApiDalIntergationTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly MatchRepository _repository;
        private readonly Mapper _mapper;
        private readonly MatchController _controllerSUT;

        public MatchApiDalIntergationTests()
        {
            // Create db
            _dbContextFactory = new DbContextInMemoryFactory(nameof(MatchApiDalIntergationTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();

            _repository = new MatchRepository(_dbContextFactory);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);

            _controllerSUT = new MatchController(_mapper, _repository);
        }

        [Fact]
        public void GetAllMatchs_Test()
        {
            // Arrange
            IEnumerable matchList = new List<MatchListModel>()
            {
                _mapper.Map<MatchListModel>(DbSeed.Match1),
                _mapper.Map<MatchListModel>(DbSeed.Match2)
            };

            // Act
            var result = (OkObjectResult)_controllerSUT.Get().Result;

            // Assert
            Assert.Equal(matchList, result.Value);
        }

        [Fact]
        public void GetMatch_Test()
        {
            // Arrange
            var match = _mapper.Map<MatchDetailModel>(DbSeed.Match1);

            // Act
            var result = (OkObjectResult)_controllerSUT.Get(match.Id).Result;

            // Assert
            Assert.Equal(match, result.Value);
        }

        [Fact]
        public void CreateMatch_Test()
        {
            var placeGuid = Guid.NewGuid();
            // Arrange
            var match = new MatchDetailModel()
            {
                Team1Id = DbSeed.Match1.Team1Id,
                Team1LogoURL = DbSeed.Match1.Team1.LogoURL,
                Team1Name = DbSeed.Match1.Team1.TeamName,
                Team2Id = DbSeed.Match1.Team2Id,
                Team2LogoURL = DbSeed.Match1.Team2.LogoURL,
                Team2Name = DbSeed.Match1.Team2.TeamName,
                PlaceId = DbSeed.Place1.Id,
                PlaceName = DbSeed.Place1.Name,
                DateTime = DbSeed.Match1.DateTime,
                Result = DAL.Enums.Results.Team2
            };

            // Act
            var result = _controllerSUT.Create(match).Result as CreatedResult;

            // Assert
            var referenceMatch = new MatchDetailModel()
            {
                Id = (Guid)result.Value,
                Team1Id = match.Team1Id,
                Team1Name = match.Team1Name,
                Team1LogoURL = match.Team1LogoURL,
                Team2Id = match.Team2Id,
                Team2Name = match.Team2Name,
                Team2LogoURL = match.Team2LogoURL,
                PlaceId = match.PlaceId,
                PlaceName = match.PlaceName,
                DateTime = match.DateTime,
                Result = match.Result

            };
            var createdMatch = _mapper.Map<MatchDetailModel>(_repository.Get((Guid)result.Value));
            Assert.Equal(referenceMatch, createdMatch);
        }

        [Fact]
        public void UpdateMatch_Test()
        {
            // Arrange
            var match = new MatchDetailModel()
            {
                Id = DbSeed.Match1.Id,
                Team1Id = DbSeed.Match1.Team1Id,
                Team1Name = DbSeed.Match1.Team1.TeamName,
                Team1LogoURL = DbSeed.Match1.Team1.LogoURL,
                Team2Id = DbSeed.Match1.Team2Id,
                Team2Name = DbSeed.Match1.Team2.TeamName,
                Team2LogoURL = DbSeed.Match1.Team2.LogoURL,
                PlaceId = DbSeed.Match1.PlaceId,
                PlaceName = DbSeed.Match1.Place.Name,
                DateTime = DbSeed.Match1.DateTime,
                Result = DAL.Enums.Results.Team2,
            };

            // Act
            var result = _controllerSUT.Update(match).Result as CreatedResult;

            // Assert
            var updatedMatch = _mapper.Map<MatchDetailModel>(_repository.Get((Guid)result.Value));
            Assert.Equal(match, updatedMatch);
        }

        [Fact]
        public void DeleteMatch_Test()
        {
            // Arrange

            // Act
            var result = _controllerSUT.Delete(DbSeed.Match2.Id);

            // Assert
            var deletedMatch = _repository.Get(DbSeed.Match2.Id);
            Assert.Null(deletedMatch);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}

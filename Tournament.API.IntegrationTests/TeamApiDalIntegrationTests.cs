using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Netizine.Enums;
using Tournament.API.Controllers;
using Tournament.API.Mappers;
using Tournament.Common.Models;
using Tournament.DAL;
using Tournament.DAL.Enums;
using Tournament.DAL.Repositories;
using Xunit;

namespace Tournament.API.IntegrationTests
{
    public class TeamApiDalIntegrationTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly TeamRepository _repository;
        private readonly Mapper _mapper;
        private readonly TeamController _controllerSUT;

        public TeamApiDalIntegrationTests()
        {
            // Create db
            _dbContextFactory = new DbContextInMemoryFactory(nameof(TeamApiDalIntegrationTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();

            _repository = new TeamRepository(_dbContextFactory);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);

            _controllerSUT = new TeamController(_mapper, _repository);
        }

        [Fact]
        public void GetAllTeams_Test()
        {
            // Arrange
            IEnumerable teamsList = new List<TeamListModel>() { _mapper.Map<TeamListModel>(DbSeed.Team1), _mapper.Map<TeamListModel>(DbSeed.Team2) };

            // Act
            var result = _controllerSUT.Get();

            // Assert
            Assert.Equal(teamsList, result.Value);
        }

        [Fact]
        public void GetTeam_Test()
        {
            // Arrange
            var team = _mapper.Map<TeamDetailModel>(DbSeed.Team1);

            // Act
            var result = _controllerSUT.Get(team.Id);

            // Assert
            Assert.Equal(team, result.Value);
        }

        [Fact]
        public void CreateTeam_Test()
        {
            // Arrange
            var team = new TeamDetailModel()
            {
                TeamName = "New team",
                Description = "This is new team",
                LogoURL = "someURL",
                RegistrationCountry = Country.Congo,
                Persons = new List<PersonListModel>() {}
            };

            // Act
            var result = _controllerSUT.Create(team).Result as CreatedResult;

            // Assert
            var referenceTeam = new TeamDetailModel()
            {
                Id = (Guid)result.Value,
                TeamName = team.TeamName,
                Description = team.Description,
                LogoURL = team.LogoURL,
                RegistrationCountry = team.RegistrationCountry,
                Persons = team.Persons
            };
            var createdTeam = _mapper.Map<TeamDetailModel>(_repository.Get((Guid)result.Value));
            Assert.Equal(referenceTeam, createdTeam);
        }

        [Fact]
        public void UpdateTeam_Test()
        {
            // Arrange
            var team = new TeamDetailModel()
            {
                Id = DbSeed.Team1.Id,
                TeamName = DbSeed.Team1.TeamName,
                Description = DbSeed.Team1.Description,
                LogoURL = DbSeed.Team1.LogoURL,
                Persons = new List<PersonListModel>() {_mapper.Map<PersonListModel>(DbSeed.Person1)}
            };

            // Act
            var result = _controllerSUT.Update(team).Result as CreatedResult;

            // Assert
            var updatedTeam = _mapper.Map<TeamDetailModel>(_repository.Get((Guid)result.Value));
            Assert.Equal(team, updatedTeam);
        }

        [Fact]
        public void DeleteTeam_Test()
        {
            // Arrange

            // Act
            var result = _controllerSUT.Delete(DbSeed.Team2.Id);

            // Assert
            var deletedTeam = _repository.Get(DbSeed.Team2.Id);
            Assert.Null(deletedTeam);
        }

        [Fact]
        public void SearchTeam_Test1()
        {
            // Arrange
            IEnumerable teamList = new List<TeamListModel>()
            {
                _mapper.Map<TeamListModel>(DbSeed.Team2)
            };

            // Act
            var result = _controllerSUT.Search("Woman");

            // Assert
            Assert.Equal(teamList, result.Value);
        }

        [Fact]
        public void SearchTeam_Test2()
        {
            // Arrange
            IEnumerable teamList = new List<TeamListModel>()
            {
                _mapper.Map<TeamListModel>(DbSeed.Team1),
                _mapper.Map<TeamListModel>(DbSeed.Team2)
            };

            // Act
            var result = _controllerSUT.Search("This");

            // Assert
            Assert.Equal(teamList, result.Value);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}

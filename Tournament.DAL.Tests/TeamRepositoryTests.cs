using System;
using System.Linq;
using Tournament.DAL.Enums;
using Tournament.DAL.Entities;
using Tournament.DAL.Repositories;
using Xunit;
using System.Collections.Generic;
using Netizine.Enums;

namespace Tournament.DAL.Tests
{
    public class TeamRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly TeamRepository _dbRepositorySUT;

        public TeamRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(TeamRepositoryTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();

            _dbRepositorySUT = new TeamRepository(_dbContextFactory);
        }

        [Fact]
        public void InsertNewTeamTest()
        {
            //Arrange
            var newTeam = new TeamEntity()
            {
                Id = Guid.Parse("e81e38cf-3970-4215-bb16-564c1509715e"),
                Description = "some description",
                LogoURL = "some URL",
                RegistrationCountry = Country.Argentina,
                TeamName = "some name"
            };

            //Act
            _dbRepositorySUT.Insert(newTeam);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedTeam = dbx.Teams.Single(entity => entity.Id == newTeam.Id);
            Assert.Equal(newTeam, retrievedTeam);
        }

        [Fact]
        public void GetTeamTest()
        {
            //Arrange
            var existingTeam = DbSeed.Team1;

            //Act
            TeamEntity retrievedTeam = _dbRepositorySUT.Get(existingTeam.Id);

            //Assert
            Assert.Equal(existingTeam, retrievedTeam);
        }

        [Fact]
        public void GetAllTeamsTest()
        {
            //Arrange
            var existingTeams = new List<TeamEntity>() { DbSeed.Team1, DbSeed.Team2 };

            //Act
            IEnumerable<TeamEntity> retrievedTeams = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(existingTeams, retrievedTeams);
        }

        [Fact]
        public void UpdateTeamTest()
        {
            //Arrange
            var existingTeam = DbSeed.Team1;

            //Act
            var updatedTeam = new TeamEntity()
            {
                Id = existingTeam.Id,
                Description = "another description",
                LogoURL = "another URL",
                RegistrationCountry = existingTeam.RegistrationCountry,
                TeamName = existingTeam.TeamName
            };

            _dbRepositorySUT.Update(updatedTeam);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedTeam = dbx.Teams.Single(team => team.Id == existingTeam.Id);
            Assert.Equal(updatedTeam.Id, retrievedTeam.Id);
            Assert.Equal(updatedTeam.Description, retrievedTeam.Description);
            Assert.NotEqual(existingTeam.Description, retrievedTeam.Description);
            Assert.Equal(updatedTeam.LogoURL, retrievedTeam.LogoURL);
            Assert.NotEqual(existingTeam.LogoURL, retrievedTeam.LogoURL);
            Assert.Equal(updatedTeam.RegistrationCountry, retrievedTeam.RegistrationCountry);
            Assert.Equal(updatedTeam.TeamName, retrievedTeam.TeamName);
        }

        [Fact]
        public void DeleteTeamTest()
        {
            //Arrange
            var existingTeam = DbSeed.Team1;

            //Act
            _dbRepositorySUT.Delete(existingTeam.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            TeamEntity retrievedTeam = dbx.Teams.SingleOrDefault(team => team.Id == existingTeam.Id);
            Assert.Null(retrievedTeam);
        }

        [Fact]
        public void DeleteTeamAndReferenceToPersonTest()
        {
            //Arrange
            var existingTeam = DbSeed.Team1;
            var existingPerson = DbSeed.Person1;

            //Act
            _dbRepositorySUT.Delete(existingTeam.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            PersonEntity retrievedPerson = dbx.Persons.SingleOrDefault(person => person.Id == existingPerson.Id);
            Assert.Null(retrievedPerson.Team);
        }

        [Fact]
        public void DeleteTeamAndReferenceToMatchTest()
        {
            //Arrange
            var existingMatch = DbSeed.Match1;

            //Act
            _dbRepositorySUT.Delete(existingMatch.Team1.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            MatchEntity retrievedMatch = dbx.Matches.SingleOrDefault(match => match.Id == existingMatch.Id);
            Assert.Null(retrievedMatch.Team1);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}

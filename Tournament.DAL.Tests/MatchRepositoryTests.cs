using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Tournament.DAL.Entities;
using Tournament.DAL.Enums;
using Tournament.DAL.Repositories;

namespace Tournament.DAL.Tests
{
    public class MatchRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly MatchRepository _dbRepositorySUT;

        public MatchRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(MatchRepositoryTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();

            _dbRepositorySUT = new MatchRepository(_dbContextFactory);
        }

        [Fact]
        public void GetMatchTest()
        {
            //Arrange
            var existingMatch = DbSeed.Match1;

            //Act
            MatchEntity retrievedMatch = _dbRepositorySUT.Get(existingMatch.Id);

            //Assert
            Assert.Equal(existingMatch, retrievedMatch);
        }

        [Fact]
        public void GetAllMatchesTest()
        {
            //Arrange
            var existingMatchesList = new List<MatchEntity>() { DbSeed.Match1, DbSeed.Match2 };

            //Act
            IEnumerable<MatchEntity> retrievedMatches = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(retrievedMatches, existingMatchesList);
        }

        [Fact]
        public void InsertNewMatchTest()
        {
            //Arrange
            var newMatch = new MatchEntity()
            {
                Id = Guid.Parse("601b43eb-84b7-43af-9b60-9f70f2699789"),
                Team1Id = DbSeed.Team1.Id,
                Team2Id = DbSeed.Team2.Id,
                PlaceId = DbSeed.Place1.Id,
                DateTime = new DateTime(2021, 1, 1, 1, 0, 0, 0),
                Result = Results.None
            };

            //Act
            _dbRepositorySUT.Insert(newMatch);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedMatch = dbx.Matches.Single(entity => entity.Id == newMatch.Id);
            Assert.Equal(newMatch, retrievedMatch);
        }

        [Fact]
        public void UpdateMatchTest()
        {
            //Arrange
            var existingMatch = DbSeed.Match1;
            var updatedMatch = new MatchEntity()
            {
                Id = existingMatch.Id,
                Team1Id = existingMatch.Team1.Id,
                Team2Id = existingMatch.Team2.Id,
                PlaceId = existingMatch.Place.Id,
                DateTime = new DateTime(2021, 1, 2, 1, 0, 0, 0), // Only DateTime is changed
                Result = existingMatch.Result
            };

            //Act
            _dbRepositorySUT.Update(updatedMatch);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedMatch = dbx.Matches.Single(match => match.Id == existingMatch.Id);
            Assert.Equal(updatedMatch.Id, retrievedMatch.Id);
            Assert.Equal(updatedMatch.Team1, retrievedMatch.Team1);
            Assert.Equal(updatedMatch.Team2, retrievedMatch.Team2);
            Assert.Equal(updatedMatch.Place, retrievedMatch.Place);
            Assert.Equal(updatedMatch.DateTime, retrievedMatch.DateTime);
            Assert.NotEqual(existingMatch.DateTime, retrievedMatch.DateTime);
            Assert.Equal(updatedMatch.Result, retrievedMatch.Result);

        }

        [Fact]
        public void DeleteMatchTest()
        {
            //Arrange
            var existingMatch = DbSeed.Match1;

            //Act
            _dbRepositorySUT.Delete(existingMatch.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            MatchEntity retrievedMatch = dbx.Matches.SingleOrDefault(match => match.Id == existingMatch.Id);
            Assert.Null(retrievedMatch);
        }

        [Fact]
        public void DeleteMatchFromPlaceTest()
        {
            //Arrange
            var existingPlace = DbSeed.Place1;
            var existingMatch = DbSeed.Match1;

            //Act
            _dbRepositorySUT.Delete(existingMatch.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            PlaceEntity retrievedPlace = dbx.Places.SingleOrDefault(place => place.Id == existingPlace.Id);
            Assert.Empty(retrievedPlace.Matches);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}
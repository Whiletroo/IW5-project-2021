using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.DAL.Entities;
using Tournament.DAL.Repositories;
using Xunit;

namespace Tournament.DAL.Tests
{
    public class PlaceRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly PlaceRepository _dbRepositorySUT;

        public PlaceRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(PlaceRepositoryTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();

            _dbRepositorySUT = new PlaceRepository(_dbContextFactory);
        }

        [Fact]
        public void GetPlaceTest()
        {
            //Arrange
            var existingPlace = DbSeed.Place1;

            //Act
            PlaceEntity retrievedModel = _dbRepositorySUT.Get(existingPlace.Id);

            //Assert
            Assert.Equal(existingPlace, retrievedModel);
        }

        [Fact]
        public void GetAllPlacesTest()
        {
            //Arrange
            var existingPlacesList = new List<PlaceEntity> { DbSeed.Place1, DbSeed.Place2 };

            //Act
            IEnumerable<PlaceEntity> retrievedPlaces = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(retrievedPlaces, existingPlacesList);
        }

        [Fact]
        public void InsertPlaceTest()
        {
            //Arrange
            var newPlace = new PlaceEntity()
            {
                Id = Guid.Parse("fee54c5b-b812-4a7e-bbf4-665e380434c2"),
                Name = "someName1"
            };

            //Act
            _dbRepositorySUT.Insert(newPlace);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedPlace = dbx.Places.Single(entity => entity.Id == newPlace.Id);
            Assert.Equal(newPlace, retrievedPlace);
        }

        [Fact]
        public void UpdatePlaceTest()
        {
            //Arrange
            var existingPlace = DbSeed.Place1;
            var updatedPlace = new PlaceEntity()
            {
                Id = existingPlace.Id,
                Name = "newName"
            };

            //Act
            _dbRepositorySUT.Update(updatedPlace);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedPlace = dbx.Places.Single(place => place.Id == existingPlace.Id);
            Assert.Equal(existingPlace.Id, retrievedPlace.Id);
            Assert.NotEqual(existingPlace.Name, retrievedPlace.Name);
        }

        [Fact]
        public void DeletePlaceTest()
        {
            //Arrange
            var existingPlace = DbSeed.Place1;

            //Act
            _dbRepositorySUT.Delete(existingPlace.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            PlaceEntity retrievedPlace = dbx.Places.SingleOrDefault(place => place.Id == existingPlace.Id);
            Assert.Null(retrievedPlace);
        }

        [Fact]
        public void DeletePlaceAndMatchesTest()
        {
            //Arrange
            var existingMatch = DbSeed.Match1;
            var existingPlace = DbSeed.Place1;

            //Act
            _dbRepositorySUT.Delete(existingPlace.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            MatchEntity retrievedMatch = dbx.Matches.SingleOrDefault(match => match.Id == existingMatch.Id);
            Assert.Null(retrievedMatch);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}
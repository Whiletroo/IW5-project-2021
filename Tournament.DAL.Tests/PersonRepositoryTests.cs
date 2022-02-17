using System;
using System.Linq;
using Tournament.DAL.Entities;
using Tournament.DAL.Repositories;
using Xunit;

namespace Tournament.DAL.Tests
{
    public class PersonRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly PersonRepository _dbRepositorySUT;

        public PersonRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(PersonRepositoryTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();

            _dbRepositorySUT = new PersonRepository(_dbContextFactory);
        }

        [Fact]
        public void InsertNewPersonTest()
        {
            //Arrange
            var newPerson = new PersonEntity()
            {
                Id = Guid.Parse("3ff65e59-e2eb-4b07-b681-ca4f05422dc7"),
                Description = "some description",
                FirstName = "Name",
                LastName = "Surname",
                PhotoURL = "some URL",
                TeamId = DbSeed.Team1.Id
            };

            //Act
            _dbRepositorySUT.Insert(newPerson);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedPerson = dbx.Persons.Single(entity => entity.Id == newPerson.Id);
            Assert.Equal(newPerson, retrievedPerson);
        }

        [Fact]
        public void GetPersonTest()
        {
            //Arrange
            var existingPerson = DbSeed.Person1;

             //Act
             PersonEntity retrievedPerson = _dbRepositorySUT.Get(existingPerson.Id);

             //Assert
             Assert.Equal(existingPerson, retrievedPerson);
        }

        [Fact]
        public void UpdatePersonTest()
        {
            //Arrange
            var existingPerson = DbSeed.Person1;
            var existingTeam = DbSeed.Team1;
            var updatedPerson = new PersonEntity()
            {
                Id = existingPerson.Id,
                Description = "another description",
                FirstName = existingPerson.FirstName,
                LastName = existingPerson.FirstName,
                PhotoURL = "another URL",
                TeamId = existingTeam.Id
            };

            //Act
            _dbRepositorySUT.Update(updatedPerson);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedPerson = dbx.Persons.Single(person => person.Id == existingPerson.Id);
            Assert.Equal(updatedPerson.Id, retrievedPerson.Id);
            Assert.Equal(updatedPerson.Description, retrievedPerson.Description);
            Assert.NotEqual(existingPerson.Description, retrievedPerson.Description);
            Assert.Equal(updatedPerson.FirstName, retrievedPerson.FirstName);
            Assert.Equal(updatedPerson.LastName, retrievedPerson.LastName);
            Assert.Equal(updatedPerson.PhotoURL, retrievedPerson.PhotoURL);
            Assert.NotEqual(existingPerson.PhotoURL, retrievedPerson.PhotoURL);
            Assert.Equal(updatedPerson.TeamId, retrievedPerson.TeamId);
        }

        [Fact]
        public void DeletePersonTest()
        {
            //Arrange
            var existingPerson = DbSeed.Person1;

            //Act
            _dbRepositorySUT.Delete(existingPerson.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            PersonEntity retrievedPerson = dbx.Persons.SingleOrDefault(person => person.Id == existingPerson.Id);
            Assert.Null(retrievedPerson);
        }

        [Fact]
        public void DeletePersonFromTeamTest()
        {
            //Arrange
            var existingTeam = DbSeed.Team1;
            var existingPerson = DbSeed.Person1;

            //Act
            _dbRepositorySUT.Delete(existingPerson.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            TeamEntity retrievedTeam = dbx.Teams.SingleOrDefault(team => team.Id == existingTeam.Id);
            Assert.Equal(existingTeam.Id, retrievedTeam.Id);
            Assert.DoesNotContain(existingPerson, retrievedTeam.Persons);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }   
    }
}

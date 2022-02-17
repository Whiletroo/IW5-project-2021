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
    public class PersonApiDalIntegrationTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly PersonRepository _repository;
        private readonly Mapper _mapper;
        private readonly PersonController _controllerSUT;

        public PersonApiDalIntegrationTests()
        {
            // Create db
            _dbContextFactory = new DbContextInMemoryFactory(nameof(PersonApiDalIntegrationTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();

            _repository = new PersonRepository(_dbContextFactory);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);

            _controllerSUT = new PersonController(_mapper, _repository);
        }

        [Fact]
        public void GetAllPersons_Test()
        {
            // Arrange
            IEnumerable personList = new List<PersonListModel>() { _mapper.Map<PersonListModel>(DbSeed.Person1), _mapper.Map<PersonListModel>(DbSeed.Person2) };

            // Act
            var result = _controllerSUT.Get();

            // Assert
            Assert.Equal(personList, result.Value);
        }

        [Fact]
        public void GetPerson_Test()
        {
            // Arrange
            var person = _mapper.Map<PersonDetailModel>(DbSeed.Person1);

            // Act
            var result = _controllerSUT.Get(person.Id);

            // Assert
            Assert.Equal(person, result.Value);
        }

        [Fact]
        public void CreatePerson_Test()
        {
            // Arrange
            var person = new PersonDetailModel()
            {
                FirstName = "New",
                LastName = "Person",
                Description = "This is a new person",
                TeamId = DbSeed.Team1.Id,
                TeamName = DbSeed.Team1.TeamName,
                TeamLogoURL = DbSeed.Team1.LogoURL
            };

            // Act
            var result = _controllerSUT.Create(person).Result as CreatedResult;

            // Assert
            var referencePerson = new PersonDetailModel()
            {
                Id = (Guid)result.Value,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Description = person.Description,
                TeamId = person.TeamId,
                TeamName = person.TeamName,
                TeamLogoURL = person.TeamLogoURL
            };
            var createdPerson = _mapper.Map<PersonDetailModel>(_repository.Get((Guid)result.Value));
            Assert.Equal(referencePerson, createdPerson);
        }

        [Fact]
        public void UpdatePerson_Test()
        {
            // Arrange
            var person = new PersonDetailModel()
            {
                Id = DbSeed.Person1.Id,
                FirstName = "New",
                LastName = "Name",
                Description = DbSeed.Person1.Description,
                PhotoURL = DbSeed.Person1.PhotoURL,
                TeamId = DbSeed.Person1.TeamId,
                TeamName = DbSeed.Person1.Team.TeamName,
                TeamLogoURL = DbSeed.Person1.Team.LogoURL
            };

            // Act
            var result = _controllerSUT.Update(person).Result as CreatedResult;

            // Assert
            var updatedPerson = _mapper.Map<PersonDetailModel>(_repository.Get((Guid)result.Value));
            Assert.Equal(person, updatedPerson);
        }

        [Fact]
        public void DeletePerson_Test()
        {
            // Arrange

            // Act
            var result = _controllerSUT.Delete(DbSeed.Person2.Id);

            // Assert
            var deletedPerson = _repository.Get(DbSeed.Person2.Id);
            Assert.Null(deletedPerson);
        }

        [Fact]
        public void SearchPerson_Test1()
        {
            // Arrange
            IEnumerable personList = new List<PersonListModel>()
            {
                _mapper.Map<PersonListModel>(DbSeed.Person1),
            };

            // Act
            var result = _controllerSUT.Search("John");

            // Assert
            Assert.Equal(personList, result.Value);
        }

        [Fact]
        public void SearchPerson_Test2()
        {
            // Arrange
            IEnumerable personList = new List<PersonListModel>()
            {
                _mapper.Map<PersonListModel>(DbSeed.Person1),
                _mapper.Map<PersonListModel>(DbSeed.Person2)
            };

            // Act
            var result = _controllerSUT.Search("named");

            // Assert
            Assert.Equal(personList, result.Value);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}

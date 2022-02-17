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
    public class PlaceApiDalIntegrationTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly PlaceRepository _repository;
        private readonly Mapper _mapper;
        private readonly PlaceController _controllerSUT;

        public PlaceApiDalIntegrationTests()
        {
            // Create db
            _dbContextFactory = new DbContextInMemoryFactory(nameof(PlaceApiDalIntegrationTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();

            _repository = new PlaceRepository(_dbContextFactory);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);

            _controllerSUT = new PlaceController(_mapper, _repository);
        }

        [Fact]
        public void GetAllPlaces_Test()
        {
            // Arrange
            IEnumerable placeList = new List<PlaceListModel>()
            {
                _mapper.Map<PlaceListModel>(DbSeed.Place1),
                _mapper.Map<PlaceListModel>(DbSeed.Place2)
            };

            // Act
            var result = (OkObjectResult)_controllerSUT.Get().Result;

                // Assert
            Assert.Equal(placeList, result.Value);
        }

        [Fact]
        public void GetPlace_Test()
        {
            // Arrange
            var place = _mapper.Map<PlaceDetailModel>(DbSeed.Place1);

            // Act
            var result = (OkObjectResult)_controllerSUT.Get(place.Id).Result;

            // Assert
            Assert.Equal(place, result.Value);
        }

        [Fact]
        public void CreatePlace_Test()
        {
            // Arrange
            var place = new PlaceDetailModel()
            {
                Name = "New",
                Description = "New description",
                Matches = new List<MatchListModel>() {}
            };

            // Act
            var result = _controllerSUT.Create(place).Result as CreatedResult;

            // Assert
            var referencePlace = new PlaceDetailModel()
            {
                Id = (Guid)result.Value,
                Name = place.Name,
                Description = place.Description,
                Matches = place.Matches
            };
            var createdPlace = _mapper.Map<PlaceDetailModel>(_repository.Get((Guid)result.Value));
            Assert.Equal(referencePlace, createdPlace);
        }

        [Fact]
        public void UpdatePlace_Test()
        {
            // Arrange
            var place = new PlaceDetailModel()
            {
                Id = DbSeed.Place1.Id,
                Name = DbSeed.Place1.Name,
                Description = DbSeed.Place1.Description,
                Matches = new List<MatchListModel>() { _mapper.Map<MatchListModel>(DbSeed.Match1) }
            };

            // Act
            var result = _controllerSUT.Update(place).Result as CreatedResult;

            // Assert
            var updatedPlace = _mapper.Map<PlaceDetailModel>(_repository.Get((Guid)result.Value));
            Assert.Equal(place, updatedPlace);
        }

        [Fact]
        public void DeletePlace_Test()
        {
            // Arrange

            // Act
            var result = _controllerSUT.Delete(DbSeed.Place1.Id);

            // Assert
            var deletedPlace = _repository.Get(DbSeed.Place1.Id);
            Assert.Null(deletedPlace);
        }

        [Fact]
        public void SearchPlace_Test1()
        {
            // Arrange
            IEnumerable placeList = new List<PlaceListModel>()
            {
                _mapper.Map<PlaceListModel>(DbSeed.Place1),
            };

            // Act
            var result = _controllerSUT.Search("Ave");

            // Assert
            Assert.Equal(placeList, result.Value);
        }

        [Fact]
        public void SearchPlace_Test2()
        {
            // Arrange
            IEnumerable placeList = new List<PlaceListModel>()
            {
                _mapper.Map<PlaceListModel>(DbSeed.Place1),
                _mapper.Map<PlaceListModel>(DbSeed.Place2)
            };

            // Act
            var result = _controllerSUT.Search("place");

            // Assert
            Assert.Equal(placeList, result.Value);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}

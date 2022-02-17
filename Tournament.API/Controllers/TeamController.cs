using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AutoMapper;
using Netizine.Enums;
using NSwag.Annotations;
using Tournament.Common.Models;
using Tournament.DAL.Entities;
using Tournament.DAL.Repositories;

namespace Tournament.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private const string ApiOperationBaseName = "Team";
        private readonly IMapper _mapper;
        private readonly IRepository<TeamEntity> _repository;

        public TeamController(IMapper mapper, IRepository<TeamEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<TeamListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<TeamListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<TeamListModel>(entity));
            }

            return result;
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TeamDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return _mapper.Map<TeamDetailModel>(result);
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create(TeamDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var result = _repository.Insert(_mapper.Map<TeamEntity>(model));

            if (result is null) return BadRequest();

            return Created($"api/Team/{result.Id}", result.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(TeamDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<TeamEntity>(model));

            if (result is null) return NotFound();

            return Created($"api/Team/{result.Id}", result.Id);
        }

        [HttpDelete("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Delete))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Delete(Guid id)
        {
            _repository.Delete(id);

            return Ok();
        }

        [HttpGet("search/")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Search))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TeamListModel>> Search([FromQuery] string search)
        {
            if (string.IsNullOrEmpty(search)) return BadRequest();

            var entityList = _repository.GetAll();
            var resultList = new List<TeamListModel>();

            foreach(var entity in entityList)
            {
                var countryName = entity.RegistrationCountry.GetName();
                var description = entity.Description ?? string.Empty;
                if (entity.TeamName.Contains(search) || description.Contains(search) || countryName.Contains(search))
                {
                    resultList.Add(_mapper.Map<TeamListModel>(entity));
                }
            }

            return resultList;
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NSwag.Annotations;
using AutoMapper;
using Tournament.DAL.Entities;
using Tournament.DAL.Repositories;
using Tournament.Common.Models;

namespace Tournament.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private const string ApiOperationBaseName = "Person";
        private readonly IMapper _mapper;
        private readonly IRepository<PersonEntity> _repository;

        public PersonController(IMapper mapper, IRepository<PersonEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PersonListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<PersonListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<PersonListModel>(entity));
            }

            return result;
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PersonDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return _mapper.Map<PersonDetailModel>(result);
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create([FromBody]PersonDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Insert(_mapper.Map<PersonEntity>(model));

            if (result is null) return BadRequest();

            return Created($"api/Person/{result.Id}", result.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(PersonDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<PersonEntity>(model));

            if (result is null) return NotFound();

            return Created($"api/Person/{result.Id}", result.Id);
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
        public ActionResult<IEnumerable<PersonListModel>> Search([FromQuery]string search)
        {
            if (string.IsNullOrEmpty(search)) return BadRequest();

            var entityList = _repository.GetAll();
            var resultList = new List<PersonListModel>();

            foreach (var entity in entityList)
            {
                var description = entity.Description ?? string.Empty;
                if (entity.FirstName.Contains(search) || entity.LastName.Contains(search) || description.Contains(search))
                {
                    resultList.Add(_mapper.Map<PersonListModel>(entity));
                }
            }

            return resultList;
        }
    }
}

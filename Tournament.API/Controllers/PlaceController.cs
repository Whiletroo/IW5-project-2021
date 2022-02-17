using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Tournament.Common.Models;
using Tournament.DAL.Entities;
using Tournament.DAL.Repositories;

namespace Tournament.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController : ControllerBase
    {
        private const string ApiOperationBaseName = "Place";
        private readonly IMapper _mapper;
        private readonly IRepository<PlaceEntity> _repository;

        public PlaceController(IMapper mapper, IRepository<PlaceEntity> repository) 
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PlaceListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<PlaceListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<PlaceListModel>(entity));
            }
        
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PlaceDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return Ok(_mapper.Map<PlaceDetailModel>(result));
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create([FromBody]PlaceDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = _repository.Insert(_mapper.Map<PlaceEntity>(model));

            if (result is null) return BadRequest();

            var detailModel = _mapper.Map<PlaceDetailModel>(result);

            return Created($"api/Place/{detailModel.Id}", detailModel.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(PlaceDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<PlaceEntity>(model));

            if (result is null) return NotFound();
        
            return Created($"api/Place/{result.Id}", result.Id);
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
        public ActionResult<IEnumerable<PlaceListModel>> Search([FromQuery] string search)
        {
            if (string.IsNullOrEmpty(search)) return BadRequest();

            var entityList = _repository.GetAll();
            var resultList = new List<PlaceListModel>();

            foreach(var entity in entityList)
            {
                var description = entity.Description ?? string.Empty;
                if (entity.Name.Contains(search) || description.Contains(search))
                {
                    resultList.Add(_mapper.Map<PlaceListModel>(entity));
                }
            }

            return resultList;
        }
    }
}
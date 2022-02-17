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
    public class MatchController : ControllerBase
    {
        private const string ApiOperationBaseName = "Match";
        private readonly IMapper _mapper;
        private readonly IRepository<MatchEntity> _repository;

        public MatchController(IMapper mapper, IRepository<MatchEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MatchListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<MatchListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<MatchListModel>(entity));
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MatchDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return Ok(_mapper.Map<MatchDetailModel>(result));
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create([FromBody] MatchDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Insert(_mapper.Map<MatchEntity>(model));

            if (result is null) return BadRequest();

            var detailModel = _mapper.Map<MatchDetailModel>(result);

            return Created($"api/Match/{detailModel.Id}", detailModel.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(MatchDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<MatchEntity>(model));

            if (result is null) return NotFound();

            return Created($"api/Match/{result.Id}", result.Id);
        }

        [HttpDelete("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Delete))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Delete(Guid id)
        {
            _repository.Delete(id);

            return Ok();
        }
    }
}
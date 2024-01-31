using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers
{
    /// <summary>
    /// Operation
    /// </summary>
    [Route("api")]
    [ApiController]
    [Authorize]
    public class OperationController : ControllerBase
    {
        public IMediator _mediator { get; set; }
        /// <summary>
        /// Operation
        /// </summary>
        /// <param name="mediator"></param>
        public OperationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get Operation By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("Operation/{id}", Name = "GetOperation")]
        [Produces("application/json", "application/xml", Type = typeof(OperationDto))]
        public async Task<IActionResult> GetOperation(Guid id)
        {
            var getOperationQuery = new GetOperationQuery
            {
                Id = id
            };
            var result = await _mediator.Send(getOperationQuery);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Get All Operations
        /// </summary>
        /// <returns></returns>
        [HttpGet("Operations")]
        [Produces("application/json", "application/xml", Type = typeof(List<OperationDto>))]
        public async Task<IActionResult> GetOperations()
        {
            var getAllOperationQuery = new GetAllOperationQuery { };
            var result = await _mediator.Send(getAllOperationQuery);
            return Ok(result);
        }
        /// <summary>
        /// Create A Operation
        /// </summary>
        /// <param name="addOperationCommand"></param>
        /// <returns></returns>
        [HttpPost("Operation")]
        [Produces("application/json", "application/xml", Type = typeof(OperationDto))]
        public async Task<IActionResult> AddOperation(AddOperationCommand addOperationCommand)
        {
            var result = await _mediator.Send(addOperationCommand);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }
            return CreatedAtAction("GetOperation", new { id = result.Id }, result);
        }
        /// <summary>
        /// Update Exist Operation By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateOperationCommand"></param>
        /// <returns></returns>
        [HttpPut("Operation/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(OperationDto))]
        public async Task<IActionResult> UpdateOperation(Guid Id, UpdateOperationCommand updateOperationCommand)
        {
            updateOperationCommand.Id = Id;
            var result = await _mediator.Send(updateOperationCommand);
            return StatusCode(result.StatusCode, result);

        }
        /// <summary>
        /// Delete Operation By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Operation/{Id}")]
        public async Task<IActionResult> DeleteOperation(Guid Id)
        {
            var deleteOperationCommand = new DeleteOperationCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(deleteOperationCommand);
            return StatusCode(result.StatusCode, result);
        }
    }
}

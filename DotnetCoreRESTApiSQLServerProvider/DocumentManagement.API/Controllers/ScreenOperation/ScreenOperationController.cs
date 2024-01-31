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
    /// ScreenOperation
    /// </summary>
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ScreenOperationController : ControllerBase
    {
        public IMediator _mediator { get; set; }
        /// <summary>
        /// ScreenOperation
        /// </summary>
        /// <param name="mediator"></param>
        public ScreenOperationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get All Screen Operations
        /// </summary>
        /// <returns></returns>
        [HttpGet("ScreenOperations")]
        [Produces("application/json", "application/xml", Type = typeof(List<ScreenOperationDto>))]
        public async Task<IActionResult> GetScreenOperations()
        {
            var getAllScreenOperationQuery = new GetAllScreenOperationQuery { };
            var result = await _mediator.Send(getAllScreenOperationQuery);
            return Ok(result);
        }
        /// <summary>
        /// Add Screen Operation
        /// </summary>
        /// <param name="addScreenOperationCommand"></param>
        /// <returns></returns>
        [HttpPost("ScreenOperation")]
        [Produces("application/json", "application/xml", Type = typeof(ScreenOperationDto))]
        public async Task<IActionResult> AddScreenOperation(AddScreenOperationCommand addScreenOperationCommand)
        {
            var result = await _mediator.Send(addScreenOperationCommand);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }
            return CreatedAtAction("GetScreenOperation", new { id = result.Id }, result);
        }
        /// <summary>
        /// Update Screen Operation By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateScreenOperationCommand"></param>
        /// <returns></returns>
        [HttpPut("ScreenOperation/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(ScreenOperationDto))]
        public async Task<IActionResult> UpdateScreenOperation(Guid Id, UpdateScreenOperationCommand updateScreenOperationCommand)
        {
            updateScreenOperationCommand.Id = Id;
            var result = await _mediator.Send(updateScreenOperationCommand);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Delete Screen Operation By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("ScreenOperation/{Id}")]
        public async Task<IActionResult> DeleteScreenOperation(Guid Id)
        {
            var deleteScreenCommand = new DeleteScreenOperationCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(deleteScreenCommand);
            return StatusCode(result.StatusCode, result);
        }
    }
}

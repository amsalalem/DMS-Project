using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers
{
    /// </summary>
    [Route("api/DocumentToken")]
    [ApiController]
    [Authorize]
    public class DocumenTokenController : BaseController
    {
        public IMediator _mediator { get; set; }

        /// <summary>
        /// DocumentLibrary
        /// </summary>
        /// <param name="mediator"></param>
        public DocumenTokenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the document token.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="isVersion"></param>
        /// <returns></returns>
        [HttpGet("{id}/token")]
        [AllowAnonymous]
        public async Task<ActionResult> GetDocumentToken(Guid id)
        {
            var getDocumentTokenQuery = new GetDocumentTokenQuery
            {
                Id = id,
            };
            var result = await _mediator.Send(getDocumentTokenQuery);
            return Ok(new { result });
        }

        /// <summary>
        /// Deletes the document token.
        /// </summary>
        /// <param name="token">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{token}")]
        public async Task<ActionResult> DeleteDocumentToken(Guid token)
        {
            var deleteDocumentTokenCommand = new DeleteDocumentTokenCommand { Token = token };
            var result = await _mediator.Send(deleteDocumentTokenCommand);
            return Ok(result);
        }

    }
}

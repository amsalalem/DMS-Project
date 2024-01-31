using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers.DocumentVersion
{
    /// <summary>
    /// Document
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentVersionController : BaseController
    {
        public IMediator _mediator { get; set; }
        private IWebHostEnvironment _webHostEnvironment;
        private PathHelper _pathHelper;

        /// <summary>
        /// Document
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="pathHelper"></param>
        public DocumentVersionController(
            IMediator mediator,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper
            )
        {
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;

        }

        /// <summary>
        /// Create a document.
        /// </summary>
        /// <param name="addDocumentVersion"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        [Produces("application/json", "application/xml", Type = typeof(DocumentVersionDto))]
        public async Task<IActionResult> AddDocumentVersion([FromForm] UploadNewDocumentVersionCommand uploadNewDocumentVersionCommand)
        {
            var result = await _mediator.Send(uploadNewDocumentVersionCommand);
            return GenerateResponse(result);
        }

        /// <summary>
        /// Gets the document versions.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentVersions(Guid id)
        {
            var updateUserProfilePhotoCommand = new GetDocumentVersionCommand()
            {
                Id = id
            };
            var result = await _mediator.Send(updateUserProfilePhotoCommand);
            return Ok(result);
        }
        /// <summary>
        /// Restores the document version.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="versionId">The version identifier.</param>
        /// <returns></returns>
        [HttpPost("{id}/restore/{versionId}")]
        public async Task<IActionResult> RestoreDocumentVersion(Guid id, Guid versionId)
        {
            var updateUserProfilePhotoCommand = new RestoreDocumentVersionCommand()
            {
                Id = versionId,
                DocumentId = id
            };
            var result = await _mediator.Send(updateUserProfilePhotoCommand);
            return GenerateResponse(result);
        }
    }
}

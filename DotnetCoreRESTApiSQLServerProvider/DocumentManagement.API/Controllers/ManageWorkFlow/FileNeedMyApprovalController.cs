using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Commands.ManageWorkFlow;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.MediatR.Queries.ManageWorkFlow;
using DocumentManagement.Repository;
using DocumentManagement.Repository.ManageWorkFlow;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers.ManageWorkFlow
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileNeedMyApprovalController : BaseController
    {
        public IMediator _mediator { get; set; }
        public FileNeedMyApprovalController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Produces("application/json", "application/xml", Type = typeof(WorkFlowList))]
        public async Task<IActionResult> GetFileNeedMyApprovals([FromQuery] WorkFlowResource documentResource)
        {
            var getDocumentLibraryQuery = new GetFileNeedMyApprovalLibraryQuery
            {
                WorkFlowResource = documentResource
            };

            if (string.IsNullOrWhiteSpace(Email))
            {
                return Unauthorized();
            }

            getDocumentLibraryQuery.Email = Email;
            var result = await _mediator.Send(getDocumentLibraryQuery);
            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(result);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Produces("application/json", "application/xml", Type = typeof(WorkflowHistoryDto))]
        public async Task<IActionResult> CreateFileNeedMyApproval(ApproveWorkFlowCommand addDocumentCommand)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                return Unauthorized();
            }
            addDocumentCommand.Email =Email;
            var result = await _mediator.Send(addDocumentCommand);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.Errors);
            }
            return RedirectToAction("GetFileNeedMyApprovals");
        }

    }
}

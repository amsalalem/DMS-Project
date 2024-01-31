using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Resources;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands.ManageWorkFlow;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto.ManageDocument;
using DocumentManagement.MediatR.Handlers.ManageworkFlow.WorkFlows;
using DocumentManagement.Repository.ManageWorkFlow;
using DocumentManagement.MediatR.Queries.ManageWorkFlow;

namespace DocumentManagement.API.Controllers.ManageWorkFlow
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkFlowController : BaseController
    {
        public IMediator _mediator { get; set; }
        private IWebHostEnvironment _webHostEnvironment;
        private PathHelper _pathHelper;
        public WorkFlowController(
           IMediator mediator,
           IWebHostEnvironment webHostEnvironment,
           PathHelper pathHelper
           )
        {
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;

        }
        [HttpGet]
        [Produces("application/json", "application/xml", Type = typeof(DocumentList))]
        public async Task<IActionResult> GetApprovalDocuments([FromQuery] WorkFlowResource documentResource)
        {
            //var getAllDocumentQuery = new GetApprovalStatusDocumentQuery
            //{
            //    DocumentResource = documentResource
            //};
            var getAllDocumentQuery = new GetAllWorkFlowQuery
            {
                WorkFlowResource = documentResource
            };
            var result = await _mediator.Send(getAllDocumentQuery);

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
        [Produces("application/json", "application/xml", Type = typeof(WorkFlowDto))]
        public async Task<IActionResult> CreateWorkFlow(AddWorkFlowCommand addDocumentCommand)
        {
            var result = await _mediator.Send(addDocumentCommand);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.Errors);
            }
            return RedirectToAction("GetApprovalDocuments");
        }

        [HttpGet("{id}/participants")]
        [Produces("application/json", "application/xml", Type = typeof(List<WorkflowParticipantDto>))]
        public async Task<IActionResult> GetWorkFlowParticipants(Guid id, bool isDeleted = false)
        {
            var getWorkFlowQuery = new GetWorkFlowParticipantCommand { Id = id, isDeleted = isDeleted };
            var workFlowParticipant = await _mediator.Send(getWorkFlowQuery);
            return Ok(workFlowParticipant);
        }
        [HttpGet("WorkFlow/Approval")]
        [Produces("application/json", "application/xml", Type = typeof(WorkFlowList))]
        public async Task<IActionResult> GetWorkFlows([FromQuery] WorkFlowResource documentResource)
        {
            var getAllDocumentQuery = new GetAllWorkFlowQuery
            {
                WorkFlowResource = documentResource
            };
            var result = await _mediator.Send(getAllDocumentQuery);

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


    }
}

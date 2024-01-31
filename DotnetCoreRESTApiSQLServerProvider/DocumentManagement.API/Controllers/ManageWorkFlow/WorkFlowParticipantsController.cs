using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Data.Enums;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands.ManageWorkFlow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers.ManageWorkFlow
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class WorkFlowParticipantsController : ControllerBase
    {
        private readonly DocumentContext _documentContext;
        public WorkFlowParticipantsController(DocumentContext documentContext)
        {
            _documentContext = documentContext;
        }
        [HttpPost("WorkFlowParticipants")]
        [DisableRequestSizeLimit]
        [Produces("application/json", "application/xml", Type = typeof(WorkFlowDto))]
        public async Task<IActionResult> AddWorkFlowParticpants([FromForm] WorkFlowParticipantsDto addDocumentCommand)
        {

            try
            {
                WorkFlowParticipants result = new WorkFlowParticipants();


                foreach (var participant in addDocumentCommand.ParticipantId)
                {
                    result = new WorkFlowParticipants
                    {
                        DocumentId = addDocumentCommand.DocumentId,
                        ParticipantId = participant,
                        WorkFlowType =(WorkFlowType)addDocumentCommand.WorkFlowType,
                        ParticipantType =(ParticipantType)addDocumentCommand.ParticipantType,
                        Order = addDocumentCommand.Order
                    };

                    _documentContext.Add(result);
                     await _documentContext.SaveChangesAsync();
                }
               
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

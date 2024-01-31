using DocumentManagement.Data.Dto.ManageDocument;
using MediatR;
using System;
using System.Collections.Generic;
namespace DocumentManagement.MediatR.Commands.ManageWorkFlow
{
    public class GetWorkFlowParticipantCommand : IRequest<List<WorkflowParticipantDto>>
    {
        public Guid Id { get; set; }
        public bool isDeleted { get; set; }
    }
}

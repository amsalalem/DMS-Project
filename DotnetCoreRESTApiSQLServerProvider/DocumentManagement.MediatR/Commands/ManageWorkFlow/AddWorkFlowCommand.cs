using DocumentManagement.Data.Dto.ManageDocument;
using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Enums;
using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands.ManageWorkFlow
{
    public class AddWorkFlowCommand : IRequest<ServiceResponse<WorkFlowDto>>
    {
        public Guid DocumentId { get; set; }
        public WorkflowMethod WorkflowMethod { get; set; }
        public WorkFlowType WorkFlowType { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public List<WorkflowParticipantDto> WorkFlowParticipant { get; set; }
    }
}

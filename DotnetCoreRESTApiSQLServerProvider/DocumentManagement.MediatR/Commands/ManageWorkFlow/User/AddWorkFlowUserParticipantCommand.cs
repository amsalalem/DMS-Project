using DocumentManagement.Data.Dto.ManageWorkFlow;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands.ManageWorkFlow.User
{
    public class AddWorkFlowUserParticipantCommand : IRequest<WorkflowUserParticipantDto>
    {
        public ICollection<WorkflowUserParticipantDto> WorkflowUserParticipants { get; set; }
    }
}

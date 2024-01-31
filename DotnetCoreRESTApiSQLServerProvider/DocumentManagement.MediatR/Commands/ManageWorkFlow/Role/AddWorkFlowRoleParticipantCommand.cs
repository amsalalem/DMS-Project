using DocumentManagement.Data.Dto.ManageWorkFlow;
using MediatR;
using System.Collections.Generic;
namespace DocumentManagement.MediatR.Commands.ManageWorkFlow.Role
{
    public class AddWorkFlowRoleParticipantCommand : IRequest<WorkflowRoleParticipantDto>
    {
        public ICollection<WorkflowRoleParticipantDto> WorkflowRoleParticipants { get; set; }
    }
}

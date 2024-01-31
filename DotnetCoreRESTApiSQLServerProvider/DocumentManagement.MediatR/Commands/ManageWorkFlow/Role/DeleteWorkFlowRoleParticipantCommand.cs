using DocumentManagement.Data.Dto.ManageWorkFlow;
using MediatR;
using System;
namespace DocumentManagement.MediatR.Commands.ManageWorkFlow.Role
{
    public class DeleteWorkFlowRoleParticipantCommand : IRequest<WorkflowRoleParticipantDto>
    {
        public Guid Id { get; set; }
    }
}

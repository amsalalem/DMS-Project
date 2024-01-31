using DocumentManagement.Data.Dto.ManageWorkFlow;
using MediatR;
using System;
namespace DocumentManagement.MediatR.Commands.ManageWorkFlow.User
{
    public class DeleteWorkFlowUserParticipantCommand : IRequest<WorkflowUserParticipantDto>
    {
        public Guid Id { get; set; }
    }
}

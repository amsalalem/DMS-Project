using DocumentManagement.Data.Dto.ManageDocument;
using DocumentManagement.MediatR.Commands.ManageWorkFlow;
using DocumentManagement.Repository.ManageWorkFlow;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.ManageworkFlow.WorkFlows
{
    public class GetWorkFlowParticipantCommandHandler : IRequestHandler<GetWorkFlowParticipantCommand, List<WorkflowParticipantDto>>
    {
        private readonly IWorkFlowParticipantRepository _salesOrderItemRepository;

        public GetWorkFlowParticipantCommandHandler(IWorkFlowParticipantRepository salesOrderItemRepository)
        {
            _salesOrderItemRepository = salesOrderItemRepository;
        }

        public async Task<List<WorkflowParticipantDto>> Handle(GetWorkFlowParticipantCommand request, CancellationToken cancellationToken)
        {

            var itemsQuery = _salesOrderItemRepository.AllIncluding(c => c.Workflow.Document, cs => cs.Workflow)
                .Where(c => c.WorkflowId == request.Id);

            if (request.isDeleted)
            {
                itemsQuery = itemsQuery.Where(c => c.ApprovalStatus != Data.Enums.ApprovalStatus.New);
            }

            var items = await itemsQuery
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new WorkflowParticipantDto
                {
                    AproverName = c.User.FirstName + " " + c.User.LastName,
                    WorkflowId = c.WorkflowId,
                    Id = c.Id,
                    UserId = c.UserId,
                    ApprovalStatus = c.ApprovalStatus,
                    OrderNumber = c.OrderNumber,
                }).ToListAsync();
            return items;
        }
    }
}

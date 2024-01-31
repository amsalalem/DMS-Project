using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Enums;
using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands.ManageWorkFlow
{
    public class ApproveWorkFlowCommand : IRequest<ServiceResponse<WorkflowHistoryDto>>
    {
        public Guid WorkflowId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string Comment { get; set; }
        public string Email { get; set; }
    }
}

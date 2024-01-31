using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Dto.ManageWorkFlow
{
    public class WorkflowHistoryDto : ErrorStatusCode
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public WorkFlowDto Workflow { get; set; }
        public string CreatedByUser { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string Comment { get; set; }
    }
}

using DocumentManagement.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Entities.ManageWorkflow
{
    public class WorkFlowParticipant:BaseEntity
    {
        public Guid? Id { get; set; }
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }
        public int OrderNumber { get; set; }
        public bool CanViewToApprove { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}

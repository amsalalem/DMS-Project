using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Entities.ManageWorkflow
{
    public class WorkFlowRoleParticipant : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public int OrderNumber { get; set; }
        public Guid RoleId { get; set; }
        List<WorkflowHistory> WorkflowHistory { get; set; } = new List<WorkflowHistory>();
    }
}

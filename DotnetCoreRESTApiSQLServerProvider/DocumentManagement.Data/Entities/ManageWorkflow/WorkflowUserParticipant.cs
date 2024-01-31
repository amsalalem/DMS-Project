using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data.Entities.ManageWorkflow
{
    public class WorkflowUserParticipant : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public int OrderNumber { get; set; }
        public Guid UserId { get; set; }
        List<WorkflowHistory> WorkflowHistory { get; set; } = new List<WorkflowHistory>();
    }
}

using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data.Entities.ManageWorkflow
{
    public class Workflow : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        public WorkflowMethod WorkflowMethod { get; set; }
        public WorkFlowType WorkFlowType { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        [ForeignKey("CreatedBy")]
        public User User { get; set; }
        public List<WorkFlowParticipant> WorkFlowParticipant { get; set; }
        List<WorkflowHistory> WorkflowHistory { get; set; }

    }
}

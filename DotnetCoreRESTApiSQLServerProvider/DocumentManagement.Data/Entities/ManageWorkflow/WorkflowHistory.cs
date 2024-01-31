using DocumentManagement.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace DocumentManagement.Data.Entities.ManageWorkflow
{
    public class WorkflowHistory:BaseEntity
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        [ForeignKey("WorkflowId")]
        public Workflow Workflow { get; set; }
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string Comment { get; set; }
    }
}

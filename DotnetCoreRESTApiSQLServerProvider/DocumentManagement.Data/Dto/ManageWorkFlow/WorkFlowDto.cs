using DocumentManagement.Data.Dto.ManageDocument;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data.Dto.ManageWorkFlow
{
    public class WorkFlowDto: ErrorStatusCode
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string Url { get; set; }
        public WorkflowMethod WorkflowMethod { get; set; }
        public WorkFlowType WorkFlowType { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public List<WorkflowParticipantDto> WorkFlowParticipant { get; set; }
        List<WorkflowHistoryDto> WorkflowHistory { get; set; }
        public DateTime CreatedDate { get; set; }
        //public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string WorkFlowSource { get; set; }
        
        
        
        
       
    
    }
}

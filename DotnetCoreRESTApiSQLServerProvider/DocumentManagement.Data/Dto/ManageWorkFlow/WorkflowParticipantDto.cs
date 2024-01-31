using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Enums;
using System;

namespace DocumentManagement.Data.Dto.ManageDocument
{
    public class WorkflowParticipantDto: ErrorStatusCode
    {
        public Guid? Id { get; set; }
        public Guid WorkflowId { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? UserId { get; set; }
        public string AproverName { get; set; }
        public WorkFlowDto WorkFlow { get; set; }
        public UserDto User { get; set; }
        public int OrderNumber { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}

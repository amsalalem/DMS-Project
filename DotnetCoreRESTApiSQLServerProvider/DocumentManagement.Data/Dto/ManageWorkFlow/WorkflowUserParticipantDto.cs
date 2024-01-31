using System;

namespace DocumentManagement.Data.Dto.ManageWorkFlow
{
    public class WorkflowUserParticipantDto: ErrorStatusCode
    {
        public Guid? Id { get; set; }
        public Guid? WorkflowId { get; set; }
        public Guid? UserId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public UserDto User { get; set;}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Dto.ManageWorkFlow
{
    public class WorkflowRoleParticipantDto : ErrorStatusCode
    {
        public Guid? Id { get; set; }
        public Guid? WorkflowId { get; set; }
        public Guid? RoleId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public RoleDto Role { get; set; }
    }
}

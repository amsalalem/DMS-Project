using DocumentManagement.Data.Entities.ManageWorkflow;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DocumentManagement.Data
{
    public class Role : IdentityRole<Guid>
    {
        public bool IsDeleted { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
        public virtual ICollection<DocumentRolePermission> DocumentRolePermissions { get; set; }
        public virtual ICollection<WorkFlowRoleParticipant> WorkFlowRoleParticipant { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;

namespace DocumentManagement.Data
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public Guid OperationId { get; set; }
        public Guid ScreenId { get; set; }
        public virtual Role Role { get; set; }
        public Operation Operation { get; set; }
        public Screen Screen { get; set; }
    }
}

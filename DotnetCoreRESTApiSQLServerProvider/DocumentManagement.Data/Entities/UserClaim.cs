using Microsoft.AspNetCore.Identity;
using System;

namespace DocumentManagement.Data
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public Guid OperationId { get; set; }
        public Guid ScreenId { get; set; }
        public virtual User User { get; set; }
        public Operation Operation { get; set; }
        public Screen Screen { get; set; }
    }
}

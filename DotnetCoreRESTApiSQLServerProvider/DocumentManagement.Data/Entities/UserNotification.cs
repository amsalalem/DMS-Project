using DocumentManagement.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class UserNotification : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public Guid? DocumentId { get; set; }
        public Document Document { get; set; }
    }
}

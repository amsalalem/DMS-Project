using System;

namespace DocumentManagement.Data
{
    public class ScreenOperation : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid OperationId { get; set; }
        public Guid ScreenId { get; set; }
        public Operation Operation { get; set; }
        public Screen Screen { get; set; }
    }
}

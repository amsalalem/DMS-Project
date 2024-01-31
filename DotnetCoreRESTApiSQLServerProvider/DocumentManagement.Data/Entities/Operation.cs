using System;

namespace DocumentManagement.Data
{
    public class Operation: BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

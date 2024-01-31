using System;

namespace DocumentManagement.Data.Dto
{
    public class CategoryDto : ErrorStatusCode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
    }
}

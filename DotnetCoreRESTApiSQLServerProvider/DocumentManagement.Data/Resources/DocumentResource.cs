using System;

namespace DocumentManagement.Data.Resources
{
    public class DocumentResource : ResourceParameter
    {
        public DocumentResource() : base("Name")
        {
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateDateString { get; set; }
        public string CreatedBy { get; set; }
        public string MetaTags { get; set; }
    }
}

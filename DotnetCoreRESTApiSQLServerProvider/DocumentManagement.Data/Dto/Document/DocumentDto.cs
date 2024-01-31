using DocumentManagement.Data.Enums;
using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto
{
    public class DocumentDto : ErrorStatusCode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string DocumentSource { get; set; }
        public string ViewerType { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public bool IsAllowDownload { get; set; }
        public List<DocumentMetaDataDto> DocumentMetaDatas { get; set; } = new List<DocumentMetaDataDto>();
    }
}

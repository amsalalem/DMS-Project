using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
namespace DocumentManagement.MediatR.Commands
{
    public class AddDocumentCommand : IRequest<ServiceResponse<DocumentDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public Guid CategoryId { get; set; }
        public string DocumentMetaDataString { get; set; }
        public IFormFileCollection Files { get; set; } = new FormFileCollection();
        public string DocumentUserPermissionString { get; set; }
        public string DocumentRolePermissionString { get; set; }
    }
}

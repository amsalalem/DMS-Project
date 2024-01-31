using DocumentManagement.Data.Dto;
using MediatR;
using System.Collections.Generic;
namespace DocumentManagement.MediatR.Queries.Document
{
    public class GetAllDocumentDropdownQuery : IRequest<List<DocumentDto>>
    {
        public bool IsAuthirized { get; set; }
    }
}

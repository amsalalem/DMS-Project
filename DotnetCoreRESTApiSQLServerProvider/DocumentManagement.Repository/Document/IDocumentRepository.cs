using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Resources;
using System;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface IDocumentRepository : IGenericRepository<Document>
    {
        Task<DocumentList> GetDocuments(DocumentResource documentResource);
        Task<DocumentList> GetApprovalDocuments(DocumentResource documentResource);
        Task<DocumentList> GetDocumentsLibrary(string email, DocumentResource documentResource);
        Task<DocumentDto> GetDocumentById(Guid Id);
    }
}

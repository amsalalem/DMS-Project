using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetAllDocumentQueryHandler : IRequestHandler<GetAllDocumentQuery, DocumentList>
    {
        private readonly IDocumentRepository _documentRepository;
        public GetAllDocumentQueryHandler(
           IDocumentRepository documentRepository
            )
        {
            _documentRepository = documentRepository;
          
        }
        public async Task<DocumentList> Handle(GetAllDocumentQuery request, CancellationToken cancellationToken)
        {
            return await _documentRepository.GetDocuments(request.DocumentResource);
        }
    }
}

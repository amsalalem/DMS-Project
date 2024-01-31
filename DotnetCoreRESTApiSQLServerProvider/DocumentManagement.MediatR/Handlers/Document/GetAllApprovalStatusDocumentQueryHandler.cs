using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetAllApprovalStatusDocumentQueryHandler : IRequestHandler<GetApprovalStatusDocumentQuery, DocumentList>
    {
        private readonly IDocumentRepository _documentRepository;
        public GetAllApprovalStatusDocumentQueryHandler(
           IDocumentRepository documentRepository
            )
        {
            _documentRepository = documentRepository;
          
        }
        public async Task<DocumentList> Handle(GetApprovalStatusDocumentQuery request, CancellationToken cancellationToken)
        {
            return await _documentRepository.GetApprovalDocuments(request.DocumentResource);
        }
    }
}

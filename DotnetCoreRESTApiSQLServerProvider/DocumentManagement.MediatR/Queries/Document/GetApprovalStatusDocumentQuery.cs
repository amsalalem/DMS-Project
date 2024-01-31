using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetApprovalStatusDocumentQuery : IRequest<DocumentList>
    {
        public DocumentResource DocumentResource { get; set; }
    }
}

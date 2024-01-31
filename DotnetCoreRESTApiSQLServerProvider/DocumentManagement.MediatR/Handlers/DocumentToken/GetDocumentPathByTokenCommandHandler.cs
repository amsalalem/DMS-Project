using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace YourDrive.MediatR.Handlers
{
    public class GetDocumentPathByTokenCommandHandler : IRequestHandler<GetDocumentPathByTokenCommand,bool>
    {
        private readonly IDocumentTokenRepository _documentTokenRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly PathHelper _pathHelper;

        public GetDocumentPathByTokenCommandHandler(IDocumentTokenRepository documentTokenRepository,
            IDocumentRepository documentRepository,
            PathHelper pathHelper)
        {
            _documentTokenRepository = documentTokenRepository;
            _documentRepository = documentRepository;
            _pathHelper = pathHelper;
        }
        public async Task<bool> Handle(GetDocumentPathByTokenCommand request, CancellationToken cancellationToken)
        {
            if ( await _documentTokenRepository.All.AnyAsync(c => c.DocumentId == request.Id && c.Token == request.Token))
            {
                return true;
            }
            return false;
        }
    }
}

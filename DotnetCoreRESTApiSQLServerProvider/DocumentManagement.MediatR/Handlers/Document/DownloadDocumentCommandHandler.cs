using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DownloadDocumentCommandHandler : IRequestHandler<DownloadDocumentCommand, string>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentVersionRepository _versionRepository;
        private readonly PathHelper _pathHelper;

        public DownloadDocumentCommandHandler(IDocumentRepository documentRepository,
            PathHelper pathHelper,
            IDocumentVersionRepository versionRepository)
        {
            _documentRepository = documentRepository;
            _pathHelper = pathHelper;
            _versionRepository = versionRepository;
        }

        public async Task<string> Handle(DownloadDocumentCommand request, CancellationToken cancellationToken)
        {
            string documentPath = string.Empty;
            if (request.IsVersion)
            {
                var version = await _versionRepository.All.FirstOrDefaultAsync(c => c.Id == request.Id);
                documentPath = version?.Url;
            }
            else
            {
                var document = await _documentRepository.All.FirstOrDefaultAsync(c => c.Id == request.Id);
                documentPath = document?.Url;
            }
            if (!string.IsNullOrWhiteSpace(documentPath))
            {
                return Path.Combine(_pathHelper.DocumentPath, documentPath);
            }
            return "";
        }
    }
}

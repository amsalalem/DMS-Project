using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetDocumentVersionCommandHandler : IRequestHandler<GetDocumentVersionCommand, List<DocumentVersionDto>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentVersionRepository _documentVersionRepository;

        public GetDocumentVersionCommandHandler(IDocumentRepository documentRepository,
            IDocumentVersionRepository documentVersionRepository)
        {
            _documentRepository = documentRepository;
            _documentVersionRepository = documentVersionRepository;
        }
        public async Task<List<DocumentVersionDto>> Handle(GetDocumentVersionCommand request, CancellationToken cancellationToken)
        {
            var document = await _documentRepository.AllIncluding(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            if (document == null)
            {
                return new List<DocumentVersionDto>();
            }

            var documnetVersions = await _documentVersionRepository.AllIncluding(c => c.CreatedByUser)
                .Where(c => c.DocumentId == request.Id)
                .Select(documnetVersion => new DocumentVersionDto
                {
                    Id = documnetVersion.Id,
                    Url = documnetVersion.Url,
                    CreatedByUser = $"{documnetVersion.CreatedByUser.FirstName}  {documnetVersion.CreatedByUser.LastName}",
                    ModifiedDate = documnetVersion.ModifiedDate,
                }).ToListAsync();

            documnetVersions.Add(new DocumentVersionDto
            {
                Id = document.Id,
                IsCurrentVersion = true,
                Url = document.Url,
                ModifiedDate = document.ModifiedDate,
                CreatedByUser = $"{document.User.FirstName}  {document.User.LastName}",
            });

            return documnetVersions.OrderByDescending(c => c.ModifiedDate).ToList();
        }
    }
}
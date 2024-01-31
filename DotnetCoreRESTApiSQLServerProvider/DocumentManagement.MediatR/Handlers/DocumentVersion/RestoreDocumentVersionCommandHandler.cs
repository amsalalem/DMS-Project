using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class RestoreDocumentVersionCommandHandler : IRequestHandler<RestoreDocumentVersionCommand, ServiceResponse<bool>>
    {
        private readonly IDocumentVersionRepository _documentVersionRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly UserInfoToken _userInfoToken;

        public RestoreDocumentVersionCommandHandler(IDocumentVersionRepository documentVersionRepository,
            IDocumentRepository documentRepository,
            IUnitOfWork<DocumentContext> uow,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            UserInfoToken userInfoToken)
        {
            _documentVersionRepository = documentVersionRepository;
            _documentRepository = documentRepository;
            _uow = uow;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _userInfoToken = userInfoToken;
        }
        public async Task<ServiceResponse<bool>> Handle(RestoreDocumentVersionCommand request, CancellationToken cancellationToken)
        {
            var document = await _documentRepository.FindAsync(request.DocumentId);
            if (document == null)
            {
                return ServiceResponse<bool>.Return404();
            }

            var originalPath = document.Url;
            var version = _documentVersionRepository
                .All.FirstOrDefault(c => c.Id == request.Id && c.DocumentId == request.DocumentId);
            if (version == null)
            {
                return ServiceResponse<bool>.Return404();
            }


            var versionId = Guid.NewGuid();
            var versionUrl = versionId.ToString() + Path.GetExtension(document.Url);
            _documentVersionRepository.Add(new DocumentVersion
            {
                Id = versionId,
                DocumentId = document.Id,
                Url = versionUrl,
                CreatedBy = document.CreatedBy,
                CreatedDate = document.CreatedDate,
                ModifiedDate = document.ModifiedDate,
                ModifiedBy = document.ModifiedBy
            });

            document.Url = version.Url;

            var rootPath = Path.Combine(_webHostEnvironment.ContentRootPath, _pathHelper.DocumentPath);
            // Copy Version File
            File.Copy(Path.Combine(rootPath, originalPath), Path.Combine(rootPath, versionUrl));

            _documentRepository.Update(document);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnSuccess(); ;
        }
    }
}

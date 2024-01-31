using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class UploadNewDocumentVersionCommandHandler : IRequestHandler<UploadNewDocumentVersionCommand, ServiceResponse<DocumentVersionDto>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentVersionRepository _documentVersionRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UploadNewDocumentVersionCommandHandler> _logger;
        private readonly UserInfoToken _userInfoToken;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadNewDocumentVersionCommandHandler(
            IDocumentRepository documentRepository,
            IDocumentVersionRepository documentVersionRepository,
            IUnitOfWork<DocumentContext> uow,
            IMapper mapper,
            ILogger<UploadNewDocumentVersionCommandHandler> logger,
            UserInfoToken userInfoToken,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment)
        {
            _documentRepository = documentRepository;
            _documentVersionRepository = documentVersionRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _userInfoToken = userInfoToken;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<ServiceResponse<DocumentVersionDto>> Handle(UploadNewDocumentVersionCommand request, CancellationToken cancellationToken)
        {
            if (request.Files.Count == 0)
            {
                return ServiceResponse<DocumentVersionDto>.ReturnFailed(409, "Please select the file.");
            }

            var doc = await _documentRepository.FindAsync(request.DocumentId);
            if (doc == null)
            {
                _logger.LogError("Document Not Found");
                return ServiceResponse<DocumentVersionDto>.Return500();
            }

            var url = UploadFile(request.Files[0]);

            var version = new DocumentVersion
            {
                Url = doc.Url,
                DocumentId = doc.Id,
                CreatedBy = doc.CreatedBy,
                CreatedDate = doc.CreatedDate,
                ModifiedBy = doc.ModifiedBy,
                ModifiedDate = doc.ModifiedDate,
            };
            doc.Url = url;
            doc.CreatedDate = DateTime.UtcNow;
            doc.CreatedBy = _userInfoToken.Id;

            _documentRepository.Update(doc);
            _documentVersionRepository.Add(version);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while adding industry");
                return ServiceResponse<DocumentVersionDto>.Return500();
            }
            var documentCommentDto = _mapper.Map<DocumentVersionDto>(version);
            return ServiceResponse<DocumentVersionDto>.ReturnResultWith200(documentCommentDto);
        }

        private string UploadFile(IFormFile file)
        {
            string uri = string.Empty;
            string documentPath = _pathHelper.DocumentPath;
            string contentRootpath = _webHostEnvironment.ContentRootPath;
            string newPath = Path.Combine(contentRootpath, documentPath);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string extesion = Path.GetExtension(fileName);
                uri = $"{Guid.NewGuid()}{extesion}";
                string fullPath = Path.Combine(newPath, uri);
                var bytesData = AesOperation.ReadAsBytesAsync(file);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    if (_pathHelper.AllowEncryption)
                    {
                        var byteArray = AesOperation.EncryptStream(bytesData, _pathHelper.AesEncryptionKey);
                        stream.Write(byteArray, 0, byteArray.Length);
                    }
                    else
                    {
                        stream.Write(bytesData, 0, bytesData.Length);
                    }
                }
            }

            return uri;
        }
    }
}

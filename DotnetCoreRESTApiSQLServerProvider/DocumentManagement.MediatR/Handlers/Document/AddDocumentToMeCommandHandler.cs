using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddDocumentToMeCommandHandler : IRequestHandler<AddDocumentToMeCommand, ServiceResponse<DocumentDto>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfo;
        private readonly IDocumentUserPermissionRepository _documentUserPermissionRepository;
        private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddDocumentToMeCommandHandler(
            IDocumentRepository documentRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow,
            UserInfoToken userInfo,
            IDocumentUserPermissionRepository documentUserPermissionRepository,
            IDocumentAuditTrailRepository documentAuditTrailRepository,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfo = userInfo;
            _documentUserPermissionRepository = documentUserPermissionRepository;
            _documentAuditTrailRepository = documentAuditTrailRepository;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse<DocumentDto>> Handle(AddDocumentToMeCommand request, CancellationToken cancellationToken)
        {
            if (request.Files.Count == 0)
            {
                return ServiceResponse<DocumentDto>.ReturnFailed(409, "Please select the file.");
            }

            var entityExist = await _documentRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return ServiceResponse<DocumentDto>.ReturnFailed(409, "Document already exist.");
            }

            var url = UploadFile(request.Files[0]);
            var entity = _mapper.Map<Data.Entities.Document>(request);
            entity.CreatedBy = _userInfo.Id;
            entity.CreatedDate = DateTime.UtcNow;
            entity.Url = url;

            try
            {
                var metaData = JsonConvert.DeserializeObject<List<DocumentMetaDataDto>>(request.DocumentMetaDataString);
                entity.DocumentMetaDatas = _mapper.Map<List<Data.DocumentMetaData>>(metaData);
            }
            catch
            {
                // igonre
            }

            _documentRepository.Add(entity);

            var documentUserPermission = new DocumentUserPermission
            {
                Id = Guid.NewGuid(),
                DocumentId = entity.Id,
                UserId = _userInfo.Id,
                IsTimeBound = false,
                IsAllowDownload = true,
                CreatedBy = _userInfo.Id,
                CreatedDate = DateTime.UtcNow

            };

            _documentUserPermissionRepository.Add(documentUserPermission);
            var documentAudit = new DocumentAuditTrail()
            {
                DocumentId = entity.Id,
                CreatedBy = _userInfo.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Add_Permission,
                AssignToUserId = _userInfo.Id
            };
            _documentAuditTrailRepository.Add(documentAudit);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<DocumentDto>.ReturnFailed(500, "Error While Added Document");
            }
            var entityDto = _mapper.Map<DocumentDto>(entity);
            return ServiceResponse<DocumentDto>.ReturnResultWith200(entityDto);
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
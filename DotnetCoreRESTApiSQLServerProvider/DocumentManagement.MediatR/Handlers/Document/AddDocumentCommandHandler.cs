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
    public class AddDocumentCommandHandler : IRequestHandler<AddDocumentCommand, ServiceResponse<DocumentDto>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfo;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AddDocumentCommandHandler(
            IDocumentRepository documentRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow,
            UserInfoToken userInfo,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfo = userInfo;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse<DocumentDto>> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
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

            try
            {
                var documentUserPermissions = JsonConvert.DeserializeObject<List<DocumentUserPermissionDto>>(request.DocumentUserPermissionString);
                entity.DocumentUserPermissions = _mapper.Map<List<Data.DocumentUserPermission>>(documentUserPermissions);
            }
            catch
            {
                // igonre
            }

            try
            {
                var documentRolePermissions = JsonConvert.DeserializeObject<List<DocumentRolePermissionDto>>(request.DocumentRolePermissionString);
                entity.DocumentRolePermissions = _mapper.Map<List<Data.DocumentRolePermission>>(documentRolePermissions);
            }
            catch
            {
                // igonre
            }

            _documentRepository.Add(entity);
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

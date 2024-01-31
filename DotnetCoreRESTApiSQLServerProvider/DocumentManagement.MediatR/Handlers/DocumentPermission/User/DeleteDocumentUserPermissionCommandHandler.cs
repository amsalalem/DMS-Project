using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteDocumentUserPermissionCommandHandler
        : IRequestHandler<DeleteDocumentUserPermissionCommand, DocumentUserPermissionDto>
    {
        private readonly IDocumentUserPermissionRepository _documentUserPermissionRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly UserInfoToken _userInfo;
        private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
        public DeleteDocumentUserPermissionCommandHandler(
           IDocumentUserPermissionRepository documentUserPermissionRepository,
            IUnitOfWork<DocumentContext> uow,
              UserInfoToken userInfo,
              IDocumentAuditTrailRepository documentAuditTrailRepository
            )
        {
            _documentUserPermissionRepository = documentUserPermissionRepository;
            _uow = uow;
            _userInfo = userInfo;
            _documentAuditTrailRepository = documentAuditTrailRepository;
        }

        public async Task<DocumentUserPermissionDto> Handle(DeleteDocumentUserPermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _documentUserPermissionRepository.FindAsync(request.Id);
            if (entity == null)
            {
                var errorDto = new DocumentUserPermissionDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found" }
                };
                return errorDto;
            }

            var documentAudit = new DocumentAuditTrail()
            {
                DocumentId = entity.DocumentId,
                CreatedBy = _userInfo.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Remove_Permission,
                AssignToUserId = entity.UserId
            };
            _documentAuditTrailRepository.Add(documentAudit);

            _documentUserPermissionRepository.Delete(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new DocumentUserPermissionDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            return new DocumentUserPermissionDto
            {
                StatusCode = 200,
                Messages = new List<string> { "Permission Deleted Successfully." }
            };
        }
    }
}

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
    public class DeleteDocumentRolePermissionCommandHandler : IRequestHandler<DeleteDocumentRolePermissionCommand, DocumentRolePermissionDto>
    {
        private readonly IDocumentRolePermissionRepository _documentRolePermissionRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly UserInfoToken _userInfo;
        private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
        public DeleteDocumentRolePermissionCommandHandler(
           IDocumentRolePermissionRepository documentRolePermissionRepository,
            IUnitOfWork<DocumentContext> uow,
            UserInfoToken userInfo,
              IDocumentAuditTrailRepository documentAuditTrailRepository
            )
        {
            _documentRolePermissionRepository = documentRolePermissionRepository;
            _uow = uow;
            _userInfo = userInfo;
            _documentAuditTrailRepository = documentAuditTrailRepository;
        }

        public async Task<DocumentRolePermissionDto> Handle(DeleteDocumentRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _documentRolePermissionRepository.FindAsync(request.Id);
            if (entity == null)
            {
                var errorDto = new DocumentRolePermissionDto
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
                AssignToRoleId = entity.RoleId
            };
            _documentAuditTrailRepository.Add(documentAudit);

            _documentRolePermissionRepository.Delete(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new DocumentRolePermissionDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            return new DocumentRolePermissionDto
            {
                StatusCode = 200,
                Messages = new List<string> { "Permission Deleted Successfully." }
            };
        }
    }
}

using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddDocumentUserPermissionCommandHandler
        : IRequestHandler<AddDocumentUserPermissionCommand, DocumentUserPermissionDto>
    {
        IDocumentUserPermissionRepository _documentUserPermissionRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
        private readonly UserInfoToken _userInfo;

        public AddDocumentUserPermissionCommandHandler(
            IDocumentUserPermissionRepository documentUserPermissionRepository,
            IUnitOfWork<DocumentContext> uow,
            IUserNotificationRepository userNotificationRepository,
            IMapper mapper,
               IDocumentAuditTrailRepository documentAuditTrailRepository,
            UserInfoToken userInfo)
        {
            _documentUserPermissionRepository = documentUserPermissionRepository;
            _uow = uow;
            _mapper = mapper;
            _userNotificationRepository = userNotificationRepository;
            _documentAuditTrailRepository = documentAuditTrailRepository;
            _userInfo = userInfo;
        }
        public async Task<DocumentUserPermissionDto> Handle(AddDocumentUserPermissionCommand request, CancellationToken cancellationToken)
        {
            var permissions = _mapper.Map<List<DocumentUserPermission>>(request.DocumentUserPermissions);
            permissions.ForEach(permission =>
            {
                if (permission.IsTimeBound)
                {
                    permission.StartDate = new DateTime(permission.StartDate.Value.Year, permission.StartDate.Value.Month, permission.StartDate.Value.Day).AddSeconds(1);
                    permission.EndDate = new DateTime(permission.EndDate.Value.Year, permission.EndDate.Value.Month, permission.EndDate.Value.Day).AddDays(1).AddSeconds(-1);
                }
            });
            _documentUserPermissionRepository.AddRange(permissions);
            var userIds = request.DocumentUserPermissions.Select(c => c.UserId).ToList();
            var documentId = request.DocumentUserPermissions.FirstOrDefault().DocumentId;

            List<DocumentAuditTrail> lstDocumentAuditTrail = new List<DocumentAuditTrail>();
            foreach (var userId in userIds)
            {
                var documentAudit = new DocumentAuditTrail()
                {
                    DocumentId = documentId.Value,
                    CreatedBy = _userInfo.Id,
                    CreatedDate = DateTime.UtcNow,
                    OperationName = DocumentOperation.Add_Permission,
                    AssignToUserId = userId
                };
                lstDocumentAuditTrail.Add(documentAudit);
            }
            if (lstDocumentAuditTrail.Count() > 0)
            {
                _documentAuditTrailRepository.AddRange(lstDocumentAuditTrail);
            }
            _userNotificationRepository.CreateUsersDocumentNotifiction(userIds, documentId.Value);
            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new DocumentUserPermissionDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            await _userNotificationRepository.SendNotification(userIds);

            return new DocumentUserPermissionDto();
        }
    }
}

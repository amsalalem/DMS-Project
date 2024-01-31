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

namespace DocumentManagement.MediatR.Handlers.DocumentPermission
{
    public class AddDocumentRolePermissionCommandHandler
         : IRequestHandler<AddDocumentRolePermissionCommand, DocumentRolePermissionDto>
    {

        private readonly IDocumentRolePermissionRepository _documentRolePermissionRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
        private readonly UserInfoToken _userInfo;

        public AddDocumentRolePermissionCommandHandler(
            IDocumentRolePermissionRepository documentRolePermissionRepository,
            IUnitOfWork<DocumentContext> uow,
            IUserNotificationRepository userNotificationRepository,
            IMapper mapper,
            IDocumentAuditTrailRepository documentAuditTrailRepository,
            UserInfoToken userInfo)
        {
            _documentRolePermissionRepository = documentRolePermissionRepository;
            _uow = uow;
            _mapper = mapper;
            _userNotificationRepository = userNotificationRepository;
            _documentAuditTrailRepository = documentAuditTrailRepository;
            _userInfo = userInfo;
        }
        public async Task<DocumentRolePermissionDto> Handle(AddDocumentRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var permissions = _mapper.Map<List<DocumentRolePermission>>(request.DocumentRolePermissions);
            permissions.ForEach(permission =>
            {
                if (permission.IsTimeBound)
                {
                    permission.StartDate = new DateTime(permission.StartDate.Value.Year, permission.StartDate.Value.Month, permission.StartDate.Value.Day).AddSeconds(1);
                    permission.EndDate = new DateTime(permission.EndDate.Value.Year, permission.EndDate.Value.Month, permission.EndDate.Value.Day).AddDays(1).AddSeconds(-1);
                }
            });
            _documentRolePermissionRepository.AddRange(permissions);

            var documentId = request.DocumentRolePermissions.FirstOrDefault().DocumentId;
            var roleIds = request.DocumentRolePermissions.Select(c => c.RoleId).Distinct().ToList();
            List<DocumentAuditTrail> lstDocumentAuditTrail = new List<DocumentAuditTrail>();
            foreach (var roleId in roleIds)
            {
                var documentAudit = new DocumentAuditTrail()
                {
                    DocumentId = documentId.Value,
                    CreatedBy = _userInfo.Id,
                    CreatedDate = DateTime.UtcNow,
                    OperationName = DocumentOperation.Add_Permission,
                    AssignToRoleId = roleId
                };
                lstDocumentAuditTrail.Add(documentAudit);
            }
            if (lstDocumentAuditTrail.Count() > 0)
            {
                _documentAuditTrailRepository.AddRange(lstDocumentAuditTrail);
            }
            var userIds = await _userNotificationRepository.CreateRolesDocumentNotifiction(roleIds, documentId.Value);

            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new DocumentRolePermissionDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }

            await _userNotificationRepository.SendNotification(userIds);
            //var documentid = request.DocumentRolePermissions.FirstOrDefault().DocumentId;
            //var roleIds = request.DocumentRolePermissions.Select(c => c.RoleId).Distinct().ToList();


            return new DocumentRolePermissionDto();
        }
    }
}

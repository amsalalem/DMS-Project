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
    public class DocumentPermissionUserRoleCommandHandler : IRequestHandler<DocumentPermissionUserRoleCommand, bool>
    {
        private readonly IDocumentRolePermissionRepository _documentRolePermissionRepository;
        private readonly IDocumentUserPermissionRepository _documentUserPermissionRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
        private readonly UserInfoToken _userInfo;

        public DocumentPermissionUserRoleCommandHandler(
            IDocumentRolePermissionRepository documentRolePermissionRepository,
            IUnitOfWork<DocumentContext> uow,
            IUserNotificationRepository userNotificationRepository,
            IMapper mapper,
            IDocumentAuditTrailRepository documentAuditTrailRepository,
            UserInfoToken userInfo,
            IDocumentUserPermissionRepository documentUserPermissionRepository)
        {
            _documentRolePermissionRepository = documentRolePermissionRepository;
            _uow = uow;
            _mapper = mapper;
            _userNotificationRepository = userNotificationRepository;
            _documentAuditTrailRepository = documentAuditTrailRepository;
            _documentUserPermissionRepository = documentUserPermissionRepository;
            _userInfo = userInfo;
        }

        public async Task<bool> Handle(DocumentPermissionUserRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.IsTimeBound)
            {
                request.StartDate = new DateTime(request.StartDate.Value.Year, request.StartDate.Value.Month, request.StartDate.Value.Day).AddSeconds(1);
                request.EndDate = new DateTime(request.EndDate.Value.Year, request.EndDate.Value.Month, request.EndDate.Value.Day).AddDays(1).AddSeconds(-1);
            }
            List<DocumentAuditTrail> lstDocumentAuditTrail = new List<DocumentAuditTrail>();

            List<Guid> userIds = new List<Guid>();
            if (request.Roles != null && request.Roles.Count() > 0)
            {
                List<DocumentRolePermission> lstDocumentRolePermission = new List<DocumentRolePermission>();

                foreach (var document in request.Documents)
                {
                    foreach (var role in request.Roles)
                    {

                        lstDocumentRolePermission.Add(new DocumentRolePermission
                        {
                            DocumentId = Guid.Parse(document),
                            RoleId = Guid.Parse(role),
                            StartDate = request.StartDate,
                            EndDate = request.EndDate,
                            IsTimeBound = request.IsTimeBound,
                            IsAllowDownload = request.IsAllowDownload,
                            CreatedBy = _userInfo.Id,
                            CreatedDate = DateTime.UtcNow
                        });

                        lstDocumentAuditTrail.Add(new DocumentAuditTrail()
                        {
                            DocumentId = Guid.Parse(document),
                            CreatedBy = _userInfo.Id,
                            CreatedDate = DateTime.UtcNow,
                            OperationName = DocumentOperation.Add_Permission,
                            AssignToRoleId = Guid.Parse(role)
                        });
                    }
                    List<Guid> roles = request.Roles.Select(c => Guid.Parse(c)).ToList();
                    userIds.AddRange(await _userNotificationRepository.CreateRolesDocumentNotifiction(roles, Guid.Parse(document)));
                }
                _documentRolePermissionRepository.AddRange(lstDocumentRolePermission);
            }

            if (request.Users != null && request.Users.Count() > 0)
            {
                List<DocumentUserPermission> lstDocumentUserPermission = new List<DocumentUserPermission>();

                foreach (var document in request.Documents)
                {
                    foreach (var user in request.Users)
                    {

                        lstDocumentUserPermission.Add(new DocumentUserPermission
                        {
                            DocumentId = Guid.Parse(document),
                            UserId = Guid.Parse(user),
                            StartDate = request.StartDate,
                            EndDate = request.EndDate,
                            IsTimeBound = request.IsTimeBound,
                            IsAllowDownload = request.IsAllowDownload,
                            CreatedBy = _userInfo.Id,
                            CreatedDate = DateTime.UtcNow
                        });

                        lstDocumentAuditTrail.Add(new DocumentAuditTrail()
                        {
                            DocumentId = Guid.Parse(document),
                            CreatedBy = _userInfo.Id,
                            CreatedDate = DateTime.UtcNow,
                            OperationName = DocumentOperation.Add_Permission,
                            AssignToUserId = Guid.Parse(user)
                        });

                    }
                    var tempUserIds = request.Users.Select(c => Guid.Parse(c)).ToList();
                    _userNotificationRepository.CreateUsersDocumentNotifiction(tempUserIds, Guid.Parse(document));
                    userIds.AddRange(tempUserIds);
                }
                _documentUserPermissionRepository.AddRange(lstDocumentUserPermission);
            }

            if (lstDocumentAuditTrail.Count() > 0)
            {
                _documentAuditTrailRepository.AddRange(lstDocumentAuditTrail);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new DocumentRolePermissionDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return false;
            }

            await _userNotificationRepository.SendNotification(userIds);

            return true;
        }
    }
}

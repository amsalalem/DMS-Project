﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Data.Resources;

namespace DocumentManagement.Repository
{
    public class LoginAuditRepository : GenericRepository<LoginAudit, DocumentContext>,
       ILoginAuditRepository
    {
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly ILogger<LoginAuditRepository> _logger;
        private readonly IPropertyMappingService _propertyMappingService;
        public LoginAuditRepository(
            IUnitOfWork<DocumentContext> uow,
            ILogger<LoginAuditRepository> logger,
            IPropertyMappingService propertyMappingService
            ) : base(uow)
        {
            _uow = uow;
            _logger = logger;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<LoginAuditList> GetLoginAudits(LoginAuditResource loginAuditResrouce)
        {
            var collectionBeforePaging = All;
            collectionBeforePaging =
               collectionBeforePaging.ApplySort(loginAuditResrouce.OrderBy,
               _propertyMappingService.GetPropertyMapping<LoginAuditDto, LoginAudit>());

            if (!string.IsNullOrWhiteSpace(loginAuditResrouce.UserName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.UserName, $"%{loginAuditResrouce.UserName}%"));
            }

            var loginAudits = new LoginAuditList();
            return await loginAudits.Create(
                collectionBeforePaging,
                loginAuditResrouce.Skip,
                loginAuditResrouce.PageSize
                );
        }

        public async Task LoginAudit(LoginAuditDto loginAudit)
        {
            try
            {
                Add(new LoginAudit
                {
                    Id = Guid.NewGuid(),
                    LoginTime = DateTime.UtcNow,
                    Provider = loginAudit.Provider,
                    Status = loginAudit.Status,
                    UserName = loginAudit.UserName,
                    RemoteIP = loginAudit.RemoteIP,
                    Latitude = loginAudit.Latitude,
                    Longitude = loginAudit.Longitude
                });
                await _uow.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}

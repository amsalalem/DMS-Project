using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class DocumentAuditTrailRepository : GenericRepository<DocumentAuditTrail, DocumentContext>,
        IDocumentAuditTrailRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly UserInfoToken _userInfoToken;
        public DocumentAuditTrailRepository(
            IUnitOfWork<DocumentContext> uow,
            IPropertyMappingService propertyMappingService,
            UserInfoToken userInfoToken
            ) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _userInfoToken = userInfoToken;
        }

        public async Task<DocumentAuditTrailList> GetDocumentAuditTrails(DocumentResource documentResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.CreatedByUser, d => d.Document, c => c.Document.Category);
            collectionBeforePaging =
               collectionBeforePaging
                .IgnoreQueryFilters()
                .ApplySort(documentResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<DocumentAuditTrailDto, DocumentAuditTrail>());

            if (!string.IsNullOrWhiteSpace(documentResource.Name))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.Document.Name, $"%{documentResource.Name}%"));
            }
            if (!string.IsNullOrWhiteSpace(documentResource.Id))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => c.DocumentId == Guid.Parse(documentResource.Id));
            }
            if (!string.IsNullOrWhiteSpace(documentResource.CategoryId))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => c.Document.CategoryId == Guid.Parse(documentResource.CategoryId) || c.Document.Category.ParentId == Guid.Parse(documentResource.CategoryId));
            }
            if (!string.IsNullOrWhiteSpace(documentResource.CreatedBy))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => c.CreatedBy == Guid.Parse(documentResource.CreatedBy));
            }
            var documentAuditTrailList = new DocumentAuditTrailList();
            return await documentAuditTrailList.Create(
                collectionBeforePaging,
                documentResource.Skip,
                documentResource.PageSize
                );
        }

    }
}

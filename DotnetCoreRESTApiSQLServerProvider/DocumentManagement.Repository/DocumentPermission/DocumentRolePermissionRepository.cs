using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class DocumentRolePermissionRepository : GenericRepository<DocumentRolePermission, DocumentContext>,
        IDocumentRolePermissionRepository
    {
        private readonly IUnitOfWork<DocumentContext> _uow;
        public DocumentRolePermissionRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
            _uow = uow;
        }
    }
}

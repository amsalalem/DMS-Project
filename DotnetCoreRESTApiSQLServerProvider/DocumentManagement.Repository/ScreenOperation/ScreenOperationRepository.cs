using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class ScreenOperationRepository : GenericRepository<ScreenOperation, DocumentContext>,
        IScreenOperationRepository
    {
        public ScreenOperationRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }
    }
}

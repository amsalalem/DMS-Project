using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class CategoryRepository : GenericRepository<Category, DocumentContext>,
           ICategoryRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public CategoryRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }
    }
}

using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class DocumentVersionRepository : GenericRepository<DocumentVersion, DocumentContext>, IDocumentVersionRepository
    {
        public DocumentVersionRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }

}
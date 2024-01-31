using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface IDocumentVersionRepository : IGenericRepository<DocumentVersion>
    {
    }
}

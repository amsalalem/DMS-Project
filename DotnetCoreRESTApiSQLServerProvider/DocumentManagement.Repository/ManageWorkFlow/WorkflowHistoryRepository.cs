using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Repository.ManageWorkFlow
{
    public class WorkflowHistoryRepository : GenericRepository<WorkflowHistory, DocumentContext>, IWorkflowHistoryRepository
    {
        public WorkflowHistoryRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

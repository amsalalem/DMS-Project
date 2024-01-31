using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Entities.ManageWorkflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Repository.ManageWorkFlow
{
    public interface IWorkflowHistoryRepository : IGenericRepository<WorkflowHistory>
    {
    }
}

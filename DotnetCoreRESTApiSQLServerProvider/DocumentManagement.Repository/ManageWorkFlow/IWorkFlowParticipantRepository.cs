using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Data.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Repository.ManageWorkFlow
{
    public interface IWorkFlowParticipantRepository : IGenericRepository<WorkFlowParticipant>
    {
        Task<WorkFlowParticipantList> GetAllWorkflowParticipant(WorkFlowResource workFlowResource);
    }
}

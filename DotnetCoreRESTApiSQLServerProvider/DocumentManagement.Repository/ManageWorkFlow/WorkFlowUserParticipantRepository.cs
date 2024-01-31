using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Domain;
namespace DocumentManagement.Repository.ManageWorkFlow
{
    public class WorkFlowUserParticipantRepository : GenericRepository<WorkflowUserParticipant, DocumentContext>,
     IWorkFlowUserParticipantRepository
    {
        public WorkFlowUserParticipantRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }
    }
}

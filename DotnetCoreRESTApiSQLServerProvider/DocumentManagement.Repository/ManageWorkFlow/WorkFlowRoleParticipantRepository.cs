using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Domain;
namespace DocumentManagement.Repository.ManageWorkFlow
{
    public class WorkFlowRoleParticipantRepository : GenericRepository<WorkFlowRoleParticipant, DocumentContext>,
    IWorkFlowRoleParticipantRepository
    {
        public WorkFlowRoleParticipantRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }
    }
}

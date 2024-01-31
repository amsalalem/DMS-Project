using DocumentManagement.Data.Resources;
using DocumentManagement.Repository.ManageWorkFlow;
using MediatR;

namespace DocumentManagement.MediatR.Queries.ManageWorkFlow
{
    public class GetAllWorkFlowQuery : IRequest<WorkFlowList>
    {
        public WorkFlowResource WorkFlowResource { get; set; }
    }
}

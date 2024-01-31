using DocumentManagement.MediatR.Queries.ManageWorkFlow;
using DocumentManagement.Repository.ManageWorkFlow;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
namespace DocumentManagement.MediatR.Handlers.ManageworkFlow.WorkFlows
{
    public class GetAllWorkFlowQueryHandler : IRequestHandler<GetAllWorkFlowQuery, WorkFlowList>
    {
        private readonly IWorkFlowRepository _documentRepository;
        public GetAllWorkFlowQueryHandler(
           IWorkFlowRepository documentRepository
            )
        {
            _documentRepository = documentRepository;

        }
        public async Task<WorkFlowList> Handle(GetAllWorkFlowQuery request, CancellationToken cancellationToken)
        {
            return await _documentRepository.GetWorkFlows(request.WorkFlowResource);
        }
    }
}

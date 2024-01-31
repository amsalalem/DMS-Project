using DocumentManagement.MediatR.Queries.ManageWorkFlow;
using DocumentManagement.Repository.ManageWorkFlow;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.ManageworkFlow.WorkFlows
{
    public class GetFileNeedMyApprovalLibraryQueryHandler : IRequestHandler<GetFileNeedMyApprovalLibraryQuery, WorkFlowList>
    {
        private readonly IWorkFlowRepository _workFlowRepository;
        public GetFileNeedMyApprovalLibraryQueryHandler(
           IWorkFlowRepository workFlowRepository
            )
        {
            _workFlowRepository = workFlowRepository;

        }
        public async Task<WorkFlowList> Handle(GetFileNeedMyApprovalLibraryQuery request, CancellationToken cancellationToken)
        {
            return await _workFlowRepository.GetFileNeedMyApprovalLibrary(request.Email, request.WorkFlowResource);
        }
    }
}

using DocumentManagement.MediatR.Queries.ManageWorkFlow;
using DocumentManagement.Repository.ManageWorkFlow;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.ManageworkFlow.WorkFlows
{
    public class GetMyApprovedFileLibraryQueryHandler : IRequestHandler<GetMyApprovedFileLibraryQuery, WorkFlowList>
    {
        private readonly IWorkFlowRepository _workFlowRepository;
        public GetMyApprovedFileLibraryQueryHandler(
           IWorkFlowRepository workFlowRepository
            )
        {
            _workFlowRepository = workFlowRepository;

        }
        public async Task<WorkFlowList> Handle(GetMyApprovedFileLibraryQuery request, CancellationToken cancellationToken)
        {
            return await _workFlowRepository.GetMyApprovedFileLibrary(request.Email, request.WorkFlowResource);
        }
    }
}

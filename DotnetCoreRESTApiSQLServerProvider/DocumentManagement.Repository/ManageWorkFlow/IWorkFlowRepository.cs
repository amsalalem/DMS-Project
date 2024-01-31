using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Data.Resources;
using System.Threading.Tasks;
using System;

namespace DocumentManagement.Repository.ManageWorkFlow
{
    public interface IWorkFlowRepository : IGenericRepository<Workflow>
    {
        Task<WorkFlowList> GetWorkFlows(WorkFlowResource documentResource);
        Task<WorkFlowList> GetFileNeedMyApprovalLibrary(string email, WorkFlowResource documentResource);
        Task<WorkFlowList> GetMyApprovedFileLibrary(string email, WorkFlowResource documentResource);
    }

}

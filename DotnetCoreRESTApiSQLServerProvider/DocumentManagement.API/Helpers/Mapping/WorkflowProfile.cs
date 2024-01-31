using AutoMapper;
using DocumentManagement.Data.Dto.ManageDocument;
using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.MediatR.Commands.ManageWorkFlow;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class WorkflowProfile : Profile
    {
        public WorkflowProfile()
        {
            CreateMap<Workflow, WorkFlowDto>().ReverseMap();
            CreateMap<WorkflowHistory, WorkflowHistoryDto>().ReverseMap();
            CreateMap<AddWorkFlowCommand, Workflow>().ReverseMap();
            CreateMap<ApproveWorkFlowCommand, WorkflowHistory>().ReverseMap();
            CreateMap<WorkFlowParticipant, WorkflowParticipantDto>().ReverseMap();
        }
    }
}

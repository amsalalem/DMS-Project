using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Data.Enums;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands.ManageWorkFlow;
using DocumentManagement.Repository;
using DocumentManagement.Repository.ManageWorkFlow;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.ManageworkFlow.WorkFlows
{
    public class ApproveWorkFlowCommandHandler : IRequestHandler<ApproveWorkFlowCommand, ServiceResponse<WorkflowHistoryDto>>
    {
        private readonly IWorkFlowRepository _workFlowRepository;
        private readonly IWorkFlowParticipantRepository _workFlowParticipantRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly IDocumentRepository _documentRepository;
        private readonly IWorkflowHistoryRepository _workflowHistoryRepository;
        public ApproveWorkFlowCommandHandler(IWorkFlowRepository workFlowRepository, IUnitOfWork<DocumentContext> uow, IMapper mapper, IWorkFlowParticipantRepository workFlowParticipantRepository, IDocumentRepository documentRepository, IWorkflowHistoryRepository workflowHistoryRepository)
        {
            _workFlowRepository = workFlowRepository;
            _uow = uow;
            _mapper = mapper;
            _workFlowParticipantRepository = workFlowParticipantRepository;
            _documentRepository = documentRepository;
            _workflowHistoryRepository = workflowHistoryRepository;
        }
        public async Task<ServiceResponse<WorkflowHistoryDto>> Handle(ApproveWorkFlowCommand request, CancellationToken cancellationToken)
        {
            Guid userId = Guid.Parse(request.Email);
            var workflowToUpdate = await _workFlowRepository.FindBy(c => c.Id == request.WorkflowId)
                 .Include(w => w.Document)
                 .FirstOrDefaultAsync();
            var documentToUpdate = workflowToUpdate?.Document;
            var getApprovalOrder = await _workFlowParticipantRepository.FindBy(x => x.WorkflowId == request.WorkflowId && x.OrderNumber == 1).FirstOrDefaultAsync();
            var getTobeRejcted = await _workFlowParticipantRepository.FindBy(x => x.WorkflowId == request.WorkflowId && x.OrderNumber > 1).ToListAsync();
            var entity = _mapper.Map<WorkflowHistory>(request);
            entity.ModifiedDate = DateTime.UtcNow;
            if (request.ApprovalStatus == ApprovalStatus.Rejected)
            {
                getApprovalOrder.CanViewToApprove = true;
                getApprovalOrder.ApprovalStatus = ApprovalStatus.Pending;
                foreach (var item in getTobeRejcted)
                {
                    item.CanViewToApprove = false;
                    item.ApprovalStatus = ApprovalStatus.Pending;
                    _workFlowParticipantRepository.Update(item);
                }

                _workFlowParticipantRepository.Update(getApprovalOrder);


            }
            else
            {
                var ParticipantListToUpdate = await _workFlowParticipantRepository.FindBy(x => x.WorkflowId == request.WorkflowId && x.CanViewToApprove == true).ToListAsync();
                foreach (var item in ParticipantListToUpdate)
                {
                    var ParticipantListNextToUpdate = await _workFlowParticipantRepository.FindBy(x => x.WorkflowId == item.WorkflowId && x.CanViewToApprove == false && x.OrderNumber == (item.OrderNumber + 1)).FirstOrDefaultAsync();
                    item.CanViewToApprove = true;
                    item.ApprovalStatus = request.ApprovalStatus;
                    if (ParticipantListNextToUpdate != null)
                    {
                        ParticipantListNextToUpdate.CanViewToApprove = true;
                        _workFlowParticipantRepository.Update(ParticipantListNextToUpdate);


                    }
                    if (ParticipantListNextToUpdate == null)
                    {
                        documentToUpdate.ApprovalStatus = request.ApprovalStatus;
                        workflowToUpdate.ApprovalStatus = request.ApprovalStatus;
                    }
                    _workFlowParticipantRepository.Update(item);
                    await _uow.SaveAsync();

                }
                _documentRepository.Update(documentToUpdate);
                _workFlowRepository.Update(workflowToUpdate);


            }

            _workflowHistoryRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<WorkflowHistoryDto>.ReturnFailed(500, "Error While Added Work Flow");
            }
            var entityDto = _mapper.Map<WorkflowHistoryDto>(entity);
            return ServiceResponse<WorkflowHistoryDto>.ReturnResultWith200(entityDto);
        }
       
    }
}

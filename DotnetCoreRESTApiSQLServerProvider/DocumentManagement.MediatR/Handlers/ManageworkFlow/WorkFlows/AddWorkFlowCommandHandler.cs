using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using DocumentManagement.MediatR.Commands.ManageWorkFlow;
using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Repository.ManageWorkFlow;
using Microsoft.EntityFrameworkCore;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Repository;

namespace DocumentManagement.MediatR.Handlers.ManageworkFlow.WorkFlows
{
    public class AddWorkFlowCommandHandler : IRequestHandler<AddWorkFlowCommand, ServiceResponse<WorkFlowDto>>
    {
        private readonly IWorkFlowRepository _workFlowRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfo;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDocumentRepository _documentRepository;
        public AddWorkFlowCommandHandler(
            IWorkFlowRepository workFlowRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow,
            UserInfoToken userInfo,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
,
            IDocumentRepository documentRepository)
        {
            _workFlowRepository = workFlowRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfo = userInfo;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
            _documentRepository = documentRepository;
        }

        public async Task<ServiceResponse<WorkFlowDto>> Handle(AddWorkFlowCommand request, CancellationToken cancellationToken)
        {
           

            var entityExist = await _workFlowRepository.FindBy(c => c.DocumentId == request.DocumentId).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return ServiceResponse<WorkFlowDto>.ReturnFailed(409, "WorkFlow already exist.");
            }
            var documentToUpdate = await _documentRepository.FindBy(v => v.Id == request.DocumentId).FirstOrDefaultAsync();
            var entity = _mapper.Map<Workflow>(request);
            entity.WorkFlowParticipant.ForEach(item =>
            {
                item.Workflow = null;
                if (item.OrderNumber == 1)
                {
                    item.CanViewToApprove = true;
                }
                item.ModifiedDate = DateTime.UtcNow;
            });
            entity.ModifiedDate = DateTime.UtcNow;
            documentToUpdate.ApprovalStatus = request.ApprovalStatus;
            _workFlowRepository.Add(entity);
            _documentRepository.Update(documentToUpdate);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<WorkFlowDto>.ReturnFailed(500, "Error While Added Work Flow");
            }
            var entityDto = _mapper.Map<WorkFlowDto>(entity);
            return ServiceResponse<WorkFlowDto>.ReturnResultWith200(entityDto);
        }
    }
}

using AutoMapper;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

using System;
using System.Linq;
using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Dto.ManageDocument;
using DocumentManagement.Data.Enums;

namespace DocumentManagement.Repository.ManageWorkFlow
{
    public class WorkFlowRepository : GenericRepository<Workflow, DocumentContext>,
          IWorkFlowRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        IUserRepository _userRepository;
        private readonly UserInfoToken _userInfoToken;
        public WorkFlowRepository(
            IUnitOfWork<DocumentContext> uow,
            IPropertyMappingService propertyMappingService,
            UserInfoToken userInfoToken,
            IUserRepository userRepository,
            IMapper mapper
            ) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _userInfoToken = userInfoToken;
            _userRepository = userRepository;
            _mapper = mapper; }

        public async Task<WorkFlowList> GetFileNeedMyApprovalLibrary(string email, WorkFlowResource documentResource)
        {
            var today = DateTime.UtcNow;
            var user = await _userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == _userInfoToken.Id);
            var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();
            var collectionBeforePaging = AllIncluding(c => c.User, c => c.Document, c => c.WorkFlowParticipant)
                .Where(c => !c.Document.IsDeleted)
                .Where(d => (d.WorkFlowParticipant.Any(c => c.UserId == user.Id && c.CanViewToApprove == true && c.ApprovalStatus == ApprovalStatus.Pending)));

            collectionBeforePaging = collectionBeforePaging
                .ApplySort(documentResource.OrderBy, _propertyMappingService.GetPropertyMapping<WorkFlowDto, Workflow>());

            if (!string.IsNullOrWhiteSpace(documentResource.Name))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.Document.Name, $"%{documentResource.Name}%") || EF.Functions.Like(c.Document.Description, $"%{documentResource.Name}%"));
            }

            var documentList = new WorkFlowList();
            return await documentList.CreateDocumentLibrary(
                collectionBeforePaging,
                documentResource.Skip,
                documentResource.PageSize
                );
        }

        public async Task<WorkFlowList> GetWorkFlows(WorkFlowResource workFlowResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.User, cs => cs.Document).Where(c => !c.Document.IsDeleted);
           
            collectionBeforePaging =
               collectionBeforePaging.ApplySort(workFlowResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<WorkFlowDto, Workflow>());

            if (!string.IsNullOrWhiteSpace(workFlowResource.Name))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.Document.Name, $"%{workFlowResource.Name}%") || EF.Functions.Like(c.Document.Description, $"%{workFlowResource.Name}%"));
            }


            if (!string.IsNullOrWhiteSpace(workFlowResource.CreateDateString))
            {
                workFlowResource.CreateDate = DateTime.ParseExact(workFlowResource.CreateDateString, "dd/MM/yyyy", null);
                var minDate = new DateTime(workFlowResource.CreateDate.Value.Year, workFlowResource.CreateDate.Value.Month, workFlowResource.CreateDate.Value.Day, 0, 0, 0);

                var maxDate = new DateTime(workFlowResource.CreateDate.Value.Year, workFlowResource.CreateDate.Value.Month, workFlowResource.CreateDate.Value.Day, 23, 59, 59);

                collectionBeforePaging = collectionBeforePaging
                    .Where(c => c.CreatedDate >= minDate && c.CreatedDate <= maxDate);
            }
            var documentList = new WorkFlowList();
            return await documentList.Create(
                collectionBeforePaging,
                workFlowResource.Skip,
                workFlowResource.PageSize
                );
        }

        public async Task<WorkFlowList> GetMyApprovedFileLibrary(string email, WorkFlowResource documentResource)
        {
            var today = DateTime.UtcNow;
            var user = await _userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == _userInfoToken.Id);
            var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();
            var collectionBeforePaging = AllIncluding(c => c.User, c => c.Document, c => c.WorkFlowParticipant)
                .Where(c => !c.Document.IsDeleted)
                .Where(d => (d.WorkFlowParticipant.Any(c => c.UserId == user.Id && c.CanViewToApprove == true && c.ApprovalStatus == ApprovalStatus.Approved)));

            collectionBeforePaging = collectionBeforePaging
                .ApplySort(documentResource.OrderBy, _propertyMappingService.GetPropertyMapping<WorkFlowDto, Workflow>());

            if (!string.IsNullOrWhiteSpace(documentResource.Name))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.Document.Name, $"%{documentResource.Name}%") || EF.Functions.Like(c.Document.Description, $"%{documentResource.Name}%"));
            }

            var documentList = new WorkFlowList();
            return await documentList.CreateDocumentLibrary(
                collectionBeforePaging,
                documentResource.Skip,
                documentResource.PageSize
                );
        }
    }
}

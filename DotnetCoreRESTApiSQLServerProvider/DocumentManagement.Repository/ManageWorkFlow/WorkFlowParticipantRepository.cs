using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto.ManageDocument;
using DocumentManagement.Data.Entities.ManageWorkflow;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Repository.ManageWorkFlow
{
    public class WorkFlowParticipantRepository
     : GenericRepository<WorkFlowParticipant, DocumentContext>, IWorkFlowParticipantRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public WorkFlowParticipantRepository(IUnitOfWork<DocumentContext> uow,
            IPropertyMappingService propertyMappingService)
          : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<WorkFlowParticipantList> GetAllWorkflowParticipant(WorkFlowResource workFlowResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.Workflow).ApplySort(workFlowResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<WorkflowParticipantDto, WorkFlowParticipant>());



            if (!string.IsNullOrWhiteSpace(workFlowResource.Name))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Workflow.Document.Name == workFlowResource.Name);
            }

            if (workFlowResource.DocumentId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.WorkflowId == workFlowResource.DocumentId);
            }

            if (workFlowResource.CreateDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Workflow.CreatedDate >= new DateTime(workFlowResource.CreateDate.Value.Year, workFlowResource.CreateDate.Value.Month, workFlowResource.CreateDate.Value.Day, 0, 0, 1));
            }
            if (workFlowResource.DocumentId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Workflow.CreatedDate <= new DateTime(workFlowResource.CreateDate.Value.Year, workFlowResource.CreateDate.Value.Month, workFlowResource.CreateDate.Value.Day, 23, 59, 59));
            }
            var salesOrderItems = new WorkFlowParticipantList();
            return await salesOrderItems
                .Create(collectionBeforePaging, workFlowResource.Skip, workFlowResource.PageSize);
        }
    }
}

using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Entities.ManageWorkflow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Repository.ManageWorkFlow
{
    public class WorkFlowList : List<WorkFlowDto>
    {
        public WorkFlowList()
        {
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public WorkFlowList(List<WorkFlowDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<WorkFlowList> Create(IQueryable<Workflow> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new WorkFlowList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<WorkFlowList> CreateDocumentLibrary(IQueryable<Workflow> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDocumentLibraryDtos(source, skip, pageSize);
            var dtoPageList = new WorkFlowList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Workflow> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<WorkFlowDto>> GetDtos(IQueryable<Workflow> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new WorkFlowDto
                {
                    Id = c.Id,
                    DocumentId = c.DocumentId,
                    DocumentName = c.Document.Name,
                    Url = c.Document.Url,
                    WorkflowMethod = c.WorkflowMethod,
                    WorkFlowType = c.WorkFlowType,
                    ApprovalStatus = c.ApprovalStatus,
                    CreatedDate = c.CreatedDate,
                    //CategoryId = c.Document.CategoryId,
                    CategoryName = c.Document.Category.Name,
                    CreatedBy = c.User != null ? $"{c.User.FirstName} {c.User.LastName}" : "",

                })

                .ToListAsync();


            return entities;
        }

        public async Task<List<WorkFlowDto>> GetDocumentLibraryDtos(IQueryable<Workflow> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new WorkFlowDto
                {
                    Id = c.Id,
                    DocumentId = c.DocumentId,
                    DocumentName = c.Document.Name,
                    Url = c.Document.Url,
                    WorkflowMethod = c.WorkflowMethod,
                    WorkFlowType = c.WorkFlowType,
                    ApprovalStatus = c.ApprovalStatus,
                    CreatedDate = c.CreatedDate,
                    //CategoryId = c.Document.CategoryId,
                    CategoryName = c.Document.Category.Name,
                    CreatedBy = c.User != null ? $"{c.User.FirstName} {c.User.LastName}" : "",
                  
                })
                .ToListAsync();
            return entities;
        }
    }
}

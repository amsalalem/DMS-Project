using DocumentManagement.Data.Dto.ManageDocument;
using DocumentManagement.Data.Entities.ManageWorkflow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Repository.ManageWorkFlow
{
    public class WorkFlowParticipantList : List<WorkflowParticipantDto>
    {
        public WorkFlowParticipantList()
        {
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public WorkFlowParticipantList(List<WorkflowParticipantDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<WorkFlowParticipantList> Create(IQueryable<WorkFlowParticipant> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new WorkFlowParticipantList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<WorkFlowParticipant> source)
        {
            try
            {
                return await source.AsNoTracking().CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<WorkflowParticipantDto>> GetDtos(IQueryable<WorkFlowParticipant> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
                             .AsNoTracking()
                             .Select(c => new WorkflowParticipantDto
                             {
                                 AproverName = c.User.FirstName + " " + c.User.LastName,
                                 WorkflowId = c.WorkflowId,
                                 Id = c.Id,
                                 UserId = c.UserId,
                                 ApprovalStatus = c.ApprovalStatus
                             }).ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
                             .Skip(skip)
                             .Take(pageSize)
                             .AsNoTracking()
                             .Select(c => new WorkflowParticipantDto
                             {
                                 AproverName = c.User.FirstName + " " + c.User.LastName,
                                 WorkflowId = c.WorkflowId,
                                 Id = c.Id,
                                 UserId = c.UserId,
                                 ApprovalStatus = c.ApprovalStatus
                             }) .ToListAsync();
                return entities;
            }

        }
    }
}

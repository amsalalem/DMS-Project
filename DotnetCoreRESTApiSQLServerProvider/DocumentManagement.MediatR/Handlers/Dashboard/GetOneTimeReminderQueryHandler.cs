using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetOneTimeReminderQueryHandler
       : IRequestHandler<GetOneTimeReminderQuery, List<CalenderReminderDto>>
    {

        private readonly IReminderRepository _reminderRepository;

        public GetOneTimeReminderQueryHandler(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public async Task<List<CalenderReminderDto>> Handle(GetOneTimeReminderQuery request, CancellationToken cancellationToken)
        {
            var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 1);
            var reminders = await _reminderRepository.All
                 .Include(c => c.ReminderUsers)
                 .Where(c => c.Frequency == Frequency.OneTime
                    && c.StartDate.Month == request.Month)
                 .ToListAsync();
            var reminderDto = reminders.Select(c => new CalenderReminderDto
            {
                Title = c.Subject,
                Start = new DateTime(startDate.Year, startDate.Month, c.StartDate.Day),
                End = new DateTime(startDate.Year, startDate.Month, c.StartDate.Day),
            }).ToList();

            return reminderDto;
        }
    }
}

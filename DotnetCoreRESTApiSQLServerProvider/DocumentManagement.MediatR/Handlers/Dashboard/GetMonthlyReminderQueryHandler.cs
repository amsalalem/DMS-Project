﻿using DocumentManagement.Data;
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
    public class GetMonthlyReminderQueryHandler
        : IRequestHandler<GetMonthlyReminderQuery, List<CalenderReminderDto>>
    {
        private readonly IReminderRepository _reminderRepository;

        public GetMonthlyReminderQueryHandler(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public async Task<List<CalenderReminderDto>> Handle(GetMonthlyReminderQuery request, CancellationToken cancellationToken)
        {
            var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 1);
            var monthEndDate = startDate.AddMonths(1).AddDays(-1);
            var endDate = new DateTime(monthEndDate.Year, monthEndDate.Month, monthEndDate.Day, 23, 59, 59);
            var lastDayOfMonth = endDate.Day;
            var reminders = await _reminderRepository.All
                 .Include(c => c.ReminderUsers)
                 .Where(c => c.Frequency == Frequency.Monthly
                    && c.StartDate <= endDate && (!c.EndDate.HasValue || c.EndDate >= startDate))
                 .ToListAsync();
            var reminderDto = reminders.Select(c => new CalenderReminderDto
            {
                Title = c.Subject,
                Start = new DateTime(startDate.Year, startDate.Month, c.StartDate.Day > lastDayOfMonth ? lastDayOfMonth : c.StartDate.Day, startDate.Hour, startDate.Minute, startDate.Hour),
                End = new DateTime(startDate.Year, startDate.Month, c.StartDate.Day > lastDayOfMonth ? lastDayOfMonth : c.StartDate.Day, startDate.Hour, startDate.Minute, startDate.Hour),
            }).ToList();

            return reminderDto;
        }
    }
}

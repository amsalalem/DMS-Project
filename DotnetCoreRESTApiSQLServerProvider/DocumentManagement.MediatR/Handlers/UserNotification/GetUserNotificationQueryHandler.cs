using AutoMapper;
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

namespace DocumentManagement.MediatR.Handlers.UserNotification
{
    public class GetUserNotificationQueryHandler : IRequestHandler<GetUserNotificationQuery, List<UserNotificationDto>>
    {
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMapper _mapper;

        public GetUserNotificationQueryHandler(
           IUserNotificationRepository userNotificationRepository,
           UserInfoToken userInfoToken,
            IMapper mapper)
        {
            _userNotificationRepository = userNotificationRepository;
            _mapper = mapper;
            _userInfoToken = userInfoToken;
        }

        public async Task<List<UserNotificationDto>> Handle(GetUserNotificationQuery request, CancellationToken cancellationToken)
        {
            var userId = _userInfoToken.Id;
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToUniversalTime();
            var fromDate = today.AddDays(-1).AddSeconds(1);
            var toDate = today.AddDays(1).AddSeconds(-1);
            var entities = await _userNotificationRepository
                .AllIncluding(c => c.Document)
                .Where(c => c.UserId == userId
                && !c.Document.IsDeleted
                && (!c.IsRead || (c.CreatedDate > fromDate && c.CreatedDate < toDate)))
                .OrderByDescending(c => c.CreatedDate)
                .Take(10).ToListAsync();
            return _mapper.Map<List<UserNotificationDto>>(entities);
        }
    }
}

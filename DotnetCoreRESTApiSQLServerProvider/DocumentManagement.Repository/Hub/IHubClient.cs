﻿using DocumentManagement.Data.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface IHubClient
    {
        Task UserLeft(string id);

        Task NewOnlineUser(SignlarUser userInfo);

        Task Joined(SignlarUser userInfo);

        Task OnlineUsers(IEnumerable<SignlarUser> userInfo);

        Task Logout(SignlarUser userInfo);

        Task ForceLogout(SignlarUser userInfo);

        Task SendDM(string message, SignlarUser userInfo);

        Task SendNotification(Guid userId);


    }
}

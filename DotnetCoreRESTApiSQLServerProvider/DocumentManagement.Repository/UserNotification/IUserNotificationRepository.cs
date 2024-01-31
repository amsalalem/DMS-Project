using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface IUserNotificationRepository : IGenericRepository<UserNotification>
    {
        void CreateUsersDocumentNotifiction(List<Guid> userIds, Guid documentId);
        Task<List<Guid>> CreateRolesDocumentNotifiction(List<Guid> roleIds, Guid documentId);
        Task<NotificationList> GetUserNotifications(NotificationResource documentResource);
        Task MarkAsRead(Guid notificationId);
        Task MarkAsReadByDocumentId(Guid documentId);
        Task MarkAllAsRead();
        void AddUserNotificationByReminderScheduler(ReminderScheduler reminderScheduler);
        Task SendNotification(Guid userId);
        Task SendNotification(List<Guid> userIds);
    }
}

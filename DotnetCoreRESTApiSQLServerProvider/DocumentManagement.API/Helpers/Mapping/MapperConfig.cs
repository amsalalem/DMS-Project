using AutoMapper;

namespace DocumentManagement.API.Helpers.Mapping
{
    public static class MapperConfig
    {
        public static IMapper GetMapperConfigs()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new OperationProfile());
                mc.AddProfile(new ScreenProfile());
                mc.AddProfile(new RoleProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new ScreenOperationProfile());
                mc.AddProfile(new CategoryProfile());
                mc.AddProfile(new DocumentProfile());
                mc.AddProfile(new DocumentPermission());
                mc.AddProfile(new DocumentAuditTrailProfile());
                mc.AddProfile(new UserNotificationProfile());
                mc.AddProfile(new ReminderProfile());
                mc.AddProfile(new EmailSMTPSettingProfile());
                mc.AddProfile(new WorkflowProfile());
            });
            return mappingConfig.CreateMapper();
        }
    }
}

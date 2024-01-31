using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Dto.ManageWorkFlow;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Entities.ManageWorkflow;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentManagement.Repository
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _documentPropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
               { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
               { "Name", new PropertyMappingValue(new List<string>() { "Name" } )},
               { "Description", new PropertyMappingValue(new List<string>() { "Description" } )},
               { "CreatedBy", new PropertyMappingValue(new List<string>() { "User.FirstName" } )},
               { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" } )},
               { "CategoryName", new PropertyMappingValue(new List<string>() { "Category.Name" } )},
               { "ApprovalStatus", new PropertyMappingValue(new List<string>() { "ApprovalStatus" } )},
           }; 
        private Dictionary<string, PropertyMappingValue> _workFlowPropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
               { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
               { "DocumentName", new PropertyMappingValue(new List<string>() { "Document.Name" } )},
               { "Url", new PropertyMappingValue(new List<string>() { "Document.Url" } )},
               { "DocumentId", new PropertyMappingValue(new List<string>() { "DocumentId" } )},
               { "WorkflowMethod", new PropertyMappingValue(new List<string>() { "WorkflowMethod" } )},
               { "WorkFlowType", new PropertyMappingValue(new List<string>() { "WorkFlowType" } )},
               { "ApprovalStatus", new PropertyMappingValue(new List<string>() { "ApprovalStatus" } )},
               { "CreatedBy", new PropertyMappingValue(new List<string>() { "User.FirstName" } )},
               { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" } )},
               { "CategoryName", new PropertyMappingValue(new List<string>() { "Document.Category.Name" } )}
           };

        private Dictionary<string, PropertyMappingValue> _documentAuditTrailPropertyMapping =
          new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
          {
               { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
               { "Name", new PropertyMappingValue(new List<string>() { "Document.Name" } )},
               { "DocumentName", new PropertyMappingValue(new List<string>() { "Document.Name" } )},
               { "DocumentId", new PropertyMappingValue(new List<string>() { "DocumentId" } )},
               { "CategoryName", new PropertyMappingValue(new List<string>() { "Document.Category.Name" } )},
               { "CreatedBy", new PropertyMappingValue(new List<string>() { "CreatedByUser.FirstName" } )},
               { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" } )},
               { "OperationName", new PropertyMappingValue(new List<string>() { "OperationName" } )},
               { "PermissionUser", new PropertyMappingValue(new List<string>() { "AssignToUser.FirstName" } )},
               { "PermissionRole", new PropertyMappingValue(new List<string>() { "AssignToRole.Name" } )}
          };

        private Dictionary<string, PropertyMappingValue> _notificationPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
                { "UserId", new PropertyMappingValue(new List<string>() { "UserId" } ) },
                { "Message", new PropertyMappingValue(new List<string>() { "Message" } ) },
                { "DocumentId", new PropertyMappingValue(new List<string>() { "DocumentId" } ) },
                { "DocumentName", new PropertyMappingValue(new List<string>() { "Document.Name" } )},
                { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" } )},
                { "IsRead", new PropertyMappingValue(new List<string>() { "IsRead" } )}
            };

        private Dictionary<string, PropertyMappingValue> _loginAuditMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
                { "UserName", new PropertyMappingValue(new List<string>() { "UserName" } )},
                { "LoginTime", new PropertyMappingValue(new List<string>() { "LoginTime" } )},
                { "RemoteIP", new PropertyMappingValue(new List<string>() { "RemoteIP" } )},
                { "Status", new PropertyMappingValue(new List<string>() { "Status" } )},
                { "Provider", new PropertyMappingValue(new List<string>() { "Provider" } )}
            };

        private Dictionary<string, PropertyMappingValue> _reminderMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
                { "Subject", new PropertyMappingValue(new List<string>() { "Subject" } )},
                { "Message", new PropertyMappingValue(new List<string>() { "Message" } )},
                { "Frequency", new PropertyMappingValue(new List<string>() { "Frequency" } )},
                { "DocumentName", new PropertyMappingValue(new List<string>() { "Document.Name" } )},
                { "StartDate", new PropertyMappingValue(new List<string>() { "StartDate" },true )},
                { "EndDate", new PropertyMappingValue(new List<string>() { "EndDate" },true )},
                { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" } )},
                { "IsRepeated", new PropertyMappingValue(new List<string>() { "IsRepeated" } )},
                { "IsEmailNotification", new PropertyMappingValue(new List<string>() { "IsEmailNotification" } )},
                { "IsActive", new PropertyMappingValue(new List<string>() { "IsActive" } )}
            };

        private Dictionary<string, PropertyMappingValue> _reminderSchedulerMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
                { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
                { "Subject", new PropertyMappingValue(new List<string>() { "Subject" } )},
                { "Message", new PropertyMappingValue(new List<string>() { "Message" } )},
                { "IsRead", new PropertyMappingValue(new List<string>() { "IsRead" } )},
                { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" }, true )}
           };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<LoginAuditDto, LoginAudit>(_loginAuditMapping));
            propertyMappings.Add(new PropertyMapping<DocumentDto, Document>(_documentPropertyMapping));
            propertyMappings.Add(new PropertyMapping<WorkFlowDto, Workflow>(_workFlowPropertyMapping));
            propertyMappings.Add(new PropertyMapping<DocumentAuditTrailDto, DocumentAuditTrail>(_documentAuditTrailPropertyMapping));
            propertyMappings.Add(new PropertyMapping<UserNotificationDto, UserNotification>(_notificationPropertyMapping));
            propertyMappings.Add(new PropertyMapping<ReminderDto, Reminder>(_reminderMapping));
            propertyMappings.Add(new PropertyMapping<ReminderSchedulerDto, ReminderScheduler>(_reminderSchedulerMapping));

        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;

        }

    }
}

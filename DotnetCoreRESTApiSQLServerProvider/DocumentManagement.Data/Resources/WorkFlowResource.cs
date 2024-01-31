using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Resources
{
    public class WorkFlowResource : ResourceParameter
    {
        public WorkFlowResource() : base("DocumentName")
        {
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public Guid? DocumentId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateDateString { get; set; }
        public string CreatedBy { get; set; }
    }
}

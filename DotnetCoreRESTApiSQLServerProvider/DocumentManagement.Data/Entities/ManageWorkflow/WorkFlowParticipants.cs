using DocumentManagement.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Entities.ManageWorkflow
{
    public class WorkFlowParticipants: BaseEntity
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }
        public WorkFlowType WorkFlowType { get; set; }
        public string Status { get; set; }
        public int Order { get; set; }
        public string ParticipantId { get; set; }
        public ParticipantType ParticipantType { get; set; }

    }
}

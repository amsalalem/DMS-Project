using DocumentManagement.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Dto.ManageWorkFlow
{
    public class WorkFlowParticipantsDto: ErrorStatusCode
    {
        public Guid DocumentId { get; set; }
        public string CreatedByUser { get; set; }
        public int WorkFlowType { get; set; }
        public int Order { get; set; }
        public List<string> ParticipantId { get; set; }
        public int ParticipantType { get; set; }
    }
}

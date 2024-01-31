using DocumentManagement.Data.Dto.ManageDocument;
using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands.ManageWorkFlow
{
    internal class AddWorkflowParticipantCommand : IRequest<ServiceResponse<WorkflowParticipantDto>>
    {
    }
}

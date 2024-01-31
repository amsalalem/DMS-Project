using MediatR;
using System.Collections.Generic;
namespace DocumentManagement.MediatR.Commands.ManageWorkFlow
{
    public class WorkFlowUserRoleCommand : IRequest<bool>
    {
        public List<string> Roles { get; set; }
        public List<string> Users { get; set; }
        public List<string> WorkFlows { get; set; }
    }
}

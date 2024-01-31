using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands
{
    public class RestoreDocumentVersionCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid DocumentId { get; set; }
        public Guid Id { get; set; }
    }
}

using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands
{
    public class GetDocumentVersionCommand : IRequest<List<DocumentVersionDto>>
    {
        public Guid Id { get; set; }
    }
}

using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands
{
    public class GetDocumentMetaDataByIdQuery : IRequest<List<DocumentMetaDataDto>>
    {
        public Guid DocumentId { get; set; }
    }
}

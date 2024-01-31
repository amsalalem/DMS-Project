using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DownloadDocumentCommand : IRequest<string>
    {
        public Guid Id { get; set; }
        public bool IsVersion { get; set; }
    }
}

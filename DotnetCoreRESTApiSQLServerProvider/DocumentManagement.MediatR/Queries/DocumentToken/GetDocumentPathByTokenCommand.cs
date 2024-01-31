using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class GetDocumentPathByTokenCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid Token { get; set; }
    }
}

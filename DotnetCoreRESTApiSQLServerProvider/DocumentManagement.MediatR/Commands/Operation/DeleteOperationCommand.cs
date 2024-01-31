using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteOperationCommand : IRequest<OperationDto>
    {
        public Guid Id { get; set; }
    }
}

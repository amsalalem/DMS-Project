using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class AddOperationCommand: IRequest<OperationDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

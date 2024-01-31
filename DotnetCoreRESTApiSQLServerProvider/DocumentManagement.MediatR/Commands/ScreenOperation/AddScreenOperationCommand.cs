using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class AddScreenOperationCommand: IRequest<ScreenOperationDto>
    {
        public Guid ScreenId { get; set; }
        public Guid OperationId { get; set; }
        public bool Flag { get; set; }
    }
}

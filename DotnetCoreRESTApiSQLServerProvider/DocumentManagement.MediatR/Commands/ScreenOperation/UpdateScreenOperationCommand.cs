using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateScreenOperationCommand: IRequest<ScreenOperationDto>
    {
        public Guid Id { get; set; }
        public Guid ScreenId { get; set; }
        public Guid OperationId { get; set; }
        public bool Flag { get; set; }
    }
}

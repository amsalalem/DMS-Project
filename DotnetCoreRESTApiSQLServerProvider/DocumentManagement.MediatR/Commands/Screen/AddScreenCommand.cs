using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class AddScreenCommand : IRequest<ScreenDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetScreenQuery : IRequest<ScreenDto>
    {
        public Guid Id { get; set; }
    }
}

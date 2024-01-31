﻿using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }
}

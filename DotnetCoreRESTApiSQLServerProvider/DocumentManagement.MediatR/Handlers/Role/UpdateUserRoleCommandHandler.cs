using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, UserRoleDto>
    {
        IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public UpdateUserRoleCommandHandler(IUserRoleRepository userRoleRepository,
            IUnitOfWork<DocumentContext> uow,
            IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<UserRoleDto> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var userRoles = await _userRoleRepository.All.Where(c => c.RoleId == request.Id).ToListAsync();
            var userRolesToAdd = request.UserRoles.Where(c => !userRoles.Select(c => c.UserId).Contains(c.UserId.Value)).ToList();
            _userRoleRepository.AddRange(_mapper.Map<List<UserRole>>(userRolesToAdd));
            var userRolesToDelete = userRoles.Where(c => !request.UserRoles.Select(cs => cs.UserId).Contains(c.UserId)).ToList();
            _userRoleRepository.RemoveRange(userRolesToDelete);

            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new UserRoleDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            return new UserRoleDto();
        }
    }

}

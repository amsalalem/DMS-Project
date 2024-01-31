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
    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public AddRoleCommandHandler(
           IRoleRepository roleRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<RoleDto> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _roleRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                var errorDto = new RoleDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "Role Name already exist." }
                };
                return errorDto;
            }
            var entity = _mapper.Map<Role>(request);
            entity.Id = Guid.NewGuid();
            entity.NormalizedName = entity.Name;
            entity.RoleClaims.ToList().ForEach(claim => claim.ClaimType = claim.ClaimType.Replace(" ", "_"));
            _roleRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new RoleDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            var entityDto = _mapper.Map<RoleDto>(entity);
            return entityDto;
        }
    }
}

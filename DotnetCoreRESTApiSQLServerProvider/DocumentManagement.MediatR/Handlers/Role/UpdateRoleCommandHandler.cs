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
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleClaimRepository _roleClaimRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public UpdateRoleCommandHandler(
           IRoleRepository roleRepository,
           IRoleClaimRepository roleClaimRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _roleRepository = roleRepository;
            _roleClaimRepository = roleClaimRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _roleRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id)
                 .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                var errorDto = new RoleDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "Role Name Already Exist." }
                };
                return errorDto;
            }

            // Update Role
            var entity = _mapper.Map<Role>(request);
            entityExist = await _roleRepository.FindByInclude(v => v.Id == request.Id, c => c.RoleClaims).FirstOrDefaultAsync();
            entityExist.Name = entity.Name;
            entityExist.NormalizedName = entity.Name;
            _roleRepository.Update(entityExist);

            // update Role Claim
            var roleClaims = entityExist.RoleClaims.ToList();
            var roleClaimsToAdd = request.RoleClaims.Where(c => !roleClaims.Select(c => c.Id).Contains(c.Id)).ToList();
            roleClaimsToAdd.ForEach(claim => claim.ClaimType = claim.ClaimType.Replace(" ", "_"));
            _roleClaimRepository.AddRange(_mapper.Map<List<RoleClaim>>(roleClaimsToAdd));
            var roleClaimsToDelete = roleClaims.Where(c => !request.RoleClaims.Select(cs => cs.Id).Contains(c.Id)).ToList();
            _roleClaimRepository.RemoveRange(roleClaimsToDelete);

            // TODO: update user Role
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

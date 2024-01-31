using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public AddUserCommandHandler(
            IMapper mapper,
            UserManager<User> userManager
            )
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<UserDto> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.FindByNameAsync(request.Email);
            if (appUser != null)
            {
                var errorDto = new UserDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "Email already exist for another user." }
                };
                return errorDto;
            }
            var entity = _mapper.Map<User>(request);
            entity.Id = Guid.NewGuid();
            IdentityResult result = await _userManager.CreateAsync(entity);
            if (!result.Succeeded)
            {
                var errorDto = new UserDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            if (!string.IsNullOrEmpty(request.Password))
            {

                string code = await _userManager.GeneratePasswordResetTokenAsync(entity);
                IdentityResult passwordResult = await _userManager.ResetPasswordAsync(entity, code, request.Password);
                if (!passwordResult.Succeeded)
                {
                    var errorDto = new UserDto
                    {
                        StatusCode = 500,
                        Messages = new List<string> { "An unexpected fault happened. Try again later." }
                    };
                    return errorDto;
                }
            }

            return _mapper.Map<UserDto>(entity);
        }
    }
}

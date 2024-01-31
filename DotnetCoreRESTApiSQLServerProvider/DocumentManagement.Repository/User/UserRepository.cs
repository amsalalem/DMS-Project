using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class UserRepository : GenericRepository<User, DocumentContext>,
          IUserRepository
    {
        private JwtSettings _settings = null;
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IRoleClaimRepository _roleClaimRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IScreenOperationRepository _screenOperationRepository;
        public UserRepository(
            IUnitOfWork<DocumentContext> uow,
             JwtSettings settings,
             IUserClaimRepository userClaimRepository,
             IRoleClaimRepository roleClaimRepository,
             IUserRoleRepository userRoleRepository,
             IScreenOperationRepository screenOperationRepository
            ) : base(uow)
        {
            _roleClaimRepository = roleClaimRepository;
            _userClaimRepository = userClaimRepository;
            _userRoleRepository = userRoleRepository;
            _settings = settings;
            _screenOperationRepository = screenOperationRepository;
        }

        private async Task<List<AppClaimDto>> GetUserAndRoleClaims(User appUser)
        {
            var userClaims = await _userClaimRepository.FindBy(c => c.UserId == appUser.Id).ToListAsync();
            var roleClaims = await GetRoleClaims(appUser);
            var screenOperations = await _screenOperationRepository.AllIncluding(c => c.Screen, c => c.Operation).ToListAsync();
            List<AppClaimDto> lstAppClaimDto = new List<AppClaimDto>();
            foreach (var screenOperation in screenOperations)
            {
                var appClaimDto = new AppClaimDto
                {
                    ClaimType = $"{screenOperation.Screen.Name.Replace(" ", "_").ToLower()}_{screenOperation.Operation.Name.Replace(" ", "_").ToLower()}",
                    ClaimValue = "false"
                };
                if (userClaims.Any(c => c.ScreenId == screenOperation.ScreenId && c.OperationId == screenOperation.OperationId))
                {
                    appClaimDto.ClaimValue = "true";
                }
                if (roleClaims.Any(c => c.ScreenId == screenOperation.ScreenId && c.OperationId == screenOperation.OperationId))
                {
                    appClaimDto.ClaimValue = "true";
                }
                lstAppClaimDto.Add(appClaimDto);
            }
            return lstAppClaimDto;
        }

        private async Task<List<RoleClaim>> GetRoleClaims(User appUser)
        {
            var rolesIds = await _userRoleRepository.All.Where(c => c.UserId == appUser.Id)
                .Select(c => c.RoleId)
                .ToListAsync();
            List<RoleClaim> lstRoleClaim = new List<RoleClaim>();
            foreach (var roleId in rolesIds)
            {
                var roleClaims = await _roleClaimRepository.FindBy(c => c.RoleId == roleId).ToListAsync();
                foreach (var roleClaim in roleClaims)
                {
                    if (!lstRoleClaim.Any(c => c.OperationId == roleClaim.OperationId && c.ScreenId == roleClaim.ScreenId))
                    {
                        lstRoleClaim.Add(roleClaim);
                    }
                }
            }
            return lstRoleClaim;
        }

        public async Task<UserAuthDto> BuildUserAuthObject(User appUser)
        {
            UserAuthDto ret = new UserAuthDto();
            List<AppClaimDto> appClaims = new List<AppClaimDto>();
            // Set User Properties
            ret.Id = appUser.Id.ToString();
            ret.UserName = appUser.UserName;
            ret.FirstName = appUser.FirstName;
            ret.LastName = appUser.LastName;
            ret.Email = appUser.Email;
            ret.PhoneNumber = appUser.PhoneNumber;
            ret.IsAuthenticated = true;
            // Get all claims for this user
            var appClaimDtos = await this.GetUserAndRoleClaims(appUser);
            ret.Claims = appClaimDtos;
            var claims = appClaimDtos.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            // Set JWT bearer token
            ret.BearerToken = BuildJwtToken(ret, claims, appUser.Id);
            return ret;
        }
        protected string BuildJwtToken(UserAuthDto authUser, IList<Claim> claims, Guid Id)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(_settings.Key));
            claims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub.ToString(), Id.ToString()));
            claims.Add(new Claim("Email", authUser.Email));
            // Create the JwtSecurityToken object
            var token = new JwtSecurityToken(
              issuer: _settings.Issuer,
              audience: _settings.Audience,
              claims: claims,
              notBefore: DateTime.UtcNow,
              expires: DateTime.UtcNow.AddMinutes(
                  _settings.MinutesToExpiration),
              signingCredentials: new SigningCredentials(key,
                          SecurityAlgorithms.HmacSha256)
            );
            // Create a string representation of the Jwt token
            return new JwtSecurityTokenHandler().WriteToken(token); ;
        }
    }
}

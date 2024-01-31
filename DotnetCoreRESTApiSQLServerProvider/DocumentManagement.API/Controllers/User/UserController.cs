using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers
{
    /// <summary>
    /// User
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        public IMediator _mediator { get; set; }
        public readonly UserInfoToken _userInfo;
        /// <summary>
        /// User
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="userInfo"></param>
        public UserController(
            IMediator mediator,
            UserInfoToken userInfo)
        {
            _mediator = mediator;
            _userInfo = userInfo;
        }
        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetUsers")]
        [Produces("application/json", "application/xml", Type = typeof(List<UserDto>))]
        public async Task<IActionResult> GetUsers()
        {
            var getAllUserQuery = new GetAllUserQuery
            {
            };
            var result = await _mediator.Send(getAllUserQuery);
            return Ok(result);
        }
        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUser")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var getUserQuery = new GetUserQuery
            {
                Id = id
            };
            var result = await _mediator.Send(getUserQuery);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        ///  Create a User
        /// </summary>
        /// <param name="addUserCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> AddUser(AddUserCommand addUserCommand)
        {
            var result = await _mediator.Send(addUserCommand);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }
            return CreatedAtAction("GetUser", new { id = result.Id }, result);
        }
        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="userLoginCommand"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [Produces("application/json", "application/xml", Type = typeof(UserAuthDto))]
        public async Task<IActionResult> UserLogin(UserLoginCommand userLoginCommand)
        {
            var result = await _mediator.Send(userLoginCommand);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Update User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateUserCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserCommand updateUserCommand)
        {
            updateUserCommand.Id = id;
            var result = await _mediator.Send(updateUserCommand);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }
            return NoContent();
        }
        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateUserProfileCommand"></param>
        /// <returns></returns>
        [HttpPut("profile")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileCommand updateUserProfileCommand)
        {
            var result = await _mediator.Send(updateUserProfileCommand);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Delete User By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            var deleteScreenCommand = new DeleteUserCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(deleteScreenCommand);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// User Change Password
        /// </summary>
        /// <param name="resetPasswordCommand"></param>
        /// <returns></returns>
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand resetPasswordCommand)
        {
            var result = await _mediator.Send(resetPasswordCommand);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Reset Resetpassword
        /// </summary>
        /// <param name="newPasswordCommand"></param>
        /// <returns></returns>
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand newPasswordCommand)
        {
            var result = await _mediator.Send(newPasswordCommand);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                return Unauthorized();
            }
            var getUserQuery = new GetUserQuery
            {
                Id = _userInfo.Id
            };
            var result = await _mediator.Send(getUserQuery);
            return StatusCode(result.StatusCode, result);
        }

    }
}

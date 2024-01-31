using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUnitOfWork<DocumentContext> _uow;
        public AuthenticationController(IUnitOfWork<DocumentContext> uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="userLoginCommand"></param>
        /// <returns></returns>
        [HttpGet("migration")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Migration()
        {
            if (_uow.Migration())
            {
                return Ok("Migration done");
            }
            return Ok("Migration Fail");
        }
    }
}

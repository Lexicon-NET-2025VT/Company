using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Companies.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public AuthController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser(UserForRegistrationDto registrationDto)
        {
            var result = await serviceManager.AuthService.RegisterUserAsync(registrationDto);
            return result.Succeeded ? StatusCode(StatusCodes.Status201Created) : BadRequest(result.Errors);
        }
    }
}

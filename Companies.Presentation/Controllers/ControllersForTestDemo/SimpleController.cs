using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Companies.API.DTOs;
using Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Domain.Models.Entities;
using Companies.Shared.Request;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Companies.Presentation.Controllers
{
    [Route("api/simple")]
    [ApiController]
    public class SimpleController : ControllerBase
    {
        public SimpleController()
        {
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany()
        {
            // return Ok("Hej från controllern");

            if(User?.Identity?.IsAuthenticated ?? false)
            {
                return Ok("Is Auth");
            }
            else
            {
                return BadRequest("Is not auth");
            }

        }
    }
}

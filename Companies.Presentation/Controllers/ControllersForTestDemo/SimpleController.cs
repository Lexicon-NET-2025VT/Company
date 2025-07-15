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
using Companies.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Companies.Presentation.Controllers
{
    [Route("api/simple")]
    [ApiController]
    public class SimpleController : ControllerBase
    {
        private readonly CompaniesContext db;
        private readonly IMapper mapper;

        public SimpleController(CompaniesContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
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

        [HttpGet("uniqueroute")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany2()
        {
            var companies = await db.Companies.ToListAsync();
            var compDtos = mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(compDtos);
        }
    }
}

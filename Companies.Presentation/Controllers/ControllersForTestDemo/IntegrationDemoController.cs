using AutoMapper;
using Companies.API.DTOs;
using Companies.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Presentation.Controllers.ControllersForTestDemo
{
    [Route("api/demo")]
    [ApiController]
    public class IntegrationDemoController : ControllerBase
    {
        private readonly CompaniesContext db;
        private readonly IMapper mapper;

        public IntegrationDemoController(CompaniesContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult Index() 
        {
            return Ok("Working");
        }

        [HttpGet("dto")]
        public ActionResult Index2()
        {
            var dto = new CompanyDto { Name = "Working AB" };
            return Ok(dto);
        }

        [HttpGet("getone")]
        public async Task<ActionResult> Get()
        {
            var companies = (await db.Companies.ToListAsync()).First();

            var compDtos = mapper.Map<CompanyDto>(companies);

            return Ok(compDtos);
        }

        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {
            var companies = (await db.Companies.ToListAsync());
            var compDtos = mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(compDtos);
        }
    }
}

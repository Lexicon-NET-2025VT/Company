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
using Domain.Contracts;
using AutoMapper;
using Companies.Shared.DTOs;
using System.Security.Claims;

namespace Companies.Presentation.Controllers.ControllersForTestDemo
{
    [Route("api/repo/{id}")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        // private readonly IEmployeeRepository employeeRepo;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public RepositoryController(IUnitOfWork uow, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.uow = uow;
            // this.employeeRepo = employeeRepo;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(int id)
        {
            //var userId2 = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userId2)) throw new NullReferenceException(nameof(userId2));

            var user = await userManager.GetUserAsync(User);
            if(user is null) throw new ArgumentException(nameof(user));

            var employees = await uow.EmployeeRepository.GetEmployeesAsync(id);
            var dto = mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(dto);
        }
    }
}

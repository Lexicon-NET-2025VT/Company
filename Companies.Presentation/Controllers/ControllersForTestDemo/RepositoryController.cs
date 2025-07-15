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
    public class RepositoryController : ApiControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IServiceManager serviceManager;

        // private readonly IEmployeeRepository employeeRepo;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public RepositoryController(IServiceManager serviceManager, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.uow = uow;
            this.serviceManager = serviceManager;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(int id)
        {
            var user = await userManager.GetUserAsync(User);
            if(user is null) throw new ArgumentException(nameof(user));

            // var employees = await serviceManager.EmployeeService.GetEmployeesAsync(id);

            var response = await serviceManager.EmployeeService.GetEmployeesAsync(id);

            //var dto = mapper.Map<IEnumerable<EmployeeDto>>(employees);
            // return Ok(dto);

            return response.Success ? Ok(response.GetOkResult<IEnumerable<EmployeeDto>>())
                                   : ProcessError(response);
        }
    }
}

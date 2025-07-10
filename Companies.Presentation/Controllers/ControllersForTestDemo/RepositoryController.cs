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

namespace Companies.Presentation.Controllers.ControllersForTestDemo
{
    [Route("api/repo/{id}")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepo;
        private readonly IMapper mapper;

        public RepositoryController(IEmployeeRepository employeeRepo, IMapper mapper)
        {
            this.employeeRepo = employeeRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(int id)
        {
            var employees = await employeeRepo.GetEmployeesAsync(id);
            var dto = mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(dto);
        }
    }
}

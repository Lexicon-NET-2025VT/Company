using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
using Companies.Shared.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Domain.Models.Entities;
// using Companies.Infrastructure.Data;
using Domain.Contracts;

namespace Companies.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public EmployeesController(IMapper mapper, IUnitOfWork uow)
        {

            _mapper = mapper;
            _uow = uow;
        }

        // GET: api/companies/2/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(int companyId)
        {
            // var companyExist = await _context.Companies.AnyAsync(c => c.Id == companyId);
            var companyExist = await _uow.CompanyRepository.CompanyExistAsync(companyId);
            if(!companyExist)
            {
                return NotFound();
            }
            var employees = await _uow.EmployeeRepository.GetEmployeesAsync(companyId);
            var employeesDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeesDtos);
        }

        //// GET: api/Employees/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Employee>> GetEmployee(int id)
        //{
        //    var employee = await _context.Employees.FindAsync(id);

        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    return employee;
        //}

        //// PUT: api/Employees/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEmployee(int id, Employee employee)
        //{
        //    if (id != employee.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(employee).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EmployeeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Employees
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        //{
        //    _context.Employees.Add(employee);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        //}

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, int companyId)
        {
            var companyExist = await _uow.CompanyRepository.CompanyExistAsync(companyId);

            if(!companyExist)
            {
                return NotFound("Company not found");
            }

            var employee = await _uow.EmployeeRepository.GetEmployeeAsync(companyId, id);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            _uow.EmployeeRepository.Delete(employee);
            await _uow.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchEmployee(int companyId, int id, JsonPatchDocument<EmployeeUpdateDto> patchDocument)
        {
            if (patchDocument is null) return BadRequest("No patchdocument");

            var companyExist = await _uow.CompanyRepository.CompanyExistAsync(companyId);

            if (!companyExist) return NotFound("Company does not exist");

            var employeeToPatch = await _uow.EmployeeRepository.GetEmployeeAsync(companyId, id, trackChanges: true);

            if (employeeToPatch == null) return NotFound("Employee does not exist");

            var dto = _mapper.Map<EmployeeUpdateDto>(employeeToPatch);
            // patchDocument.ApplyTo(dto, ModelState); // Todo: check, not working. Här patchas dto:n ihop med patchdokumentet.
            TryValidateModel(dto);

            if(!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(dto, employeeToPatch);
            await _uow.CompleteAsync();

            return NoContent();
        }

















        //private bool EmployeeExists(int id)
        //{
        //    return _context.Employees.Any(e => e.Id == id);
        //}
    }
}

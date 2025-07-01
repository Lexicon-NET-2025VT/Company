using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Companies.API.DTOs;

using Companies.Shared.DTOs;
using Domain.Models.Entities;
using Domain.Contracts;
using Services.Contracts;

namespace Companies.Presentation.Controllers
{
    [Route("api/Companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CompaniesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany(bool includeEmployees)
        {
            var companyDtos = await _serviceManager.CompanyService.GetCompaniesAsync(includeEmployees);
            return Ok(companyDtos);
        }

       

        // GET: api/Companies/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            CompanyDto dto = await _serviceManager.CompanyService.GetCompanyAsync(id);
            return Ok(dto);
        }


        //// PUT: api/Companies/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCompany(int id, CompanyUpdateDto dto)
        //{
        //    if (id != dto.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var existingCompany = await _uow.CompanyRepository.GetCompanyAsync(id, trackChanges: true);
        //    if(existingCompany == null)
        //    {
        //        return NotFound("Company does not exist");
        //    }

        //    _mapper.Map(dto, existingCompany);
        //    await _uow.CompleteAsync();

        //    return NoContent();
        //}

        //// POST: api/Companies
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<CompanyDto>> PostCompany(CompanyCreateDto dto)
        //{
        //    var company = _mapper.Map<Company>(dto);
        //    _uow.CompanyRepository.Create(company);
        //    await _uow.CompleteAsync();

        //    var createdCompany = _mapper.Map<CompanyDto>(company);

        //    return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, createdCompany);
        //}

        //// DELETE: api/Companies/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCompany(int id)
        //{
        //    var company = await _uow.CompanyRepository.GetCompanyAsync(id);
        //    if (company == null)
        //    {
        //        return NotFound("Company not found");
        //    }

        //    _uow.CompanyRepository.Delete(company);
        //    await _uow.CompleteAsync();

        //    return NoContent();
        //}
    }
}

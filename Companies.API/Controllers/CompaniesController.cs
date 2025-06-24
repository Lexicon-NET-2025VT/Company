using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Companies.API.Data;
using Companies.API.Entities;
using Companies.API.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Companies.Shared.DTOs;
using Companies.API.Services;

namespace Companies.API.Controllers
{
    [Route("api/Companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        // private readonly CompaniesContext _context;
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepo;

        public CompaniesController(CompaniesContext context, IMapper mapper, ICompanyRepository companyRepo)
        {
            // _context = context;
            _mapper = mapper;
            _companyRepo = companyRepo;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany(bool includeEmployees)
        {
            // return await _context.Company.ToListAsync();
            // return await _context.Company.Include(c => c.Employees).ToListAsync();

            //var companies = _context.Companies.Select(c => new CompanyDto
            //{
            //    Id = c.Id,
            //    Name = c.Name,
            //    Address = c.Address,
            //    Country = c.Country
            //});

            // var companies = await _context.Companies.ProjectTo<CompanyDto>(_mapper.ConfigurationProvider).ToListAsync();

            //var companies = includeEmployees ? _mapper.Map<IEnumerable<CompanyDto>>(await _context.Companies.Include(c => c.Employees).ToListAsync())
            //                 : _mapper.Map<IEnumerable<CompanyDto>>(await _context.Companies.ToListAsync());

            var companies = includeEmployees ? _mapper.Map<IEnumerable<CompanyDto>>(await _companyRepo.GetCompaniesAsync(true))
                            : _mapper.Map<IEnumerable<CompanyDto>>(await _companyRepo.GetCompaniesAsync());

            return Ok(companies);
        }

       

        // GET: api/Companies/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            // Company? company = await _context.Companies.FindAsync(id);
            Company? company = await _companyRepo.GetCompanyAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            //var dto = new CompanyDto
            //{
            //    Id = company.Id,
            //    Name = company.Name,
            //    Country = company.Country,
            //    Address = company.Address
            //};

            var dto = _mapper.Map<CompanyDto>(company);

            return dto;
        }


        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, CompanyUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var existingCompany = await _companyRepo.GetCompanyAsync(id)
            if(existingCompany == null)
            {
                return NotFound("Company does not exist");
            }

            _mapper.Map(dto, existingCompany);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> PostCompany(CompanyCreateDto dto)
        {
            var company = _mapper.Map<Company>(dto);
            _companyRepo.Add(company);
            await _context.SaveChangesAsync();

            var createdCompany = _mapper.Map<CompanyDto>(company);

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, createdCompany);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _companyRepo.GetCompanyAsync(id);
            if (company == null)
            {
                return NotFound("Company not found");
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}

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
        private readonly IUnitOfWork _uow;

        // private readonly ICompanyRepository _companyRepo;

        public CompaniesController(IMapper mapper, IUnitOfWork uow)
        {
            // _context = context;
            _mapper = mapper;
            _uow = uow;
            // _companyRepo = companyRepo;
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

            var companies = includeEmployees ? _mapper.Map<IEnumerable<CompanyDto>>(await _uow.CompanyRepository.GetCompaniesAsync(true))
                            : _mapper.Map<IEnumerable<CompanyDto>>(await _uow.CompanyRepository.GetCompaniesAsync());

            return Ok(companies);
        }

       

        // GET: api/Companies/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            // Company? company = await _context.Companies.FindAsync(id);
            Company? company = await _uow.CompanyRepository.GetCompanyAsync(id);

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

            var existingCompany = await _uow.CompanyRepository.GetCompanyAsync(id);
            if(existingCompany == null)
            {
                return NotFound("Company does not exist");
            }

            _mapper.Map(dto, existingCompany);
            await _uow.CompleteAsync();

            return NoContent();
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> PostCompany(CompanyCreateDto dto)
        {
            var company = _mapper.Map<Company>(dto);
            _uow.CompanyRepository.Add(company);
            await _uow.CompleteAsync();

            var createdCompany = _mapper.Map<CompanyDto>(company);

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, createdCompany);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _uow.CompanyRepository.GetCompanyAsync(id);
            if (company == null)
            {
                return NotFound("Company not found");
            }

            _uow.CompanyRepository.Delete(company);
            await _uow.CompleteAsync();

            return NoContent();
        }


    }
}

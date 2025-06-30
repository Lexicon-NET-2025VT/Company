using AutoMapper;
using Companies.API.DTOs;
using Domain.Contracts;
using Domain.Models.Entities;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    internal class CompanyService : ICompanyService
    {
        private IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync(bool includeEmployees, bool trackChanges = false)
        {
            return _mapper.Map<IEnumerable<CompanyDto>>(await _uow.CompanyRepository.GetCompaniesAsync(includeEmployees, trackChanges));
        }

        public async Task<CompanyDto> GetCompanyAsync(int id, bool trackChanges = false)
        {
            Company? company = await _uow.CompanyRepository.GetCompanyAsync(id);

            return _mapper.Map<CompanyDto>(company);
        }
    }
}

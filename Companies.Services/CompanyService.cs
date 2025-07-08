using AutoMapper;
using Companies.API.DTOs;
using Companies.Services;
using Companies.Shared.Request;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    public class CompanyService : ICompanyService
    {
        private IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<CompanyDto> companyDtos, MetaData metaData)> GetCompaniesAsync(CompanyRequestParams requestParams, bool trackChanges = false)
        {
            var pagedList = await _uow.CompanyRepository.GetCompaniesAsync(requestParams, trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(pagedList.Items);

            // return _mapper.Map<IEnumerable<CompanyDto>>(await _uow.CompanyRepository.GetCompaniesAsync(requestParams, trackChanges));

            return(companiesDto, pagedList.MetaData);
        }

        public async Task<CompanyDto> GetCompanyAsync(int id, bool trackChanges = false)
        {
            Company? company = await _uow.CompanyRepository.GetCompanyAsync(id);

            if (company == null)
            {
                throw new CompanyNotFoundException(id);
            }

            return _mapper.Map<CompanyDto>(company);
        }
    }
}

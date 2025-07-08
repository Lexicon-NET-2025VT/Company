using AutoMapper;
using Companies.Shared.DTOs;
using Domain.Contracts;
using Domain.Models.Responses;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;

        public EmployeeService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> GetEmployeesAsync(int companyId)
        {
            var companyExist = await _uow.CompanyRepository.CompanyExistAsync(companyId);

            if(!companyExist)
            {
                return new CompanyNotFoundResponse(companyId);
            }

            var employees = await _uow.EmployeeRepository.GetEmployeesAsync(companyId);

            var employeesDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return new ApiOkResponse<IEnumerable<EmployeeDto>>(employeesDtos);
        }
    }
}

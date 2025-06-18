using AutoMapper;
using Companies.API.Entities;
using Companies.API.DTOs;
using Companies.Shared.DTOs;

namespace Companies.API.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Company, CompanyDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CompanyCreateDto, Company>();
        }



    }
}

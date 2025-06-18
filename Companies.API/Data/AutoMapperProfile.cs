using AutoMapper;
using Companies.API.Entities;
using Companies.API.DTOs;

namespace Companies.API.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Company, CompanyDto>();
        }



    }
}

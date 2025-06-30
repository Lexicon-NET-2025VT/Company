using AutoMapper;
using Domain.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    internal class EmployeeService : IEmployeeService
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;

        public EmployeeService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
    }
}

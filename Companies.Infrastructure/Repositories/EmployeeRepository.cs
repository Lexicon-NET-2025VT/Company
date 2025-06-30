using Companies.Infrastructure.Data;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Infrastructure.Repositories
{
    internal class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CompaniesContext context) : base(context)
        {
            
        }

        public async Task<Employee?> GetEmployeeAsync(int companyId, int employeeId, bool trackChanges = false)
        {
            return await FindByCondition(e => e.Id.Equals(employeeId) && e.CompanyId.Equals(companyId), trackChanges).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(int companyId, bool trackChanges = false)
        {
            // var employees = await _context.Employees.Where(e => e.CompanyId.Equals(companyId)).ToListAsync();

            return await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).ToListAsync();

        }
    }
}

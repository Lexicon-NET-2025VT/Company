using Companies.Infrastructure.Data;
using Domain.Contracts;

namespace Companies.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompaniesContext _context;
        public ICompanyRepository CompanyRepository { get;}
        public IEmployeeRepository EmployeeRepository { get; }

        // Fler repos

        public UnitOfWork(CompaniesContext context)
        {
            _context = context;
            CompanyRepository = new CompanyRepository(context);

            EmployeeRepository = new EmployeeRepository(context);
        }



        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

using Companies.Infrastructure.Data;
using Domain.Contracts;

namespace Companies.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompaniesContext _context;
        public ICompanyRepository CompanyRepository { get;}
        // Fler repos

        public UnitOfWork(CompaniesContext context)
        {
            _context = context;
            CompanyRepository = new CompanyRepository(context);
        }



        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

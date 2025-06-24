
using Companies.API.Data;

namespace Companies.API.Services
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

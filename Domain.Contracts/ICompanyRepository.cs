using Domain.Models.Entities;

namespace Domain.Contracts
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees = false);
        Task<Company?> GetCompanyAsync(int id);

        void Add(Company company);
        void Delete(Company company);
    }
}
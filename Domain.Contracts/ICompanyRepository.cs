using Domain.Models.Entities;

namespace Domain.Contracts
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees = false, bool trackChanges = false);
        Task<Company?> GetCompanyAsync(int id, bool trackChanges = false);

        void Create(Company company);
        void Delete(Company company);

        Task<bool> CompanyExistAsync(int id);
    }
}
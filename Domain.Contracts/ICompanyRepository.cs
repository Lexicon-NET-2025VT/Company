using Companies.Shared.Request;
using Domain.Models.Entities;

namespace Domain.Contracts
{
    public interface ICompanyRepository
    {
        Task<PagedList<Company>> GetCompaniesAsync(CompanyRequestParams requestParams, bool trackChanges = false);
        Task<Company?> GetCompanyAsync(int id, bool trackChanges = false);

        void Create(Company company);
        void Delete(Company company);

        Task<bool> CompanyExistAsync(int id);
    }
}
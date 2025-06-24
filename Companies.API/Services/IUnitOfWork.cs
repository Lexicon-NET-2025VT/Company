namespace Companies.API.Services
{
    public interface IUnitOfWork
    {
        ICompanyRepository CompanyRepository { get; }
        Task CompleteAsync();
    }
}

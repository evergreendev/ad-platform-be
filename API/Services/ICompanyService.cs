using API.DTOs.Companies;

namespace API.Services;

public interface ICompanyService
{
    Task<CompanyResponse> CreateCompanyAsync(CreateCompanyRequest request);
    Task<IEnumerable<CompanyResponse>> GetCompaniesAsync();
    Task<CompanyResponse?> GetCompanyByIdAsync(Guid id);
}

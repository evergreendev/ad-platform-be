using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Services;

public interface ICompanySearchService
{
    Task<(IEnumerable<Company> Items, int TotalCount)> SearchAsync(CompanyQuery query);
}

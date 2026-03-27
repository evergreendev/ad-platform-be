using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class DbCompanySearchService(ApplicationDbContext context) : ICompanySearchService
{
    public async Task<(IEnumerable<Company> Items, int TotalCount)> SearchAsync(CompanyQuery query)
    {
        var dbQuery = context.Companies.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.CompanyName))
            dbQuery = dbQuery.Where(c => c.CompanyName.Contains(query.CompanyName));

        if (!string.IsNullOrWhiteSpace(query.Address))
            dbQuery = dbQuery.Where(c => c.Address != null && c.Address.Contains(query.Address));

        if (!string.IsNullOrWhiteSpace(query.City))
            dbQuery = dbQuery.Where(c => c.City != null && c.City.Contains(query.City));

        if (!string.IsNullOrWhiteSpace(query.State))
            dbQuery = dbQuery.Where(c => c.State != null && c.State.Contains(query.State));

        if (!string.IsNullOrWhiteSpace(query.Zip))
            dbQuery = dbQuery.Where(c => c.Zip != null && c.Zip.Contains(query.Zip));

        if (!string.IsNullOrWhiteSpace(query.Country))
            dbQuery = dbQuery.Where(c => c.Country != null && c.Country.Contains(query.Country));

        if (!string.IsNullOrWhiteSpace(query.WebsiteUrl))
            dbQuery = dbQuery.Where(c => c.WebsiteUrl != null && c.WebsiteUrl.Contains(query.WebsiteUrl));

        if (!string.IsNullOrWhiteSpace(query.Type))
            dbQuery = dbQuery.Where(c => c.Type != null && c.Type.Contains(query.Type));

        if (!string.IsNullOrWhiteSpace(query.TaxId))
            dbQuery = dbQuery.Where(c => c.TaxId != null && c.TaxId.Contains(query.TaxId));

        if (!string.IsNullOrWhiteSpace(query.PrimaryRepName))
            dbQuery = dbQuery.Where(c => c.PrimaryRepName != null && c.PrimaryRepName.Contains(query.PrimaryRepName));

        if (!string.IsNullOrWhiteSpace(query.LegacyPrimaryCategory))
            dbQuery = dbQuery.Where(c => c.LegacyPrimaryCategory != null && c.LegacyPrimaryCategory.Contains(query.LegacyPrimaryCategory));

        if (query.HubspotCompanyId.HasValue)
            dbQuery = dbQuery.Where(c => c.HubspotCompanyId == query.HubspotCompanyId.Value);

        if (query.Collections.HasValue)
            dbQuery = dbQuery.Where(c => c.Collections == query.Collections.Value);

        if (query.WriteOff.HasValue)
            dbQuery = dbQuery.Where(c => c.WriteOff == query.WriteOff.Value);

        if (query.Dead.HasValue)
            dbQuery = dbQuery.Where(c => c.IsActive == query.Dead.Value);

        if (query.IsNewCompany.HasValue)
            dbQuery = dbQuery.Where(c => c.IsNewCompany == query.IsNewCompany.Value);

        if (query.CompanySpecialBilling.HasValue)
            dbQuery = dbQuery.Where(c => c.CompanySpecialBilling == query.CompanySpecialBilling.Value);

        var totalCount = await dbQuery.CountAsync();

        var page = query.Page ?? 1;
        var pageSize = query.PageSize ?? 20;

        var items = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}

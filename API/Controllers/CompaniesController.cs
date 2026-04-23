using API.DTOs.Companies;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompaniesController(ICompanySearchService searchService, ICompanyService companyService) : ControllerBase
{
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] CompanyQuery query)
    {
        var (items, totalCount) = await searchService.SearchAsync(query);
        
        return Ok(new
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page ?? 1,
            PageSize = query.PageSize ?? 20
        });
    }

    [HttpPost]
    public async Task<ActionResult<CompanyResponse>> CreateCompany(CreateCompanyRequest request)
    {
        var createdCompany = await companyService.CreateCompanyAsync(request);
        return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany.Id }, createdCompany);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyResponse>>> GetCompanies()
    {
        var companies = await companyService.GetCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyResponse>> GetCompanyById(Guid id)
    {
        var company = await companyService.GetCompanyByIdAsync(id);
        if (company == null)
        {
            return NotFound();
        }

        return Ok(company);
    }
}

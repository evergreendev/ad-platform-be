using System.Threading.Tasks;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompaniesController(ICompanySearchService searchService) : ControllerBase
{
    [HttpGet]
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
}

using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController(CompanyService companyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll(
        [FromQuery] string username,
        [FromQuery] string password,
        CancellationToken ct)
    {
        var companies = await companyService.GetAllCompaniesAsync(username, password, ct);
        return Ok(companies);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CompanyDto>> GetById(
        int id,
        [FromQuery] string username,
        [FromQuery] string password,
        CancellationToken ct)
    {
        var company = await companyService.GetCompanyByIdAsync(id, username, password, ct);
        return company is not null ? Ok(company) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<CompanyDto>> Create(
        [FromBody] CreateCompanyDto dto,
        [FromQuery] string username,
        [FromQuery] string password,
        CancellationToken ct)
    {
        var createdCompany = await companyService.CreateCompanyAsync(dto, username, password, ct);
        return CreatedAtAction(nameof(GetById), new { id = createdCompany.Id, username, password }, createdCompany);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CreateCompanyDto dto,
        [FromQuery] string username,
        [FromQuery] string password,
        CancellationToken ct)
    {
        await companyService.UpdateCompanyAsync(id, dto, username, password, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        [FromQuery] string username,
        [FromQuery] string password,
        CancellationToken ct)
    {
        await companyService.DeleteCompanyAsync(id, username, password, ct);
        return NoContent();
    }
}
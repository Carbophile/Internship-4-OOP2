using Application.DTOs;
using Application.Interfaces;
using Domain.Classes;
using Domain.Exceptions;

namespace Application.Services;

public class CompanyService(
    ICompanyRepository companyRepository,
    IUserRepository userRepository)
{
    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(string username, string password,
        CancellationToken ct)
    {
        await ValidateUserAsync(username, password, ct);

        var companies = await companyRepository.GetAllAsync(ct);
        return companies.Select(MapToDto);
    }

    public async Task<CompanyDto?> GetCompanyByIdAsync(int id, string username, string password, CancellationToken ct)
    {
        await ValidateUserAsync(username, password, ct);

        var company = await companyRepository.GetByIdAsync(id, ct);
        return company == null ? null : MapToDto(company);
    }

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto, string username, string password,
        CancellationToken ct)
    {
        await ValidateUserAsync(username, password, ct);

        var existingCompany = await companyRepository.GetByNameAsync(dto.Name, ct);
        if (existingCompany != null)
            throw new DomainValidationException($"Company with name '{dto.Name}' already exists.");

        var newCompany = new Company
        {
            Name = dto.Name
        };

        await companyRepository.AddAsync(newCompany, ct);
        await companyRepository.SaveChangesAsync(ct);

        return MapToDto(newCompany);
    }

    public async Task UpdateCompanyAsync(int id, CreateCompanyDto dto, string username, string password,
        CancellationToken ct)
    {
        await ValidateUserAsync(username, password, ct);

        var company = await companyRepository.GetByIdAsync(id, ct);
        if (company == null) throw new KeyNotFoundException($"Company with ID {id} not found.");

        if (!string.Equals(company.Name, dto.Name, StringComparison.OrdinalIgnoreCase))
        {
            var existingCompany = await companyRepository.GetByNameAsync(dto.Name, ct);
            if (existingCompany != null)
                throw new DomainValidationException($"Company with name '{dto.Name}' already exists.");
        }

        company.Name = dto.Name;

        await companyRepository.UpdateAsync(company, ct);
        await companyRepository.SaveChangesAsync(ct);
    }

    public async Task DeleteCompanyAsync(int id, string username, string password, CancellationToken ct)
    {
        await ValidateUserAsync(username, password, ct);

        var company = await companyRepository.GetByIdAsync(id, ct);
        if (company == null) return;

        await companyRepository.DeleteAsync(company, ct);
        await companyRepository.SaveChangesAsync(ct);
    }

    private async Task ValidateUserAsync(string username, string password, CancellationToken ct)
    {
        var user = await userRepository.GetByUsernameAsync(username, ct);

        if (user == null || user.Password != password)
            throw new DomainValidationException("Invalid username or password.");

        if (!user.IsActive) throw new DomainValidationException("User is not active.");
    }

    private static CompanyDto MapToDto(Company company)
    {
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name
        };
    }
}
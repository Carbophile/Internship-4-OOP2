using Application.Interfaces;
using Dapper;
using Domain.Classes;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class CompanyRepository(CompanyDbContext context) : ICompanyRepository
{
    // Dapper Queries
    public async Task<IEnumerable<Company>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        const string query = "SELECT id, name FROM companies";
        return await connection.QueryAsync<Company>(query);
    }

    public async Task<Company?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        const string query = "SELECT id, name FROM companies WHERE id = @Id";
        return await connection.QuerySingleOrDefaultAsync<Company>(query, new { Id = id });
    }

    public async Task<Company?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        const string query = "SELECT id, name FROM companies WHERE name = @Name";
        return await connection.QuerySingleOrDefaultAsync<Company>(query, new { Name = name });
    }

    // EF Core Commands
    public async Task AddAsync(Company company, CancellationToken cancellationToken = default)
    {
        await context.Companies.AddAsync(company, cancellationToken);
    }

    public Task UpdateAsync(Company company, CancellationToken cancellationToken = default)
    {
        context.Companies.Update(company);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Company company, CancellationToken cancellationToken = default)
    {
        context.Companies.Remove(company);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
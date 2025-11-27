using Application.Interfaces;
using Dapper;
using Domain.Classes;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRepository(UserDbContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        const string query = """
                             SELECT id, name, username, email, address_street AS AddressStreet, 
                                    address_city AS AddressCity, geo_lat AS GeoLat, geo_lng AS GeoLng, 
                                    website, password, created_at AS CreatedAt, updated_at AS UpdatedAt, is_active AS IsActive
                             FROM users
                             """;
        return await connection.QueryAsync<User>(query);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        const string query = """
                             SELECT id, name, username, email, address_street AS AddressStreet, 
                                    address_city AS AddressCity, geo_lat AS GeoLat, geo_lng AS GeoLng, 
                                    website, password, created_at AS CreatedAt, updated_at AS UpdatedAt, is_active AS IsActive
                             FROM users WHERE id = @Id
                             """;
        return await connection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        const string query = """
                             SELECT id, name, username, email, address_street AS AddressStreet, 
                                    address_city AS AddressCity, geo_lat AS GeoLat, geo_lng AS GeoLng, 
                                    website, password, created_at AS CreatedAt, updated_at AS UpdatedAt, is_active AS IsActive
                             FROM users WHERE username = @Username
                             """;
        return await connection.QuerySingleOrDefaultAsync<User>(query, new { Username = username });
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        const string query = """
                             SELECT id, name, username, email, address_street AS AddressStreet, 
                                    address_city AS AddressCity, geo_lat AS GeoLat, geo_lng AS GeoLng, 
                                    website, password, created_at AS CreatedAt, updated_at AS UpdatedAt, is_active AS IsActive
                             FROM users WHERE email = @Email
                             """;
        return await connection.QuerySingleOrDefaultAsync<User>(query, new { Email = email });
    }

    public async Task<IEnumerable<(double Lat, double Lng)>> GetAllCoordinatesAsync(
        CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        const string query = "SELECT geo_lat AS Lat, geo_lng AS Lng FROM users";
        return await connection.QueryAsync<(double Lat, double Lng)>(query);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        context.Users.Remove(user);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
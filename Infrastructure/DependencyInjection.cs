using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string userConnectionString,
        string companyConnectionString)
    {
        services.AddDbContext<UserDbContext>(options =>
            options
                .UseNpgsql(userConnectionString)
                .UseSnakeCaseNamingConvention());

        services.AddDbContext<CompanyDbContext>(options =>
            options
                .UseNpgsql(companyConnectionString)
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();

        services.AddMemoryCache();
        services.AddSingleton<ICacheService, InMemoryCacheService>();

        services.AddHttpClient<IExternalSystemService, ExternalSystemService>();

        return services;
    }
}
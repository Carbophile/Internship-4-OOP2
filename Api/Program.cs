using Api.Middleware;
using Application.Services;
using Infrastructure;

namespace Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<CompanyService>();

        var userConn = builder.Configuration.GetConnectionString("UserDatabase");
        var companyConn = builder.Configuration.GetConnectionString("CompanyDatabase");

        if (string.IsNullOrWhiteSpace(userConn) || string.IsNullOrWhiteSpace(companyConn))
            throw new InvalidOperationException(
                "Connection strings 'UserDatabase' and 'CompanyDatabase' must be configured.");

        builder.Services.AddInfrastructure(userConn, companyConn);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
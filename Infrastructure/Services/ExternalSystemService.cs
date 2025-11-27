using System.Net.Http.Json;
using Application.DTOs.External;
using Application.Interfaces;

namespace Infrastructure.Services;

public class ExternalSystemService(HttpClient httpClient) : IExternalSystemService
{
    public async Task<IEnumerable<ExternalUserDto>> FetchExternalUsersAsync(
        CancellationToken cancellationToken = default)
    {
        const string url = "https://jsonplaceholder.typicode.com/users";
        var response = await httpClient.GetAsync(url, cancellationToken);

        response.EnsureSuccessStatusCode();

        var users = await response.Content.ReadFromJsonAsync<IEnumerable<ExternalUserDto>>(cancellationToken);
        return users ?? [];
    }
}
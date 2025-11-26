using Application.DTOs.External;

namespace Application.Interfaces;

public interface IExternalSystemService
{
    Task<IEnumerable<ExternalUserDto>> FetchExternalUsersAsync(CancellationToken cancellationToken = default);
}
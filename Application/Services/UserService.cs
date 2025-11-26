using Application.DTOs;
using Application.DTOs.External;
using Application.Interfaces;
using Domain.Classes;
using Domain.Exceptions;

namespace Application.Services;

public class UserService(
    IUserRepository userRepository,
    IExternalSystemService externalSystem,
    ICacheService cacheService)
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken ct)
    {
        var users = await userRepository.GetAllAsync(ct);
        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(id, ct);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserDto> CreateUserAsync(CreaterUserDto dto, CancellationToken ct)
    {
        var existingUser = await userRepository.GetByUsernameAsync(dto.Username, ct);
        if (existingUser != null) throw new DomainValidationException("Username must be unique.");

        var existingEmail = await userRepository.GetByEmailAsync(dto.Email, ct);
        if (existingEmail != null) throw new DomainValidationException("Email must be unique.");

        var allCoords = await userRepository.GetAllCoordinatesAsync(ct);
        foreach (var (lat, lng) in allCoords)
            if (CalculateDistance(dto.GeoLat, dto.GeoLng, lat, lng) < 3.0)
                throw new DomainValidationException("User location is within 3km of an existing user.");

        var newUser = new User
        {
            Name = dto.Name,
            Username = dto.Username,
            Email = dto.Email,
            AddressStreet = dto.AddressStreet,
            AddressCity = dto.AddressCity,
            GeoLat = dto.GeoLat,
            GeoLng = dto.GeoLng,
            Website = dto.Website,
            Password = Guid.NewGuid().ToString(),
            IsActive = true
        };

        await userRepository.AddAsync(newUser, ct);
        await userRepository.SaveChangesAsync(ct);

        return MapToDto(newUser);
    }

    public async Task UpdateUserAsync(int id, CreaterUserDto dto, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(id, ct);
        if (user == null) throw new KeyNotFoundException($"User with ID {id} not found.");

        user.Name = dto.Name;
        user.AddressStreet = dto.AddressStreet;
        user.AddressCity = dto.AddressCity;
        user.GeoLat = dto.GeoLat;
        user.GeoLng = dto.GeoLng;
        user.Website = dto.Website;

        await userRepository.UpdateAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);
    }

    public async Task DeactivateUserAsync(int id, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(id, ct);
        if (user == null) throw new KeyNotFoundException($"User {id} not found.");

        user.IsActive = false;
        await userRepository.UpdateAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);
    }

    public async Task ActivateUserAsync(int id, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(id, ct);
        if (user == null) throw new KeyNotFoundException($"User {id} not found.");

        user.IsActive = true;
        await userRepository.UpdateAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);
    }

    public async Task DeleteUserAsync(int id, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(id, ct);
        if (user == null) return;

        await userRepository.DeleteAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);
    }

    public async Task ImportExternalUsersAsync(CancellationToken ct)
    {
        const string cacheKey = "ExternalUsersData";

        var cachedUsers = await cacheService.GetAsync<IEnumerable<ExternalUserDto>>(cacheKey);

        if (cachedUsers == null)
            try
            {
                var fetched = await externalSystem.FetchExternalUsersAsync(ct);
                var fetchedList = fetched.ToList();
                var endOfDay = DateTime.Today.AddDays(1) - DateTime.Now;
                await cacheService.SetAsync(cacheKey, fetchedList, endOfDay);
                cachedUsers = fetchedList;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("External API unavailable and no local cache.");
            }

        foreach (var ext in cachedUsers)
        {
            if (await userRepository.GetByUsernameAsync(ext.Username, ct) != null) continue;

            var newUser = new User
            {
                Name = ext.Name,
                Username = ext.Username,
                Email = ext.Email,
                AddressStreet = ext.Address.Street,
                AddressCity = ext.Address.City,
                GeoLat = ext.Address.Geo.Lat,
                GeoLng = ext.Address.Geo.Lng,
                Website = ext.Website,
                Password = Guid.NewGuid().ToString(),
                IsActive = true
            };

            await userRepository.AddAsync(newUser, ct);
        }

        await userRepository.SaveChangesAsync(ct);
    }

    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const int r = 6371;
        var dLat = Deg2Rad(lat2 - lat1);
        var dLon = Deg2Rad(lon2 - lon1);
        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return r * c;
    }

    private static double Deg2Rad(double deg)
    {
        return deg * (Math.PI / 180);
    }

    private static UserDto MapToDto(User u)
    {
        return new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Username = u.Username,
            Email = u.Email,
            GeoLat = u.GeoLat,
            GeoLng = u.GeoLng,
            Website = u.Website,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt ?? DateTime.MinValue,
            UpdatedAt = u.UpdatedAt
        };
    }
}
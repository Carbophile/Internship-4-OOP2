namespace Application.DTOs;

public sealed class UserDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required double GeoLat { get; set; }
    public required double GeoLng { get; set; }
    public required string? Website { get; set; }
    public required bool IsActive { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}
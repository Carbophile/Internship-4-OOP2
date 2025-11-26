namespace Application.DTOs;

public sealed class CreaterUserDto
{
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string AddressStreet { get; set; }
    public required string AddressCity { get; set; }
    public required double GeoLat { get; set; }
    public required double GeoLng { get; set; }
    public string? Website { get; set; }
}
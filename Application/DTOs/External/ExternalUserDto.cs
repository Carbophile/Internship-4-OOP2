namespace Application.DTOs.External;

public sealed class ExternalUserDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required ExternalAddressDto Address { get; set; }
    public required string Phone { get; set; }
    public required string Website { get; set; }
    public required ExternalCompanyDto Company { get; set; }
}
namespace Application.DTOs.External;

public sealed class ExternalAddressDto
{
    public required string Street { get; set; }
    public required string Suite { get; set; }
    public required string City { get; set; }
    public required string Zipcode { get; set; }
    public required ExternalGeoDto Geo { get; set; }
}
namespace Application.DTOs.External;

public sealed class ExternalCompanyDto
{
    public required string Name { get; set; } = string.Empty;
    public required string CatchPhrase { get; set; } = string.Empty;
    public required string Bs { get; set; } = string.Empty;
}
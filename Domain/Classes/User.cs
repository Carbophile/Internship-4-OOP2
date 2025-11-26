using Domain.Classes.Common;
using Domain.Validation;

namespace Domain.Classes;

public sealed class User : BaseEntity
{
    public const int MaxNameLength = 100;
    public const int MaxUsernameLength = 100;
    public const int MaxEmailLength = 255;
    public const int MaxStreetLength = 150;
    public const int MaxCityLength = 100;
    public const int MaxWebsiteLength = 100;
    public const int MaxPasswordLength = 100;

    public required string Name
    {
        get;
        set
        {
            Guard.AgainstInvalidString(value, MaxNameLength, nameof(Name));
            UpdateTimestamp();
            field = value;
        }
    }

    public required string Username
    {
        get;
        set
        {
            Guard.AgainstInvalidString(value, MaxUsernameLength, nameof(Username));
            UpdateTimestamp();
            field = value;
        }
    }

    public required string Email
    {
        get;
        set
        {
            Guard.AgainstInvalidEmail(value, MaxEmailLength, nameof(Email));
            UpdateTimestamp();
            field = value;
        }
    }

    public required string AddressStreet
    {
        get;
        set
        {
            Guard.AgainstInvalidString(value, MaxStreetLength, nameof(AddressStreet));
            UpdateTimestamp();
            field = value;
        }
    }

    public required string AddressCity
    {
        get;
        set
        {
            Guard.AgainstInvalidString(value, MaxCityLength, nameof(AddressCity));
            UpdateTimestamp();
            field = value;
        }
    }

    public required double GeoLat
    {
        get;
        set
        {
            Guard.AgainstInvalidDouble(value, -90, 90, nameof(GeoLat));
            UpdateTimestamp();
            field = value;
        }
    }

    public required double GeoLng
    {
        get;
        set
        {
            Guard.AgainstInvalidDouble(value, -180, 180, nameof(GeoLng));
            UpdateTimestamp();
            field = value;
        }
    }

    public string? Website
    {
        get;
        set
        {
            if (value != null) Guard.AgainstInvalidString(value, MaxWebsiteLength, nameof(Website));
            UpdateTimestamp();
            field = value;
        }
    }

    public required string Password
    {
        get;
        set
        {
            Guard.AgainstInvalidString(value, MaxPasswordLength, nameof(Password));
            UpdateTimestamp();
            field = value;
        }
    }

    public DateTime? CreatedAt
    {
        get;
        init => field = value ?? DateTime.UtcNow;
    }

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
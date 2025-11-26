using Domain.Classes.Common;
using Domain.Validation;

namespace Domain.Classes;

public sealed class Company : BaseEntity
{
    public const int MaxNameLength = 150;

    public required string Name
    {
        get;
        set
        {
            Guard.AgainstInvalidString(value, MaxNameLength, nameof(Name));
            field = value;
        }
    }
}
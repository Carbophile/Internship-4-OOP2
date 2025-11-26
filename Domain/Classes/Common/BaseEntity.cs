using Domain.Validation;

namespace Domain.Classes.Common;

public abstract class BaseEntity
{
    public required int Id
    {
        get;
        init
        {
            Guard.AgainstInvalidInt(value, nameof(Id));
            field = value;
        }
    }
}
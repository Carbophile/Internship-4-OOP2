using Domain.Validation;

namespace Domain.Classes.Common;

public abstract class BaseEntity
{
    public int Id
    {
        get;
        init
        {
            Guard.AgainstInvalidInt(value, nameof(Id));
            field = value;
        }
    }
}
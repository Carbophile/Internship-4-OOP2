using System.Net.Mail;
using Domain.Exceptions;

namespace Domain.Validation;

internal static class Guard
{
    internal static void AgainstInvalidInt(int integer, string field)
    {
        if (integer < 0) throw new DomainValidationException($"{field} cannot be negative!");
    }

    internal static void AgainstInvalidString(string str, int maxLength, string field)
    {
        if (string.IsNullOrEmpty(str)) throw new DomainValidationException($"{field} cannot be empty!");
        if (str.Length > maxLength)
            throw new DomainValidationException($"{field} cannot be longer than {maxLength} characters!");
    }

    internal static void AgainstInvalidEmail(string email, int maxLength, string field)
    {
        AgainstInvalidString(email, maxLength, field);

        MailAddress.TryCreate(email, out var parsedEmail);
        if (parsedEmail == null || email != parsedEmail.ToString())
            throw new DomainValidationException($"{field} must be a properly formatted email!");
    }

    internal static void AgainstInvalidDouble(double dbl, int min, int max, string field)
    {
        if (dbl < min || dbl > max)
            throw new DomainValidationException($"{field} must be between {min} and {max}!");
    }

    internal static void AgainstInvalidUrl(string url, int maxLength, string field)
    {
        AgainstInvalidString(url, maxLength, field);

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            throw new DomainValidationException($"{field} must be a valid URL (http or https)!");
    }
}
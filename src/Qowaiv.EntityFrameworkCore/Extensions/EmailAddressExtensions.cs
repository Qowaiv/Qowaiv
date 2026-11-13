namespace Qowaiv.EntityFrameworkCore.Extensions;

public static class EmailAddressExtensions
{
    extension(EmailAddress email)
    {
        public bool Like(string pattern)
        {
            return true;
        }

        public bool HasDomain(string domain) => email.Like($"@{domain}%");
    }
}

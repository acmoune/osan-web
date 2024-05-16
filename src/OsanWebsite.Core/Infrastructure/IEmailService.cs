namespace OsanWebsite.Core.Infrastructure;

public interface IEmailService
{
    Task<bool> SendMail(string to, string subject, string body);
}

using Microsoft.Extensions.Logging;
using MimeKit;

namespace OsanWebsite.Core.Infrastructure;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendMail(string to, string subject, string body)
    {
        var smtpServer = Environment.GetEnvironmentVariable("OSAN_SMTP_Server");
        var smtpPort = Environment.GetEnvironmentVariable("OSAN_SMTP_Port");
        var smtpUser = Environment.GetEnvironmentVariable("OSAN_SMTP_User");
        var smtpPwd = Environment.GetEnvironmentVariable("OSAN_SMTP_Password");

        bool success;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("O'SAN Cave", smtpUser));
        message.ReplyTo.Add(new MailboxAddress("O'SAN Cave", smtpUser));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;
        message.Body = new TextPart("plain")
        {
            Text = body,
        };

        try
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(smtpServer, int.Parse(smtpPort!), true);
                await client.AuthenticateAsync(smtpUser, smtpPwd);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            _logger.LogInformation($"Sent email to {to}, concerning {subject}");
            success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            success = false;
        }

        return success;
    }
}

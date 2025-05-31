using Billi.Backend.Application.Services.Email.Interfaces;
using Billi.Backend.CrossCutting.Configurations;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Billi.Backend.Application.Services.Email
{
    public class EmailService(IOptions<EmailSettings> options) : IEmailService
    {
        private readonly EmailSettings _settings = options.Value;

        public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.SslOnConnect, cancellationToken);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
            await smtp.SendAsync(message, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}

namespace Billi.Backend.Application.Services.Email.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken);
    }
}

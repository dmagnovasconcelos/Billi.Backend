using Billi.Backend.Domain.ValueObjects;

namespace Billi.Backend.Application.Services
{
    public interface IAccessTokenGeneratorService
    {
        AccessToken GenerateAccessToken(string email, Guid userId, CancellationToken cancellationToken);
        Task<SessionAuth> ValidateToken(string token, CancellationToken cancellationToken);
    }
}

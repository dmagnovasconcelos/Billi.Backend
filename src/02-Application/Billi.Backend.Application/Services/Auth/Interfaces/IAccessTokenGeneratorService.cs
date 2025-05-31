using Billi.Backend.CrossCutting.Auth;
using Billi.Backend.Domain.ValueObjects;

namespace Billi.Backend.Application.Services.Auth.Interfaces
{
    public interface IAccessTokenGeneratorService
    {
        AccessToken GenerateAccessToken(string email, Guid userId, CancellationToken cancellationToken);

        Task<CurrentUser> ValidateToken(string token, CancellationToken cancellationToken);
    }
}
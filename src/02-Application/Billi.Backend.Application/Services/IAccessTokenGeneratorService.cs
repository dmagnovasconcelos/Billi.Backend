using Billi.Backend.Domain.ValueObjects;

namespace Billi.Backend.Application.Services
{
    public interface IAccessTokenGeneratorService
    {
        AccessToken GenerateAccessToken(string email);

        AccessToken RefreshAccessToken(AccessToken accessToken);
    }
}

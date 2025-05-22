using Billi.Backend.CrossCutting.Configurations;
using Billi.Backend.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Billi.Backend.Application.Services
{
    public class AccessTokenGeneratorService(IOptions<AuthenticationOptions> configuration) : IAccessTokenGeneratorService
    {
        private readonly AuthenticationOptions _configuration = configuration.Value;

        public AccessToken GenerateAccessToken(string email)
        {
            ClaimsIdentity identity = new(
            new GenericIdentity(email, "E-mail"),
            [
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                  new Claim(JwtRegisteredClaimNames.UniqueName, email)
            ]);

            var created = DateTime.Now;
            var expiration = created.AddMinutes(_configuration.Expiration);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = _configuration.Issuer,
                Audience = _configuration.Audience,
                NotBefore = created,
                Expires = expiration,
                SigningCredentials = _configuration.SigningCredentials,
                Subject = identity
            };

            JwtSecurityTokenHandler tokenHandler = new();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            var result = new AccessToken(accessToken, false, created, expiration);

            return result;
        }

        public AccessToken RefreshAccessToken(AccessToken accessToken)
        {
            throw new NotImplementedException();
        }
    }
}

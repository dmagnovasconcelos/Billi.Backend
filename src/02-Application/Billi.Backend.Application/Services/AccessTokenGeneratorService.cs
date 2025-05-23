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

        public AccessToken GenerateAccessToken(string email, Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ClaimsIdentity identity = new(
            new GenericIdentity(email, "E-mail"),
            [
                new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
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
            var result = new AccessToken(accessToken, email, userId, false, created, expiration);

            return result;
        }

        public async Task<SessionAuth> ValidateToken(string token, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _configuration.SymmetricSecurityKey,
                ValidateIssuer = true,
                ValidIssuer = _configuration.Issuer,
                ValidateAudience = true,
                ValidAudience = _configuration.Audience,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);

            return new SessionAuth(token);
        }
    }
}

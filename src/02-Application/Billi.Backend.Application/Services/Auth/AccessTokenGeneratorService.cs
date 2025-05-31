using Billi.Backend.Application.Services.Auth.Interfaces;
using Billi.Backend.CrossCutting.Auth;
using Billi.Backend.CrossCutting.Configurations;
using Billi.Backend.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Billi.Backend.Application.Services.Auth
{
    public class AccessTokenGeneratorService(IOptions<AuthenticationSettings> authConfiguration, IHttpContextAccessor httpContextAccessor) : IAccessTokenGeneratorService
    {
        private readonly AuthenticationSettings _authConfiguration = authConfiguration.Value;

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
            var expiration = created.AddHours(_authConfiguration.Expiration);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = _authConfiguration.Issuer,
                Audience = _authConfiguration.Audience,
                NotBefore = created,
                Expires = expiration,
                SigningCredentials = _authConfiguration.SigningCredentials,
                Subject = identity
            };

            JwtSecurityTokenHandler tokenHandler = new();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            var result = new AccessToken(accessToken, email, userId, false, created, expiration);

            return result;
        }

        public async Task<CurrentUser> ValidateToken(string token, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _authConfiguration.SymmetricSecurityKey,
                ValidateIssuer = true,
                ValidIssuer = _authConfiguration.Issuer,
                ValidateAudience = true,
                ValidAudience = _authConfiguration.Audience,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);

            return CurrentUser.GetSessionAuth(token, httpContextAccessor);
        }
    }
}
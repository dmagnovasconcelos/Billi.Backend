using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace Billi.Backend.CrossCutting.Auth
{
    public class CurrentUser : ICurrentUser
    {
        public const string BearerPrefix = "Bearer ";

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            StringValues value = string.Empty;
            httpContextAccessor?.HttpContext?.Request?.Headers?.TryGetValue("Authorization", out value);

            if (value.Count > 0 && value.Any(x => x.Contains(BearerPrefix)))
            {
                Deserialize(value);
            }
        }

        public string Email { get; private set; }
        public Guid? UserId { get; private set; }
        public string Token { get; private set; }

        public void Deserialize(string token)
        {
            Token = token.Replace(BearerPrefix, string.Empty, StringComparison.InvariantCultureIgnoreCase);

            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(Token);

            Email = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            UserId = new Guid(jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
        }

        public static CurrentUser GetSessionAuth(string token, IHttpContextAccessor httpContextAccessor)
        {
            var session = new CurrentUser(httpContextAccessor);
            session.Deserialize(token);
            return session;
        }
    }
}
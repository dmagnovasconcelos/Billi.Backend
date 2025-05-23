using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace Billi.Backend.Domain.ValueObjects
{
    public class SessionAuth
    {
        public SessionAuth(IHttpContextAccessor httpContextAccessor)
        {
            StringValues value = string.Empty;
            httpContextAccessor?.HttpContext?.Request?.Headers?.TryGetValue("Authorization", out value);

            if (value.Count > 0)
            {
                Deserialize(value);
            }
        }

        public SessionAuth(string token)
        {
            Deserialize(token);
        }

        public string Email { get; private set; }
        public Guid? UserId { get; private set; }


        public void Deserialize(string token)
        {
            token = token.Replace("bearer ", string.Empty, StringComparison.InvariantCultureIgnoreCase);

            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            Email = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            UserId = new Guid(jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
        }
    }
}

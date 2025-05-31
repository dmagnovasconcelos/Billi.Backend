using System.Security.Cryptography;

namespace Billi.Backend.Domain.ValueObjects
{
    public class AccessToken(string token, string email, Guid userId, bool redefinePassword, DateTime created, DateTime expiration)
    {
        public DateTime CreatedAt { get; } = created;
        public DateTime Expiration { get; } = expiration;
        public string Email { get; } = email;
        public Guid UserId { get; } = userId;
        public bool RedefinePassword { get; } = redefinePassword;
        public string Token { get; } = token;
        public string RefreshToken { get; } = GenerateRefreshToken();

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
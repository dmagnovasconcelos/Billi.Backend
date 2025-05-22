namespace Billi.Backend.Domain.ValueObjects
{
    public class AccessToken(string token, bool redefinePassword, DateTime created, DateTime expiration)
    {
        public DateTime Created { get; } = created;
        public DateTime Expiration { get; } = expiration;
        public bool RedefinePassword { get; } = redefinePassword;
        public string Token { get; } = token;
    }
}

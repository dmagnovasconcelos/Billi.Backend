using Billi.Backend.Application.Services.Auth.Interfaces;
using Billi.Backend.CrossCutting.Configurations;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Billi.Backend.Application.Services.Auth
{
    public class TokenBlacklistService(IOptions<AuthenticationSettings> authConfiguration, IConnectionMultiplexer redis) : ITokenBlacklistService
    {
        private readonly AuthenticationSettings _authConfiguration = authConfiguration.Value;
        private readonly IDatabase _database = redis.GetDatabase();
        private const string _keyPrefix = "blacklist_token:";

        public Task AddTokenAsync(string token)
        {
            return _database.StringSetAsync(_keyPrefix + token, "true", TimeSpan.FromHours(_authConfiguration.Expiration));
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _database.KeyExistsAsync(_keyPrefix + token);
        }
    }
}

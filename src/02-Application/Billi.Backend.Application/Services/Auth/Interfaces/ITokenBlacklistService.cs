namespace Billi.Backend.Application.Services.Auth.Interfaces
{
    public interface ITokenBlacklistService
    {
        Task AddTokenAsync(string token);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }

}

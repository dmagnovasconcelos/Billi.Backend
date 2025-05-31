namespace Billi.Backend.CrossCutting.Auth
{
    public interface ICurrentUser
    {
        string Email { get; }
        Guid? UserId { get; }
        string Token { get; }
    }
}

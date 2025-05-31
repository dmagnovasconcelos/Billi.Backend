namespace Billi.Backend.Application.Commands.Auth
{
    public class ValidateResetPasswordCodeCommand(string email, string code) : ResetPasswordCommand(email)
    {
        public string Code { get; } = code;
    }
}

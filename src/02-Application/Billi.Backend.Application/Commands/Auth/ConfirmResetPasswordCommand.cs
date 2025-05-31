namespace Billi.Backend.Application.Commands.Auth
{
    public class ConfirmResetPasswordCommand(string email, string code, string newPassword) : ValidateResetPasswordCodeCommand(email, code)
    {
        public string NewPassword { get; } = newPassword;
    }
}

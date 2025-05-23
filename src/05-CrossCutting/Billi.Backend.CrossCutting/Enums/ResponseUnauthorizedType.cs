using System.ComponentModel;

namespace Billi.Backend.CrossCutting.Enums
{
    public enum ResponseUnauthorizedType
    {
        [Description("Unauthorized")]
        Unauthorized,

        [Description("User not found")]
        NotFound,

        [Description("Inactive user")]
        InactiveUser,

        [Description("Invalid password")]
        InvalidPassword,

        [Description("Invalid RefreshToken")]
        InvalidRefreshToken,

        [Description("Expired RefreshToken")]
        ExpiredRefreshToken,

        [Description("Revoked RefreshToken")]
        RevokedRefreshToken
    }
}

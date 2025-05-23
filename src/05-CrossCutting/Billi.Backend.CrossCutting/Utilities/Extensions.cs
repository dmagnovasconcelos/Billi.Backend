using System.ComponentModel;
using System.Reflection;

namespace Billi.Backend.CrossCutting.Utilities
{
    public static class Extensions
    {
        public static DescriptionAttribute GetDescription(this Enum enumValue)
        {
            try
            {
                return enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault()
                    .GetCustomAttribute<DescriptionAttribute>();
            }
            catch
            {
                return null;
            }
        }

        public static bool IsExpired(this DateTime ExpiresAt)
        {
            return DateTime.UtcNow >= ExpiresAt;
        }
    }
}

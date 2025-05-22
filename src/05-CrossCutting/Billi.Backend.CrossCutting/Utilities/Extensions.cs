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
                return enumValue.GetType().GetMember(enumValue.ToString()).First()
                    .GetCustomAttribute<DescriptionAttribute>();
            }
            catch
            {
                return null;
            }
        }
    }
}

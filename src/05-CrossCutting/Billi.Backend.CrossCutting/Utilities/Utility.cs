using System.ComponentModel;
using System.Reflection;

namespace Billi.Backend.CrossCutting.Utilities
{
    public static class Utility
    {
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly Random _random = new();

        public static string GenerateAlphaNumericCode(int length = 10)
        {
            return new string([.. Enumerable.Range(0, length).Select(_ => _chars[_random.Next(_chars.Length)])]);
        }

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
            return DateTime.Now >= ExpiresAt;
        }

        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    if (i > 0)
                        sb.Append('_');
                    sb.Append(char.ToLowerInvariant(input[i]));
                }
                else
                {
                    sb.Append(input[i]);
                }
            }
            return sb.ToString();
        }

        public static string ToSnakeCaseKeepPrefix(string input, string prefix)
        {
            if (string.IsNullOrWhiteSpace(input) || !input.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return ToSnakeCase(input);

            var withoutPrefix = input[prefix.Length..];

            var restSnake = ToSnakeCase(withoutPrefix);

            return prefix.ToLowerInvariant() + restSnake;
        }
    }
}
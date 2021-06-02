using System;

namespace Dreamfly.JavaEstateCodeGenerator.Helper
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string variable)
        {
            return ToCase(variable, () =>
                char.ToLowerInvariant(variable[0]) + variable.Substring(1)
            );
        }

        private static string ToCase(string variable, Func<string> convert)
        {
            if (String.IsNullOrWhiteSpace(variable))
            {
                return String.Empty;
            }

            return convert();
        }

        public static string ToPascalCase(this string variable)
        {
            return ToCase(variable, () =>
                char.ToUpperInvariant(variable[0]) + variable.Substring(1)
            );
        }

        public static string RemoveUnderLine(this string variable)
        {
            if (String.IsNullOrWhiteSpace(variable))
            {
                return String.Empty;
            }
            return variable.Replace("_", "");
        }

        public static T ToEnum<T>(this string value, T defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return Enum.TryParse(typeof(T), value, true, out var result) ? 
                (T) result 
                : defaultValue;
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
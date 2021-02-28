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
    }
}
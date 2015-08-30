using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Money
{
    /// <summary>
    /// Extension methods for strings.
    /// </summary>
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Determines whether [is all lower case] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool IsAllLowercase(this String value)
        {
            Argument.CheckIfNull(value, "value");
            return string.Equals(value.ToLower(CultureInfo.CurrentCulture), value, StringComparison.CurrentCulture);
        }

        public static bool IsNull(this String value)
        {
            return value == null;
        }

        public static bool IsNotEmpty(this String value)
        {
            return value.Any();
        }

        /// <summary>
        /// Base64 encodes the string.
        /// </summary>
        public static string Base64Encode(this string text)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Base64s decode the string
        /// </summary>
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Formats the specified value.
        /// </summary>
        public static string FormatWith(this String value, params object[] args)
        {
            Argument.CheckIfNull(value, "value");
            return string.Format(CultureInfo.CurrentCulture, value, args);
        }

        /// <summary>
        /// Converts string to lowercase.
        /// </summary>
        public static string ToLowercase(this String value)
        {
            Argument.CheckIfNull(value, "value");
            return value.ToLower(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Determines whether [is not null or white space] [the specified instance].
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        ///   <c>true</c> if [is not null or white space] [the specified instance]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNullOrWhiteSpace(this string instance)
        {
            return !instance.IsNullOrWhiteSpace();
        }

        /// <summary>
        /// Determines whether [is null or white space] [the specified instance].
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        ///   <c>true</c> if [is null or white space] [the specified instance]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string instance)
        {
            return string.IsNullOrWhiteSpace(instance);
        }

        /// <summary>
        /// Determines whether a string contans another string.
        /// </summary>
        public static bool Contains(this string source, string value, StringComparison compareMode)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            return source.IndexOf(value, compareMode) >= 0;
        }

        /// <summary>
        /// Trims the end off a string
        /// </summary>
        public static string TrimEnd(this string input, string suffixToRemove)
        {
            Argument.CheckIfNull(input, "input");
            Argument.CheckIfNull(suffixToRemove, "suffixToRemove");

            if (input != null && suffixToRemove != null &&
                input.EndsWith(suffixToRemove, StringComparison.CurrentCulture))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }

            return input;
        }

        /// <summary>
        /// Creates a single separated string out of a list of strings.
        /// </summary>
        public static string ToSeparated(this IEnumerable<string> source, string separator)
        {
            Argument.CheckIfNull(source, "source");
            Argument.CheckIfNull(separator, "separator");

            StringBuilder codes = new StringBuilder();
            foreach (string code in source)
            {
                codes.Append("{0}{1}".FormatWith(code.Trim(), separator));
            }

            return codes.ToString().TrimEnd(separator);
        }


        /// <summary>
        /// Creates a list of strings with leading and trailing quotes.
        /// </summary>
        public static IEnumerable<string> ToStringsWithLeadingAndTrailingChar(this IEnumerable<string> source, string sep)
        {
            Argument.CheckIfNull(source, "source");
            return source.Select(s => sep + s + sep);
        }

        /// <summary>
        /// Creates a single separated string out of a list of strings.
        /// </summary>
        public static IEnumerable<string> ToSplit(this string source, string separator)
        {
            Argument.CheckIfNull(source, "source");
            Argument.CheckIfNull(separator, "separator");

            if (string.IsNullOrEmpty(source))
            {
                return Enumerable.Empty<string>();
            }

            var strings = source.Split(separator.ToCharArray());
            return strings;
        }

        /// <summary>
        /// Removes special characters from the string
        /// </summary>
        public static string RemoveLineBreaksAndCarriageReturnCharacters(this string input)
        {
            Argument.CheckIfNull(input, "input");
            return input.Replace("\n", string.Empty).Replace("\r", string.Empty);
        }

        /// <summary>
        /// returns an empty string if null
        /// </summary>
        public static string ToEmptyStringIfNull(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return input;
        }
    }
}

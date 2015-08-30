using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money
{
    /// <summary>
    /// Provides extension methods for mathematical expressions.
    /// </summary>
    public static class Maths
    {
        /// <summary>
        /// The maximum parse length of a decimal.
        /// </summary>
        private const int MaxParseLengthOfDecimal = 28;

        /// <summary>
        /// Returns true if the given text can be converted to a decimal. False otherwise. If the given
        /// text is longer than 29 characters which is close to the maximum size of a decimal it will be truncated.
        /// to 29 characters. This is a conversion that is inteneded for hanlding user input.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryParseDecimal(string text, out decimal value)
        {
            if (string.IsNullOrEmpty(text))
            {
                value = 0;
                return true;
            }

            text = text.Substring(0, Math.Min(text.Length, MaxParseLengthOfDecimal));
            return Decimal.TryParse(text, out value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Money
{
    /// <summary>
    /// Represents the system of money in use for a particular country
    /// </summary>
    public class Currency
    {
        private readonly string _code;
        private static Currency _default;
        private const int _decimalPlaces = 2;

        /// <summary>
        /// The constructor of a <see cref="Currency" />
        /// </summary>
        /// <param name="code">The code should be in the format languagecode2-country/regioncode2 from <see cref="CultureInfo.Name"/>.</param>
        public Currency(string code)
        {
            Argument.CheckIfNullOrEmpty(code, "code");

            _code = code;
        }

        /// <summary>
        /// Gets the default currency.
        /// </summary>
        public static Currency DefaultCurrency
        {
            get { return _default ?? (_default = new Currency(CultureInfo.CurrentCulture.Name)); }
        }

        /// <summary>
        /// Gets the currency symbol.
        /// </summary>
        public string CurrencySymbol
        {
            get
            {
                RegionInfo regionInfo = new RegionInfo(_code);
                return regionInfo.CurrencySymbol;
            }
        }

        /// <summary>
        /// Formats the specified amount using this currency
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public string Format(decimal amount)
        {
            decimal roundedAmount = Math.Round(amount, _decimalPlaces, MidpointRounding.AwayFromZero);

            NumberFormatInfo localNumberFormatter = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            localNumberFormatter.CurrencySymbol = CurrencySymbol;

            return string.Format(localNumberFormatter, "{0:C}", roundedAmount);
        }

        /// <summary>
        /// The currency code.
        /// </summary>
        public string Code
        {
            get { return _code; }
        }

        /// <summary>
        /// Returns true if the currency values are equal
        /// </summary>
        public static bool operator ==(Currency currency1, Currency currency2)
        {
            return Equals(currency1, currency2);
        }

        /// <summary>
        /// Returns true if the currency values are not equal
        /// </summary>
        public static bool operator !=(Currency currency1, Currency currency2)
        {
            return !(currency1 == currency2);
        }

        /// <summary>
        /// Returns true if the object provided is a <see cref="Currency"/> and the id and code are the same
        /// </summary>
        public override bool Equals(object obj)
        {
            Currency other = obj as Currency;
            if (other == null)
            {
                return false;
            }
            return Code == other.Code;
        }

        ///<summary>
        /// Serves as a hash function for a particular type. 
        ///</summary>
        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}

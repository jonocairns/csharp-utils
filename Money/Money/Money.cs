using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Money
{
    /// <summary>
    /// Represents money
    /// </summary>
    public struct Money : IComparable<Money>, IEquatable<Money>, IComparable
    {
        private readonly Currency _currency;
        private readonly decimal _amount;

        /// <summary>
        /// Gets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public Currency Currency
        {
            get { return _currency; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is negative.
        /// </summary>
        public bool IsNegative
        {
            get { return _amount < 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is zero.
        /// </summary>        
        public bool IsZero
        {
            get { return _amount == 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is greater than zero.
        /// </summary>
        public bool IsGreaterThanZero
        {
            get { return _amount > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is less than zero.
        /// </summary>
        public bool IsLessThanOrEqualToZero
        {
            get { return _amount <= 0; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> struct.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        public Money(decimal amount, Currency currency)
        {
            Argument.CheckIfNull(currency, "currency");

            //4 decimal places matches precision in the DB don't change this to 5
            _amount = Math.Round(amount, 4, MidpointRounding.AwayFromZero);
            _currency = currency;
        }


        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="money1">The money1.</param>
        /// <param name="money2">The money2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Money money1, Money money2)
        {
            return money1.Equals(money2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="money1">The money1.</param>
        /// <param name="money2">The money2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Money money1, Money money2)
        {
            return !(money1 == money2);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="firstMoney">The first money.</param>
        /// <param name="secondMoney">The second money.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator +(Money firstMoney, Money secondMoney)
        {
            EnforceMoneyInSameCurrency(firstMoney, secondMoney);
            return new Money(firstMoney._amount + secondMoney._amount, firstMoney.Currency);
        }

        private static bool CurrenciesMatch(Money first, Money second)
        {
            return first.Currency == second.Currency;
        }

        private static void EnforceMoneyInSameCurrency(Money first, Money second)
        {
            if (!CurrenciesMatch(first, second))
            {
                throw new InvalidOperationException("Money operation cannot be performed as currencies do not match {0}, {1}".FormatWith(first.Currency, second.Currency));
            }
        }

        /// <summary>
        /// Returns the Absolute value of the money object.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static Money Abs(Money money)
        {
            decimal absMoney = Math.Abs(money.ToDecimal());
            return new Money(absMoney, money.Currency);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="addAmount">The add amount.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator +(Money money, int addAmount)
        {
            return new Money(money._amount + addAmount, money.Currency);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="firstMoney">The first money.</param>
        /// <param name="secondMoney">The second money.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator -(Money firstMoney, Money secondMoney)
        {
            EnforceMoneyInSameCurrency(firstMoney, secondMoney);
            return new Money(firstMoney._amount - secondMoney._amount, firstMoney.Currency);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator *(Money money, int multiplier)
        {
            return new Money(money._amount * multiplier, money.Currency);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator *(Money money, decimal multiplier)
        {
            return new Money(money._amount * multiplier, money.Currency);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator *(Money money, double multiplier)
        {
            return new Money(money._amount * (decimal)multiplier, money.Currency);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="divider">The divider.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator /(Money money, int divider)
        {
            return new Money(money._amount, money.Currency).Divide(divider);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="divider">The divider.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator /(Money money, decimal divider)
        {
            return new Money(money._amount, money.Currency).Divide(divider);
        }


        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="divider">The divider.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator /(Money money, double divider)
        {
            return new Money(money._amount, money.Currency).Divide((decimal)divider);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="money1">The money1.</param>
        /// <param name="money2">The money2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Money money1, Money money2)
        {
            if (CurrenciesMatch(money1, money2))
            {
                return (money1._amount < money2._amount);
            }
            return false;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="money1">The money1.</param>
        /// <param name="money2">The money2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Money money1, Money money2)
        {
            if (CurrenciesMatch(money1, money2))
            {
                return (money1._amount > money2._amount);
            }
            return false;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="money1">The money1.</param>
        /// <param name="money2">The money2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Money money1, Money money2)
        {
            if (CurrenciesMatch(money1, money2))
            {
                return (money1._amount <= money2._amount);
            }
            return false;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="money1">The money1.</param>
        /// <param name="money2">The money2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Money money1, Money money2)
        {
            if (CurrenciesMatch(money1, money2))
            {
                return (money1._amount >= money2._amount);
            }
            return false;
        }

        /// <summary>
        /// Divides the specified divider.
        /// </summary>
        /// <param name="divider">The divider.</param>
        /// <returns></returns>
        public Money Divide(int divider)
        {
            if (divider == 0)
            {
                throw new DivideByZeroException("Money cannot be divided by zero");
            }
            return new Money(_amount / divider, Currency);
        }

        /// <summary>
        /// Divides the specified divider.
        /// </summary>
        /// <param name="divider">The divider.</param>
        /// <returns></returns>
        public Money Divide(decimal divider)
        {
            if (divider == 0)
            {
                throw new DivideByZeroException("Money cannot be divided by zero");
            }
            return new Money(_amount / divider, Currency);
        }

        /// <summary>
        /// Multiplies the specified multiplier.
        /// </summary>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns></returns>
        public Money Multiply(int multiplier)
        {
            return new Money(_amount * multiplier, Currency);
        }

        /// <summary>
        /// Multiplies the specified multiplier.
        /// </summary>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns></returns>
        public Money Multiply(decimal multiplier)
        {
            return new Money(_amount * multiplier, Currency);
        }

        /// <summary>
        /// Returns the total amount of GST that should be payed for this monetary amount based on
        /// the given GST rate.
        /// </summary>
        public Money GstToPay(Gst gstRate)
        {
            decimal value = gstRate.ToDecimal() / 100;
            return Multiply(value);
        }

        /// <summary>
        /// Returns the GST portion of the <see cref="Money"/> that has been paid given a GST rate.
        /// </summary>
        /// <param name="gstRate">The GST rate.</param>
        public Money GstPaid(Gst gstRate)
        {
            return Multiply(gstRate.GstFraction / gstRate.Multiplier);
        }

        /// <summary>
        /// Adds the GST portion to the original money at the GSTRate provided.
        /// </summary>
        public Money Add(Gst gstRate)
        {
            return Multiply(gstRate.Multiplier);
        }

        /// <summary>
        /// Adds the specified money.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public Money Add(Money money)
        {
            return this + money;
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return _currency.Format(_amount);
        }

        /// <summary>
        /// Subtracts the specified money.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public Money Subtract(Money money)
        {
            return this - money;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Money)
            {
                return Equals((Money)obj);
            }
            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Money other)
        {
            if (CurrenciesMatch(this, other))
            {
                return (_amount == other._amount);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() + _amount.GetHashCode();
        }

        /// <summary>
        /// Parse a numeric string representing a unit of money
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if the string amount cannot be parsed to a Money
        /// </exception>
        public static Money Parse(string amount, Currency currency)
        {
            Argument.CheckIfNull(currency, "currency");

            if (string.IsNullOrEmpty(amount))
            {
                return new Money(0, currency);
            }

            string unformattedText = amount.Replace(currency.CurrencySymbol, "");

            decimal parsedValue;

            if (Maths.TryParseDecimal(unformattedText, out parsedValue))
            {
                return new Money(parsedValue, currency);
            }

            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The amount {0} is not a monetary value", amount));
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(Money other)
        {
            EnforceMoneyInSameCurrency(this, other);
            return _amount.CompareTo(other._amount);
        }

        /// <summary>
        /// Returns the money value.
        /// </summary>
        /// <returns></returns>
        public decimal ToDecimal()
        {
            return _amount;
        }

        /// <summary>
        /// Returns the decimal value of money value to 2 decimal places.
        /// </summary>
        /// <returns></returns>
        public decimal ToRoundedDecimal()
        {
            return Math.Round(_amount, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Returns the money value to 2 decimal places.
        /// </summary>
        /// <returns></returns>
        public Money ToRoundedMoney()
        {
            return new Money(ToRoundedDecimal(), Currency);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This instance is less than <paramref name="obj"/>.
        /// Zero
        /// This instance is equal to <paramref name="obj"/>.
        /// Greater than zero
        /// This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="obj"/> is not the same type as this instance.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is Money)
            {
                return CompareTo((Money)obj);
            }
            throw new ArgumentException("obj is not the same type as this instance.", "obj");
        }

        /// <summary>
        /// Returns a numeric string representation of the money object.
        /// </summary>
        /// <returns></returns>
        public string ToNumericString()
        {
            return _amount.ToString(CultureInfo.CurrentCulture);
        }
    }

    /// <summary>
    /// Extension methods for Linq queries for the <see cref="Money"/> class
    /// </summary>
    public static class MoneyExtensionMethods
    {
        /// <summary>
        /// Sums the items in the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Money Sum(this IEnumerable<Money> source)
        {
            Money result = source.First();
            return source.Skip(1).Aggregate(result, (current, money) => current.Add(money));
        }

        /// <summary>
        /// Sums the items in the specified source.
        /// </summary>
        public static Money Sum(this IEnumerable<Money> source, Currency currency)
        {
            Money result = new Money(0, currency);
            return source.Aggregate(result, (current, money) => current.Add(money));
        }

        /// <summary>
        /// Sums the items in the specified source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static Money Sum<T>(this IEnumerable<T> source, Func<T, Money> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Sums the items in the specified source.
        /// </summary>
        public static Money Sum<T>(this IEnumerable<T> source, Func<T, Money> selector, Currency currency)
        {
            return source.Select(selector).Sum(currency);
        }

    }
}

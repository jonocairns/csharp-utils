using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

namespace Money.Test
{
    [TestFixture]
    public class MoneyTest
    {
        [Test]
        [Ignore]
        public void TextReutrnsAmountFormattedTo2Dp()
        {
            Currency currency = CurrencyFixture.NewZealandDollar;

            new Money(55.66666666m, currency).ToString().Should().Be("$55.67");
            new Money(55, currency).ToString().Should().Be("$55.00");
            new Money(64.11111111m, currency).ToString().Should().Be("$64.11");
            new Money(33.3m, currency).ToString().Should().Be("$33.30");

            string formattedNegativeAmount = new Money(-55.66666666m, currency).ToString();
            if (formattedNegativeAmount.Contains("-"))
            {
                new Money(-55.66666666m, currency).ToString().Should().Be("-$55.67");
                new Money(-55, currency).ToString().Should().Be("-$55.00");
                new Money(-64.11111111m, currency).ToString().Should().Be("-$64.11");
                new Money(-33.3m, currency).ToString().Should().Be("-$33.30");
            }
            else
            {
                new Money(-55.66666666m, currency).ToString().Should().Be("($55.67)");
                new Money(-55, currency).ToString().Should().Be("($55.00)");
                new Money(-64.11111111m, currency).ToString().Should().Be("($64.11)");
                new Money(-33.3m, currency).ToString().Should().Be("($33.30)");
            }
        }

        [Test]
        public void CreateWithDecimalAmountShouldRoundTo4DecimalPlaces()
        {
            Money money = new Money(1.12345m, CurrencyFixture.NewZealandDollar);

            money.ToDecimal().Should().Be(1.1235m);
        }

        [Test]
        public void CreatingMoneyWithNegativeAmountReturnsMoney()
        {
            Assert.IsNotNull(new Money(-5, CurrencyFixture.NewZealandDollar));
        }

        [Test]
        public void ToStringReturnsFormattedMoneyAmount()
        {
            const decimal amount = (decimal)45.87;
            Money money = new Money(amount, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual("$45.87", money.ToString());
        }

        [Test]
        public void ToStringRoundsDownCorrectly()
        {
            const decimal amount = (decimal)45.8741;
            Money money = new Money(amount, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual("$45.87", money.ToString());
        }

        [Test]
        public void ToStringRoundsUpCorrectly()
        {
            const decimal amount = (decimal)45.8781;
            Money money = new Money(amount, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual("$45.88", money.ToString());
        }

        [Test]
        public void EqualsMoney()
        {
            Money money1 = new Money((decimal)10.05, CurrencyFixture.NewZealandDollar);
            Money money2 = new Money((decimal)10.05, CurrencyFixture.NewZealandDollar);
            Money money3 = new Money((decimal)10.05, CurrencyFixture.NewZealandDollar);

            Assert.IsTrue(money1.Equals(money1));

            Assert.IsTrue(money1.Equals(money2));
            Assert.IsTrue(money2.Equals(money1));
            Assert.IsTrue(money1.Equals(money2) && money2.Equals(money3));

            Assert.IsFalse(money1.Equals(null));
        }

        [Test]
        public void MultipliedByZeroReturnsNewMoneyWithAmountZero()
        {
            Money beforeMarriage = new Money(1000, CurrencyFixture.NewZealandDollar);
            Money afterMarriage = new Money(0, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(afterMarriage, beforeMarriage * 0);
        }

        [Test]
        public void CanMultiplyByPositiveIntSuccessfully()
        {
            Money beforeLoan = new Money(10, CurrencyFixture.NewZealandDollar);
            Money afterLoan = new Money(100, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(afterLoan, beforeLoan * 10);
        }

        [Test]
        public void CanMultiplyByPositiveDecimalSuccessfully()
        {
            Money beforeLoan = new Money(10, CurrencyFixture.NewZealandDollar);
            Money afterLoan = new Money(105, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(afterLoan, beforeLoan * 10.5);
            Assert.AreEqual(beforeLoan * 10.5, afterLoan);
        }

        [Test]
        public void AddMoneyUsingOperatorOverloadCalculatesCorrectly()
        {
            Money five = new Money(5, CurrencyFixture.NewZealandDollar);
            Money ten = new Money(10, CurrencyFixture.NewZealandDollar);
            Money expectedResult = new Money(15, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expectedResult, five + ten);
        }

        [Test]
        public void SubtractMoneyWithSubtractMethodCalculatesCorrectly()
        {
            Money five = new Money(5, CurrencyFixture.NewZealandDollar);
            Money two = new Money(2, CurrencyFixture.NewZealandDollar);
            Money expectedResult = new Money(3, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expectedResult, (five - two));
        }

        [Test]
        public void SubtractMoneyUsingOperatorOverloadCalculatesCorrectly()
        {
            Money ten = new Money(10, CurrencyFixture.NewZealandDollar);
            Money five = new Money(5, CurrencyFixture.NewZealandDollar);
            Money expectedResult = new Money(5, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expectedResult, ten - five);
        }

        [Test]
        public void DecimalMultipliesMoneyUsingOperatorOverload()
        {
            Money five = new Money(10, CurrencyFixture.NewZealandDollar);
            const decimal multiplier = (decimal)1.125;
            Money expected = new Money((decimal)11.25, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, five * multiplier);
        }

        [Test]
        public void DecimalMultipliesMoneyUsingMultiplyMethod()
        {
            Money five = new Money(10, CurrencyFixture.NewZealandDollar);
            const decimal multiplier = (decimal)1.125;
            Money expected = new Money((decimal)11.25, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, five.Multiply(multiplier));
        }

        [Test]
        public void CanDivideMoneyByANegativeInteger()
        {
            Money money = new Money(10, CurrencyFixture.NewZealandDollar);
            const int negativeDivider = -2;
            Money expectedMoney = new Money(-5, CurrencyFixture.NewZealandDollar);
            money.Divide(negativeDivider).Should().Be(expectedMoney);
        }

        [Test]
        public void CanDivideMoneyByANegativeDecimal()
        {
            Money money = new Money(10, CurrencyFixture.NewZealandDollar);
            const decimal negativeDivider = -2;
            Money expectedMoney = new Money(-5, CurrencyFixture.NewZealandDollar);
            money.Divide(negativeDivider).Should().Be(expectedMoney);
        }

        [Test]
        public void CanBeDividedByIntUsingDivideMethod()
        {
            Money ten = new Money(10, CurrencyFixture.NewZealandDollar);
            const int divider = 2;
            Money expected = new Money(5, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, ten.Divide(divider));
        }

        [Test]
        public void CanBeDividedByIntUsingOperatorOverload()
        {
            Money ten = new Money(10, CurrencyFixture.NewZealandDollar);
            const int divider = 2;
            Money expected = new Money(5, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, ten / divider);
        }

        [Test]
        public void TenIsLessThanMoneyTwentyReturnsTrue()
        {
            Assert.IsTrue(new Money(10, CurrencyFixture.NewZealandDollar) < new Money(20, CurrencyFixture.NewZealandDollar));
        }

        [Test]
        public void TenIsGreaterThanMoneyTwentyReturnsFalse()
        {
            Assert.IsFalse(new Money(10, CurrencyFixture.NewZealandDollar) > new Money(20, CurrencyFixture.NewZealandDollar));
        }

        [Test]
        public void Money1CompareToMoney2ReturnsTrueIfMoney1IsLessThanMoney2()
        {
            Money money1 = new Money(10, CurrencyFixture.NewZealandDollar);
            Money money2 = new Money(20, CurrencyFixture.NewZealandDollar);
            Assert.IsTrue((money1.CompareTo(money2)) < 0);
        }

        [Test]
        public void Money1CompareToMoney2ReturnsFalseIfMoney1IsLessThanMoney2()
        {
            Money money1 = new Money(20, CurrencyFixture.NewZealandDollar);
            Money money2 = new Money(10, CurrencyFixture.NewZealandDollar);
            Assert.IsFalse((money1.CompareTo(money2)) < 0);
        }

        [Test]
        public void Money1CompareToMoney2ReturnsZeroIfMoney1AndMoney2AreTheSame()
        {
            Money money1 = new Money(10, CurrencyFixture.NewZealandDollar);
            Money money2 = new Money(10, CurrencyFixture.NewZealandDollar);
            Assert.IsTrue((money1.CompareTo(money2)) == 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void MoneyComparedToOtherTypeThrowsArgumentException()
        {
            new MoneyBuilder().Build().CompareTo(new object());
        }

        [Test]
        public void EqualsMoneyReturnsTrueIsMoneyAmountIsTheSameUsingOperatorOverload()
        {
            Money money1 = new Money(10, CurrencyFixture.NewZealandDollar);
            Money money2 = new Money(10, CurrencyFixture.NewZealandDollar);
            Assert.IsTrue(money1 == money2);
        }

        [Test]
        public void ToDecimalReturnsDecimalFromMoneyAmount()
        {
            Money theMoney = new Money(10, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(10m, theMoney.ToDecimal());
        }

        [Test]
        public void ToNumericStringReturnsStringRepresentationOfAmountWithNoFormatting()
        {
            const decimal expected = 10.56m;
            Money money = new Money(expected, CurrencyFixture.NewZealandDollar);

            money.ToNumericString().Should().Be(expected.ToString());
        }

        [Test]
        public void ParseNullStringReturnsZero()
        {
            Money.Parse(null, CurrencyFixture.NewZealandDollar)
                .Should()
                .Be(new Money(0m, CurrencyFixture.NewZealandDollar));
        }

        [Test]
        public void ParseEmptyStringReturnsZero()
        {
            Money.Parse(string.Empty, CurrencyFixture.NewZealandDollar)
                .Should()
                .Be(new Money(0, CurrencyFixture.NewZealandDollar));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseInvalidStringThrowsException()
        {
            Money.Parse("invalidMoney", CurrencyFixture.NewZealandDollar);
        }

        [Test]
        public void ParseReturnsParsedAmountWhenInputIsValidDecimal()
        {
            const string parseThis = "10.54";
            Money expected = new Money((decimal)10.54, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, Money.Parse(parseThis, CurrencyFixture.NewZealandDollar));
        }

        [Test]
        public void ParseReturnParsedAmountWhenInputIsValidFormattedString()
        {
            const string parseThis = "$1,054.45";
            Money expected = new Money((decimal)1054.45, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, Money.Parse(parseThis, CurrencyFixture.NewZealandDollar));
        }

        [Test]
        public void MoneyIsNotEqualToNull()
        {
            Money money = new MoneyBuilder().Build();
            money.Should().NotBeNull();
        }

        [Test]
        public void MoneysAreReflexive()
        {
            Money money = new MoneyBuilder().Build();
            money.Should().Be(money);
        }

        [Test]
        public void MoneysAreSymmetric()
        {
            Money moneyA = new MoneyBuilder().Build();
            Money moneyB = new MoneyBuilder().Build();

            moneyA.Should().Be(moneyB);
            moneyB.Should().Be(moneyA);
        }

        [Test]
        public void MoneysAreTransitive()
        {
            Money moneyA = new MoneyBuilder().Build();
            Money moneyB = new MoneyBuilder().Build();
            Money moneyC = new MoneyBuilder().Build();

            moneyA.Should().Be(moneyB);
            moneyB.Should().Be(moneyC);
            moneyA.Should().Be(moneyC);
        }

        [Test]
        public void ToRoundedDecimalReturnsTheValueToTwoDecimalPlaces()
        {
            Money money = new Money(12.567m, CurrencyFixture.NewZealandDollar);
            money.ToRoundedDecimal().Should().Be(12.57m);
        }

        [Test]
        public void IsNegativeaReturnsTrueWhenAmountIsLessThanZero()
        {
            Money money = new Money(-1, CurrencyFixture.AustralianDollar);
            money.IsNegative.Should().Be(true);
        }

        [Test]
        public void IsNegativeReturnsFalseWhenAmountIsZero()
        {
            Money zeroMoney = new Money(0, CurrencyFixture.AustralianDollar);
            zeroMoney.IsNegative.Should().Be(false);
        }

        [Test]
        public void IsNegativeReturnsFalseWhenAmountIsMoreThanZero()
        {
            Money zeroMoney = new Money(7, CurrencyFixture.AustralianDollar);
            zeroMoney.IsNegative.Should().Be(false);
        }

        [Test]
        public void IsZeroReturnsFalseWhenAmountIsGreaterThanZero()
        {
            Money zeroMoney = new Money(7, CurrencyFixture.AustralianDollar);
            zeroMoney.IsZero.Should().Be(false);
        }

        [Test]
        public void IsZeroReturnsTrueWhenAmountIsEqualtToZero()
        {
            Money zeroMoney = new Money(0, CurrencyFixture.AustralianDollar);
            zeroMoney.IsZero.Should().Be(true);
        }

        [Test]
        public void IsGreaterThanZeroReturnsFalseWhenAmountIsZero()
        {
            Money zeroMoney = new Money(0, CurrencyFixture.AustralianDollar);
            zeroMoney.IsGreaterThanZero.Should().Be(false);
        }

        [Test]
        public void IsGreaterThanZeroReturnsTrueWhenAmountIsGreaterThanZero()
        {
            Money zeroMoney = new Money(7, CurrencyFixture.AustralianDollar);
            zeroMoney.IsGreaterThanZero.Should().Be(true);
        }

        [Test]
        public void IsLessThanOrEqualToZeroReturnsTrueWhenAmountIsZero()
        {
            Money zeroMoney = new Money(0, CurrencyFixture.AustralianDollar);
            zeroMoney.IsLessThanOrEqualToZero.Should().BeTrue();
        }

        [Test]
        public void IsLessThanOrEqualToZeroReturnsTrueWhenAmountIsLessThanZero()
        {
            Money zeroMoney = new Money(-7, CurrencyFixture.AustralianDollar);
            zeroMoney.IsLessThanOrEqualToZero.Should().BeTrue();
        }

        [Test]
        public void IsLessThanOrEqualToZeroReturnsFalseWhenAmountIsGreaterThanZero()
        {
            Money zeroMoney = new Money(7, CurrencyFixture.AustralianDollar);
            zeroMoney.IsLessThanOrEqualToZero.Should().BeFalse();
        }

        [Test]
        public void EqualsReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreNotEqual(aud, nzd);
        }

        [Test]
        public void EqualsMethodReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            aud.Equals(nzd).Should().Be(false);
        }


        [Test]
        public void NotEqualReturnsTrueWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreNotEqual(aud, nzd);
        }

        [Test]
        public void LessThanReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(10.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreEqual(aud < nzd, false);
        }

        [Test]
        public void GreaterThanReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(14.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreEqual(aud > nzd, false);
        }

        [Test]
        public void LessThanOrEqualsReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(10.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreEqual(aud <= nzd, false);
        }

        [Test]
        public void GreaterThanOrEqualsReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(14.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreEqual(aud >= nzd, false);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddingAudToNzdThrowsException()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Money result = aud + nzd;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubtractingNzdFromAudThrowsException()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Money result = aud - nzd;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddingNzdToAudViaMethodThrowsException()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Money result = aud.Add(nzd);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubtractingNzdFromAudViaMethodThrowsException()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Money result = aud.Subtract(nzd);
        }


        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CompareThrowsExceptionWhenCurrenciesDontMatch()
        {
            Money pounds = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money yen = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            int compare = pounds.CompareTo(yen);
        }

        [Test]
        public void AddGstReturnsOriginalMoneyIncreasedByGSTAmount()
        {
            Money money = new Money(1900.5m, CurrencyFixture.NewZealandDollar);
            Gst gst = new Gst(12.5m);
            Money expected = money.Multiply(gst.Multiplier);

            Money actual = money.Add(gst);

            actual.Should().Be(expected);
        }

        [Test]
        public void SubtractGstReturnsOriginalMoneyIncreasedByGSTAmount()
        {
            Money money = new MoneyBuilder().WithPrice(1900.5m).Build();
            Gst gst = new Gst(12.5m);
            Money expected = money.Multiply(gst.GstFraction / gst.Multiplier);

            Money actual = money.GstPaid(gst);

            actual.Should().Be(expected);
        }

        [Test]
        public void AbsReturnsPositiveMoneyFromNegativeMoney()
        {
            Money moneyPositive = new Money(1900.5m, CurrencyFixture.NewZealandDollar);
            Money moneyNegative = new Money(-1900.5m, CurrencyFixture.NewZealandDollar);

            Money.Abs(moneyNegative).Should().Be(moneyPositive);
        }

        [Test]
        public void AbsReturnsPositiveMoneyFromPositiveMoney()
        {
            Money moneyPositive = new Money(1900.5m, CurrencyFixture.NewZealandDollar);

            Money.Abs(moneyPositive).Should().Be(moneyPositive);
        }

        [Test]
        public void SumWorks()
        {
            IEnumerable<Money> items = new[] { 
                new MoneyBuilder().WithPrice(20.1m).Build(),
                new MoneyBuilder().WithPrice(30.1m).Build() 
            };

            items.Sum(item => item.ToDecimal()).Should().Be(50.2m);
        }

        [Test]
        public void SumWithSelectorWorks()
        {
            var itemWithPrice1 = new { Price = new MoneyBuilder().WithPrice(10m).Build() };
            var itemWithPrice2 = new { Price = new MoneyBuilder().WithPrice(3m).Build() };
            var itemList = new[] { itemWithPrice1, itemWithPrice2 };

            itemList.Sum(item => item.Price.ToDecimal()).Should().Be(new MoneyBuilder().WithPrice(13).Build().ToDecimal());
        }

        [Test]
        public void SumWithCollectionWithOneItemWorks()
        {
            IEnumerable<Money> items = new[] { new MoneyBuilder().WithPrice(20.1m).Build() };

            items.Sum(item => item.ToDecimal()).Should().Be(new MoneyBuilder().WithPrice(20.1m).Build().ToDecimal());
        }
    }
}

[TestClass]
    public class MoneyTest
    {
        [TestMethod]
        [Ignore]
        public void TextReutrnsAmountFormattedTo2Dp ()
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

        [TestMethod]
        public void CreateWithDecimalAmountShouldRoundTo4DecimalPlaces()
        {
            Money money = new Money(1.12345m, CurrencyFixture.NewZealandDollar);

            money.ToDecimal().Should().Be(1.1235m);
        }

        [TestMethod]
        public void CreatingMoneyWithNegativeAmountReturnsMoney()
        {
            Assert.IsNotNull(new Money(-5, CurrencyFixture.NewZealandDollar));
        }

        [TestMethod]
        public void ToStringReturnsFormattedMoneyAmount()
        {
            const decimal amount = (decimal)45.87;
            Money money = new Money(amount, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual("$45.87", money.ToString());
        }

        [TestMethod]
        public void ToStringRoundsDownCorrectly()
        {
            const decimal amount = (decimal)45.8741;
            Money money = new Money(amount, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual("$45.87", money.ToString());
        }

        [TestMethod]
        public void ToStringRoundsUpCorrectly()
        {
            const decimal amount = (decimal)45.8781;
            Money money = new Money(amount, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual("$45.88", money.ToString());
        }

        [TestMethod]
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

        [TestMethod]
        public void MultipliedByZeroReturnsNewMoneyWithAmountZero()
        {
            Money beforeMarriage = new Money(1000, CurrencyFixture.NewZealandDollar);
            Money afterMarriage = new Money(0, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(afterMarriage, beforeMarriage * 0);
        }

        [TestMethod]
        public void CanMultiplyByPositiveIntSuccessfully()
        {
            Money beforeLoan = new Money(10, CurrencyFixture.NewZealandDollar);
            Money afterLoan = new Money(100, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(afterLoan, beforeLoan * 10);
        }

        [TestMethod]
        public void CanMultiplyByPositiveDecimalSuccessfully()
        {
            Money beforeLoan = new Money(10, CurrencyFixture.NewZealandDollar);
            Money afterLoan = new Money(105, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(afterLoan, beforeLoan * 10.5);
            Assert.AreEqual(beforeLoan * 10.5, afterLoan);
        }

        [TestMethod]
        public void AddMoneyUsingOperatorOverloadCalculatesCorrectly()
        {
            Money five = new Money(5, CurrencyFixture.NewZealandDollar);
            Money ten = new Money(10, CurrencyFixture.NewZealandDollar);
            Money expectedResult = new Money(15, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expectedResult, five + ten);
        }

        [TestMethod]
        public void SubtractMoneyWithSubtractMethodCalculatesCorrectly()
        {
            Money five = new Money(5, CurrencyFixture.NewZealandDollar);
            Money two = new Money(2, CurrencyFixture.NewZealandDollar);
            Money expectedResult = new Money(3, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expectedResult, (five - two));
        }

        [TestMethod]
        public void SubtractMoneyUsingOperatorOverloadCalculatesCorrectly()
        {
            Money ten = new Money(10, CurrencyFixture.NewZealandDollar);
            Money five = new Money(5, CurrencyFixture.NewZealandDollar);
            Money expectedResult = new Money(5, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expectedResult, ten - five);
        }

        [TestMethod]
        public void DecimalMultipliesMoneyUsingOperatorOverload()
        {
            Money five = new Money(10, CurrencyFixture.NewZealandDollar);
            const decimal multiplier = (decimal)1.125;
            Money expected = new Money((decimal)11.25, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, five * multiplier);
        }

        [TestMethod]
        public void DecimalMultipliesMoneyUsingMultiplyMethod()
        {
            Money five = new Money(10, CurrencyFixture.NewZealandDollar);
            const decimal multiplier = (decimal)1.125;
            Money expected = new Money((decimal)11.25, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, five.Multiply(multiplier));
        }

        [TestMethod]
        public void CanDivideMoneyByANegativeInteger()
        {
            Money money = new Money(10, CurrencyFixture.NewZealandDollar);
            const int negativeDivider = -2;
            Money expectedMoney = new Money(-5, CurrencyFixture.NewZealandDollar);            
            money.Divide(negativeDivider).Should().Be(expectedMoney);
        }

        [TestMethod]
        public void CanDivideMoneyByANegativeDecimal()
        {
            Money money = new Money(10, CurrencyFixture.NewZealandDollar);
            const decimal negativeDivider = -2;
            Money expectedMoney = new Money(-5, CurrencyFixture.NewZealandDollar);
            money.Divide(negativeDivider).Should().Be(expectedMoney);
        }

        [TestMethod]
        public void CanBeDividedByIntUsingDivideMethod()
        {
            Money ten = new Money(10, CurrencyFixture.NewZealandDollar);
            const int divider = 2;
            Money expected = new Money(5, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, ten.Divide(divider));
        }

        [TestMethod]
        public void CanBeDividedByIntUsingOperatorOverload()
        {
            Money ten = new Money(10, CurrencyFixture.NewZealandDollar);
            const int divider = 2;
            Money expected = new Money(5, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, ten / divider);
        }

        [TestMethod]
        public void TenIsLessThanMoneyTwentyReturnsTrue()
        {
            Assert.IsTrue(new Money(10, CurrencyFixture.NewZealandDollar) < new Money(20, CurrencyFixture.NewZealandDollar));
        }

        [TestMethod]
        public void TenIsGreaterThanMoneyTwentyReturnsFalse()
        {
            Assert.IsFalse(new Money(10, CurrencyFixture.NewZealandDollar) > new Money(20, CurrencyFixture.NewZealandDollar));
        }

        [TestMethod]
        public void Money1CompareToMoney2ReturnsTrueIfMoney1IsLessThanMoney2()
        {
            Money money1 = new Money(10, CurrencyFixture.NewZealandDollar);
            Money money2 = new Money(20, CurrencyFixture.NewZealandDollar);
            Assert.IsTrue((money1.CompareTo(money2)) < 0);
        }

        [TestMethod]
        public void Money1CompareToMoney2ReturnsFalseIfMoney1IsLessThanMoney2()
        {
            Money money1 = new Money(20, CurrencyFixture.NewZealandDollar);
            Money money2 = new Money(10, CurrencyFixture.NewZealandDollar);
            Assert.IsFalse((money1.CompareTo(money2)) < 0);
        }

        [TestMethod]
        public void Money1CompareToMoney2ReturnsZeroIfMoney1AndMoney2AreTheSame()
        {
            Money money1 = new Money(10, CurrencyFixture.NewZealandDollar);
            Money money2 = new Money(10, CurrencyFixture.NewZealandDollar);
            Assert.IsTrue((money1.CompareTo(money2)) == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoneyComparedToOtherTypeThrowsArgumentException()
        {
            new MoneyBuilder().Build().CompareTo(new object());
        }

        [TestMethod]
        public void EqualsMoneyReturnsTrueIsMoneyAmountIsTheSameUsingOperatorOverload()
        {
            Money money1 = new Money(10, CurrencyFixture.NewZealandDollar);
            Money money2 = new Money(10, CurrencyFixture.NewZealandDollar);
            Assert.IsTrue(money1 == money2);
        }

        [TestMethod]
        public void ToDecimalReturnsDecimalFromMoneyAmount()
        {
            Money theMoney = new Money(10, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(10m, theMoney.ToDecimal());
        }

        [TestMethod]
        public void ToNumericStringReturnsStringRepresentationOfAmountWithNoFormatting()
        {
            const decimal expected = 10.56m;
            Money money = new Money(expected, CurrencyFixture.NewZealandDollar);

            money.ToNumericString().Should().Be(expected.ToString());
        }

        [TestMethod]
        public void ParseNullStringReturnsZero()
        {
            Money.Parse(null, CurrencyFixture.NewZealandDollar)
                .Should()
                .Be(new Money(0m, CurrencyFixture.NewZealandDollar));
        }

        [TestMethod]
        public void ParseEmptyStringReturnsZero()
        {            
            Money.Parse(string.Empty, CurrencyFixture.NewZealandDollar)
                .Should()
                .Be(new Money(0, CurrencyFixture.NewZealandDollar));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseInvalidStringThrowsException()
        {
            Money.Parse("invalidMoney", CurrencyFixture.NewZealandDollar);
        }

        [TestMethod]
        public void ParseReturnsParsedAmountWhenInputIsValidDecimal()
        {
            const string parseThis = "10.54";
            Money expected = new Money((decimal)10.54, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, Money.Parse(parseThis, CurrencyFixture.NewZealandDollar));
        }

        [TestMethod]
        public void ParseReturnParsedAmountWhenInputIsValidFormattedString()
        {
            const string parseThis = "$1,054.45";
            Money expected = new Money((decimal)1054.45, CurrencyFixture.NewZealandDollar);
            Assert.AreEqual(expected, Money.Parse(parseThis, CurrencyFixture.NewZealandDollar));
        }

        [TestMethod]
        public void MoneyIsNotEqualToNull()
        {
            Money money = new MoneyBuilder().Build();
            money.Should().NotBeNull();
        }

        [TestMethod]
        public void MoneysAreReflexive()
        {
            Money money = new MoneyBuilder().Build();
            money.Should().Be(money);
        }

        [TestMethod]
        public void MoneysAreSymmetric()
        {
            Money moneyA = new MoneyBuilder().Build();
            Money moneyB = new MoneyBuilder().Build();

            moneyA.Should().Be(moneyB);
            moneyB.Should().Be(moneyA);
        }

        [TestMethod]
        public void MoneysAreTransitive()
        {
            Money moneyA = new MoneyBuilder().Build();
            Money moneyB = new MoneyBuilder().Build();
            Money moneyC = new MoneyBuilder().Build();

            moneyA.Should().Be(moneyB);
            moneyB.Should().Be(moneyC);
            moneyA.Should().Be(moneyC);
        }

        [TestMethod]
        public void ToRoundedDecimalReturnsTheValueToTwoDecimalPlaces()
        {
            Money money = new Money(12.567m, CurrencyFixture.NewZealandDollar);
            money.ToRoundedDecimal().Should().Be(12.57m);
        }

        [TestMethod]
        public void IsNegativeaReturnsTrueWhenAmountIsLessThanZero()
        {
            Money money = new Money(-1, CurrencyFixture.AustralianDollar);
            money.IsNegative.Should().Be(true);
        }

        [TestMethod]
        public void IsNegativeReturnsFalseWhenAmountIsZero()
        {
            Money zeroMoney = new Money(0, CurrencyFixture.AustralianDollar);
            zeroMoney.IsNegative.Should().Be(false);
        }

        [TestMethod]
        public void IsNegativeReturnsFalseWhenAmountIsMoreThanZero()
        {
            Money zeroMoney = new Money(7, CurrencyFixture.AustralianDollar);
            zeroMoney.IsNegative.Should().Be(false);
        }

        [TestMethod]
        public void IsZeroReturnsFalseWhenAmountIsGreaterThanZero()
        {
            Money zeroMoney = new Money(7, CurrencyFixture.AustralianDollar);
            zeroMoney.IsZero.Should().Be(false);
        }

        [TestMethod]
        public void IsZeroReturnsTrueWhenAmountIsEqualtToZero()
        {
            Money zeroMoney = new Money(0, CurrencyFixture.AustralianDollar);
            zeroMoney.IsZero.Should().Be(true);
        }

        [TestMethod]
        public void IsGreaterThanZeroReturnsFalseWhenAmountIsZero()
        {
            Money zeroMoney = new Money(0, CurrencyFixture.AustralianDollar);
            zeroMoney.IsGreaterThanZero.Should().Be(false);
        }

        [TestMethod]
        public void IsGreaterThanZeroReturnsTrueWhenAmountIsGreaterThanZero()
        {
            Money zeroMoney = new Money(7, CurrencyFixture.AustralianDollar);
            zeroMoney.IsGreaterThanZero.Should().Be(true);
        }

        [TestMethod]
        public void IsLessThanOrEqualToZeroReturnsTrueWhenAmountIsZero()
        {
            Money zeroMoney = new Money(0, CurrencyFixture.AustralianDollar);
            zeroMoney.IsLessThanOrEqualToZero.Should().BeTrue();
        }

        [TestMethod]
        public void IsLessThanOrEqualToZeroReturnsTrueWhenAmountIsLessThanZero()
        {
            Money zeroMoney = new Money(-7, CurrencyFixture.AustralianDollar);
            zeroMoney.IsLessThanOrEqualToZero.Should().BeTrue();
        }

        [TestMethod]
        public void IsLessThanOrEqualToZeroReturnsFalseWhenAmountIsGreaterThanZero()
        {
            Money zeroMoney = new Money(7, CurrencyFixture.AustralianDollar);
            zeroMoney.IsLessThanOrEqualToZero.Should().BeFalse();
        }

        [TestMethod]
        public void EqualsReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreNotEqual(aud, nzd);            
        }

        [TestMethod]
        public void EqualsMethodReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            aud.Equals(nzd).Should().Be(false);
        }


        [TestMethod]
        public void NotEqualReturnsTrueWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreNotEqual(aud, nzd);
        }

        [TestMethod]
        public void LessThanReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(10.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreEqual(aud < nzd, false);
        }

        [TestMethod]
        public void GreaterThanReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(14.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreEqual(aud > nzd, false);
        }

        [TestMethod]
        public void LessThanOrEqualsReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(10.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreEqual(aud <= nzd, false);
        }

        [TestMethod]
        public void GreaterThanOrEqualsReturnsFalseWhenMoneyHasDifferentCurrencies()
        {
            Money aud = new Money(14.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Assert.AreEqual(aud >= nzd, false);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddingAudToNzdThrowsException()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Money result = aud + nzd;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubtractingNzdFromAudThrowsException()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Money result = aud - nzd;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddingNzdToAudViaMethodThrowsException()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Money result = aud.Add(nzd);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubtractingNzdFromAudViaMethodThrowsException()
        {
            Money aud = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money nzd = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            Money result = aud.Subtract(nzd);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CompareThrowsExceptionWhenCurrenciesDontMatch()
        {
            Money pounds = new Money(12.5m, CurrencyFixture.AustralianDollar);
            Money yen = new Money(12.5m, CurrencyFixture.NewZealandDollar);

            int compare = pounds.CompareTo(yen);
        }

        [TestMethod]
        public void AddGstReturnsOriginalMoneyIncreasedByGSTAmount()
        {
            Money money = new Money(1900.5m, CurrencyFixture.NewZealandDollar);
            Gst gst = new Gst(12.5m);
            Money expected = money.Multiply(gst.Multiplier);

            Money actual = money.Add(gst);

            actual.Should().Be(expected);
        }

        [TestMethod]
        public void SubtractGstReturnsOriginalMoneyIncreasedByGSTAmount()
        {
            Money money = new MoneyBuilder().WithPrice(1900.5m).Build();
            Gst gst = new Gst(12.5m);
            Money expected = money.Multiply(gst.GstFraction / gst.Multiplier);

            Money actual = money.GstPaid(gst);

            actual.Should().Be(expected);
        }

        [TestMethod]
        public void AbsReturnsPositiveMoneyFromNegativeMoney()
        {
            Money moneyPositive = new Money(1900.5m, CurrencyFixture.NewZealandDollar);
            Money moneyNegative = new Money(-1900.5m, CurrencyFixture.NewZealandDollar);

            Money.Abs(moneyNegative).Should().Be(moneyPositive);
        }

        [TestMethod]
        public void AbsReturnsPositiveMoneyFromPositiveMoney()
        {
            Money moneyPositive = new Money(1900.5m, CurrencyFixture.NewZealandDollar);

            Money.Abs(moneyPositive).Should().Be(moneyPositive);
        }

        [TestMethod]
        public void SumWorks()
        {
            IEnumerable<Money> items = new[] { 
                new MoneyBuilder().WithPrice(20.1m).Build(),
                new MoneyBuilder().WithPrice(30.1m).Build() 
            };

            items.Sum(item => item.ToDecimal()).Should().Be(50.2m);
        }

        [TestMethod]
        public void SumWithSelectorWorks()
        {
            var itemWithPrice1 = new { Price = new MoneyBuilder().WithPrice(10m).Build() };
            var itemWithPrice2 = new { Price = new MoneyBuilder().WithPrice(3m).Build() };
            var itemList = new[] { itemWithPrice1, itemWithPrice2 };

            itemList.Sum(item => item.Price.ToDecimal()).Should().Be(new MoneyBuilder().WithPrice(13).Build().ToDecimal());
        }

        [TestMethod]
        public void SumWithCollectionWithOneItemWorks()
        {
            IEnumerable<Money> items = new[] { new MoneyBuilder().WithPrice(20.1m).Build() };

            items.Sum(item => item.ToDecimal()).Should().Be(new MoneyBuilder().WithPrice(20.1m).Build().ToDecimal());
        }
    }
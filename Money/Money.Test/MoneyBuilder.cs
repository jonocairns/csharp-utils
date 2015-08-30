using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money.Test
{
    public class MoneyBuilder
    {
        private Currency _currency;
        private decimal _price;

        public MoneyBuilder()
        {
            _price = 124.99m;
            _currency = new Currency("en-NZ");
        }

        public Money Build()
        {
            return new Money(_price, _currency);
        }

        public MoneyBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }

        public MoneyBuilder WithCurrency(Currency currency)
        {
            _currency = currency;
            return this;
        }
    }
}

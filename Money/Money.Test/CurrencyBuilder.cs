using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money.Test
{
    public class CurrencyBuilder
    {
        private string _code;

        public CurrencyBuilder()
        {
            _code = "en-au";
        }

        public Currency Build()
        {
            return new Currency(_code);
        }

        public CurrencyBuilder WithCode(string countryCode)
        {
            _code = countryCode;
            return this;
        }
    }
}

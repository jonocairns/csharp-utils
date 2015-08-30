using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money.Test
{
    public static class CurrencyFixture
    {
        public static Currency NewZealandDollar
        {
            get { return new CurrencyBuilder().WithCode("en-NZ").Build(); }
        }

        public static Currency AustralianDollar
        {
            get { return new CurrencyBuilder().Build(); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteListenerLib
{

    public class QuoteReaderFactory
    {
        private QuoteReaderFactory() { }

        public static IQuoteReader GetReader(string provider)
        {
            switch (provider)
            {
                case "Bloomberg":
                    return new BloombergQuoteListener();

                case "TWS":
                    return new TWSQuoteListener();

                default:
                    throw new ApplicationException(String.Format("Provider {0} is not supported by QuoteListenerLib"));
            }
        }
    }
}
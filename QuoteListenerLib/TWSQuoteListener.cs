using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWSLib;
using Gargoyle.Messaging.Common;

namespace QuoteListenerLib
{
    public class TWSQuoteListener : TWSUtilities, IQuoteReader
    {
        public TWSQuoteListener()
        {
            base.OnTick += TWSUtilities_OnTick;
         }

        void TWSUtilities_OnTick(object sender, TWSTickEventArgs e)
        {
            if (m_quoteEventHandler != null)
            {
                QuoteMessage quote = BuildQuoteMessage(e);
                if (quote != null)
                    m_quoteEventHandler(null, new QuoteEventArgs(quote, e.ClientObject));
            }
        }

       #region IQuoteReader Members

        public bool Init(params object[] paramList)
        {
            switch (paramList.Count())
            {
                case 0:
                    return base.Init(TWSUtilities.QUOTE_READER);
                case 1:
                    return base.Init(TWSUtilities.QUOTE_READER, (string)paramList[0]);
                case 2:
                    return base.Init(TWSUtilities.QUOTE_READER, (string)paramList[0], (int?)paramList[1]);
                default:
                    return base.Init(TWSUtilities.QUOTE_READER, (string)paramList[0], (int?)paramList[1], (int?)paramList[2]);
            }
       }

        private event QuoteEventHandler m_quoteEventHandler;
        public event QuoteEventHandler OnQuote
        {
            add { m_quoteEventHandler += value; }
            remove { m_quoteEventHandler -= value; }
        }
 
        #endregion

        QuoteMessage BuildQuoteMessage(TWSTickEventArgs e)
        {
            QuoteMessage quote = new QuoteMessage() { Ticker = e.Ticker, SubscriptionStatus = e.SubscriptionStatus };

            try
            {

                if (e.Open.HasValue)
                {
                    quote.Open = e.Open.Value;
                }

                if (e.PrevClose.HasValue)
                {
                    quote.PrevClose = e.PrevClose.Value;
                }

                if (e.Last.HasValue)
                {
                    quote.Last = e.Last.Value;
                }

                if (e.Bid.HasValue)
                {
                    quote.Bid = e.Bid.Value;
                }
                if (e.Ask.HasValue)
                {
                    quote.Ask = e.Ask.Value;
                }
                if (e.HasGreeks)
                {
                    if (e.Delta.HasValue) quote.Delta = e.Delta.Value;
                    if (e.Gamma.HasValue) quote.Gamma = e.Gamma.Value;
                    if (e.Theta.HasValue) quote.Theta = e.Theta.Value;
                    if (e.Vega.HasValue) quote.Vega = e.Vega.Value;
                    if (e.ImpliedVol.HasValue) quote.ImpliedVol = e.ImpliedVol.Value;
                }

            }
            catch (Exception ex)
            {
                Error("ERROR in Message handler: ", ex);
                return null;
            }

            return quote;
        }
    }
}

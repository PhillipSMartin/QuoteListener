using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloombergLib;
using Bloomberglp.Blpapi;
using GargoyleMessageLib;
using Gargoyle.Messaging.Common;

namespace QuoteListenerLib
{
    class BloombergQuoteListener : BloombergUtilities, IQuoteReader
    {
        public BloombergQuoteListener()
        {
            base.OnMessage += BloombergQuoteListener_OnMessage;
         }

        void BloombergQuoteListener_OnMessage(object sender, BloombergMessageEventArgs e)
        {
            if (m_quoteEventHandler != null)
            {
                QuoteMessage quote = BuildQuoteMessage(e.Message, e.SubscriptionId);
                if (quote != null)
                    m_quoteEventHandler(null, new QuoteEventArgs(quote, e.SubscriptionId.QuoteObject));
            }
        }

          #region IQuoteListenerReader Members

        public bool Init(params object[] paramList)
        {
            return base.Init();
        }

        public bool StartQuoteReader()
        {
            return IsConnected;
        }

        public bool StopQuoteReader()
        {
            return base.Disconnect();
        }

        #region Events
        private event QuoteEventHandler m_quoteEventHandler;

        public event QuoteEventHandler OnQuote
        {
            add { m_quoteEventHandler += value; }
            remove { m_quoteEventHandler -= value; }
        }
        #endregion

        public bool IsReading
        {
            get { return IsConnected; }
        }

        #endregion

        #region Private Methods
        QuoteMessage BuildQuoteMessage(Message message, ISubscriptionId id)
        {
            QuoteMessage quote = new QuoteMessage() { Ticker = id.Symbol };

            try
            {
                switch (message.MessageType.ToString())
                {
                    case "SubscriptionStarted":
                        quote.SubscriptionStatus = SubscriptionStatus.Subscribed;
                        Info("Subscription started for " + id.Symbol);
                        break;
 
                    case "SubscriptionTerminated":
                        quote.SubscriptionStatus = SubscriptionStatus.Unsubscribed;
                        Info("Subscription terminated for " + id.Symbol);
                         break;

                    case "SubscriptionFailure":
                         quote.SubscriptionStatus = SubscriptionStatus.Rejected;
                        Info("Subscription rejected for " + id.Symbol);
                         break;

                    case "MarketDataEvents":

                         if (message.HasElement("OPEN", true))
                         {
                             quote.Open = (double)message.GetElementAsFloat64("OPEN");
                         }

                         if (message.HasElement("PREV_CLOSE_VALUE_REALTIME", true))
                         {
                             quote.PrevClose = (double)message.GetElementAsFloat64("PREV_CLOSE_VALUE_REALTIME");
                         }

                         if (message.HasElement("LAST_PRICE", true))
                         {
                             quote.Last = (double)message.GetElementAsFloat64("LAST_PRICE");
                         }

                        if (message.HasElement("BID", true))
                        {
                            quote.Bid = (double)message.GetElementAsFloat64("BID");
                        }
                        if (message.HasElement("ASK", true))
                        {
                            quote.Ask = (double)message.GetElementAsFloat64("ASK");
                        }
                        if (message.HasElement("DELTA_MID_RT", true))
                        {
                            quote.Delta = (double)message.GetElementAsFloat64("DELTA_MID_RT");
                        }
                        if (message.HasElement("GAMMA_MID_RT", true))
                        {
                            quote.Gamma = (double)message.GetElementAsFloat64("GAMMA_MID_RT");
                        }
                        if (message.HasElement("THETA_MID_RT", true))
                        {
                            quote.Theta = (double)message.GetElementAsFloat64("THETA_MID_RT");
                        }
                        if (message.HasElement("VEGA_MID_RT", true))
                        {
                            quote.Vega = (double)message.GetElementAsFloat64("VEGA_MID_RT");
                        }
                        if (message.HasElement("IVOL_MID_RT", true))
                        {
                            quote.ImpliedVol = (double)message.GetElementAsFloat64("IVOL_MID_RT");
                        }

                        if (message.HasElement("OFFICIAL_CLOSE_TODAY_RT", true))
                        {
                            quote.Close = (double)message.GetElementAsFloat64("OFFICIAL_CLOSE_TODAY_RT");
                        }

                        if (message.HasElement("RT_SIMP_SEC_STATUS", true))
                            switch (message.GetElementAsString("RT_SIMP_SEC_STATUS"))
                            {
                                case "CLOS":
                                case "POST":
                                case "OUT":
                                    quote.OpenStatus = OpenStatus.Closed;
                                    break;
                                default:
                                    quote.OpenStatus = OpenStatus.Open;
                                    break;
                            }
                        break;

                    default:
                        return null;
                }

            }
            catch (Exception ex)
            {
                Error("ERROR in Message handler: ", ex);
                 return null;
            }

            return quote;
        }
        #endregion
    }
}

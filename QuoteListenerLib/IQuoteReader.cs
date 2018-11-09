using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GargoyleMessageLib;
using Gargoyle.Common;
using Gargoyle.Messaging.Common;

namespace QuoteListenerLib
{
    public interface IQuoteReader : IDisposable
    {
        bool Init(params object[] paramList);
        bool Connect();
        bool StartQuoteReader();
        bool StopQuoteReader();

        bool SubscribeToStocks(KeyValuePair<string, object>[] stocks);
        bool SubscribeToOptions(KeyValuePair<string, object>[] options);
        bool SubscribeToFutures(KeyValuePair<string, object>[] futures);
        bool SubscribeToIndices(KeyValuePair<string, object>[] indices);
        bool Unsubscribe(string[] symbol);
 
        event LoggingEventHandler OnError;
        event LoggingEventHandler OnInfo;
        event QuoteEventHandler OnQuote;
        event ServiceStoppedEventHandler OnReaderStopped;

        bool IsInitialized { get; }
        bool IsConnected { get; }
        bool IsReading { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gargoyle.Utilities.CommandLine;

namespace QuoteListenerConsole
{
    public class CommandLineParameters
    {
        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "r", Description = "Source of market data - Bloomberg or TWS")]
        public string Reader = "Bloomberg";

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "pname", Description = "Name of program to specify to DBAccess")]
        public string ProgramName = "QuoteListener";

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "tname", Description = "Task name for reporting completion")]
        public string TaskName = "QuoteListener - IB";

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "rhost", Description = "Host for Reader's TCP Connection")]
        public string ReaderHost = null;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "rport", Description = "Port for Reader's TCP Connection")]
        public string ReaderPort = null;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "wport", Description = "Port for Writer's TCP Connection")]
        public int WriterPort = 20000;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, Description = "Time in milleseconds before events time out")]
        public int Timeout = 10000;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "qt", Description = "DELAYED or REALTIME (default)")]
        public string QuoteType = "REALTIME";

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, Description = "Time app should automatically stop, expressed as a string hh:mm")]
        public string StopTime = "16:30";

        public bool GetStopTime(out TimeSpan stopTime)
        {
            return TimeSpan.TryParse(StopTime, out stopTime);
        }

        public int? GetReaderPort()
        {
            if (String.IsNullOrEmpty(ReaderPort))
                return null;
            else
                return int.Parse(ReaderPort);
        }

        public int GetQuoteType()
        {
            switch (QuoteType)
            {
                case "REALTIME":
                    return TWSLib.TWSUtilities.REAL_TIME;
                case "DELAYED":
                    return TWSLib.TWSUtilities.DELAYED;
                default:
                    throw new Exception("QuoteType must be REALTIME or DELAYED");
            }
         }
    }
}

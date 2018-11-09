using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace QuoteListenerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineParameters parms = new CommandLineParameters();
            Listener listener = null;
            var dirDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Gargoyle Strategic Investments\\QuoteListenerConsole";
            var appDataPath = dirDataPath + "\\TraceListener.log";
            DirectoryInfo dInfo = new DirectoryInfo(dirDataPath);
            if (!dInfo.Exists)
                dInfo.Create();

            var trace = new TextWriterTraceListener(new StreamWriter(appDataPath, false));

            try
            {

                if (Gargoyle.Utilities.CommandLine.Utility.ParseCommandLineArguments(args, parms))
                {

                    listener = new Listener(parms);
                    if (listener.Run())
                    {
                        trace.WriteLine("QuoteListener completed");
                    }
                    else
                    {
                        trace.WriteLine("QuoteListener failed - see error log");
                    }
                }
                else
                {
                    // display usage message
                    string errorMessage = Gargoyle.Utilities.CommandLine.Utility.CommandLineArgumentsUsage(typeof(CommandLineParameters));

                    trace.WriteLine(errorMessage);
                }
            }
            catch (Exception ex)
            {
                trace.WriteLine(ex.ToString());
            }
            finally
            {
                trace.Flush();
                if (listener != null)
                {
                    listener.Dispose();
                    listener = null;
                }
            }
        }
    }
}

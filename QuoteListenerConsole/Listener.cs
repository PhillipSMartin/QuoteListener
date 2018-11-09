using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gargoyle.Utilities.CommandLine;
using GargoyleTaskLib;
using log4net;
using QuoteListenerLib;
using Gargoyle.Utils.DBAccess;
using LoggingUtilitiesLib;
using GargoyleMessageLib;
using Gargoyle.Common;
using Gargoyle.Messaging.Common;

namespace QuoteListenerConsole
{
    public class Listener
    {
         CommandLineParameters m_parms;
         private bool m_initialized;
         private bool m_bTaskStarted;
         private bool m_bTaskFailed;
         private bool m_bWaiting;
         private bool m_bReaderStarted;
         private bool m_bWriterStarted;
         private string m_lastErrorMessage;

        private ILog m_logger = LogManager.GetLogger(typeof(Listener));
        private System.Data.SqlClient.SqlConnection m_hugoConnection;

        public IQuoteReader QuoteReader { get; private set; }
        private GargoyleMessageUtilities m_writer = new GargoyleMessageUtilities();

        TimeSpan m_stopTime;
        private AutoResetEvent m_waitHandle = new AutoResetEvent(false);
 
        #region Housekeeping
        public Listener(CommandLineParameters parms)
        {
            m_parms = parms;
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (m_bWaiting)
            {
                m_waitHandle.Set();
                System.Threading.Thread.Sleep(m_parms.Timeout * 2);
            }

            if (m_bTaskStarted)
                m_bTaskStarted = !EndTask(m_parms.TaskName, !m_bTaskFailed);

            if (disposing)
            {
                if (m_waitHandle != null)
                {
                    m_waitHandle.Dispose();
                    m_waitHandle = null;
                }
                if (QuoteReader != null)
                {
                    QuoteReader.Dispose();
                    QuoteReader = null;
                }
  
                if (m_hugoConnection != null)
                {
                    m_hugoConnection.Dispose();
                    m_hugoConnection = null;
                }

                if (m_writer != null)
                {
                    m_writer.Dispose();
                    m_writer = null;
                }
             }
        }
        #endregion
        #endregion

        public bool Run()
        {
            m_bTaskFailed = true;
            try
            {
                // initialize logging
                log4net.Config.XmlConfigurator.Configure();
                TaskUtilities.OnInfo += new LoggingEventHandler(Utilities_OnInfo);
                TaskUtilities.OnError += new LoggingEventHandler(Utilities_OnError);

                if (!m_parms.GetStopTime(out m_stopTime))
                {
                    OnInfo("Invalid stop time specified", true);
                }

                else
                {
                    if (Initialize())
                    {
                        int rc = StartTask(m_parms.TaskName);
                        if (rc == 1)
                        {
                            OnInfo("Task not started because it is a holiday");
                        }
                        else
                        {
                            // if task wasn't started (which probably means taskname was not in the table), so be it - no need to abort
                            m_bTaskStarted = (rc == 0);

                            if (StartListener())
                            {
                                m_bTaskFailed = false;  // set up was successful - we now wait for the timer to expire or for a post from an event handler
                                m_bWaiting = true;
                                bool timedOut = WaitForCompletion();
                                m_bWaiting = false;
                                StopListener();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                m_bTaskFailed = true;
                OnError("Error in Run method", ex, true);
            }
            finally
            {
                if (m_bTaskStarted)
                {
                     m_bTaskStarted = !EndTask(m_parms.TaskName, !m_bTaskFailed);
                }
            }
            return !m_bTaskFailed;
        }
        #region Start Listener
        private bool StartListener()
        {
            if (Login())
            {
                if (StartWriter())
                {
                    return m_bReaderStarted = StartReader(m_parms.Reader);
                }
            }

            return false;
        }

        private bool Login()
        {
            try
            {
                // parameters are ignored for Bloomberg
                if (QuoteReader.Init(m_parms.ReaderHost, m_parms.GetReaderPort(), m_parms.GetQuoteType()))
                {
                    if (QuoteReader.Connect())
                        return true;
                    else
                        OnInfo("Unable to log in", true);
                    return false;
                }
                else
                {
                    OnInfo("Unable to initialize quote reader", true);
                }
            }
            catch (Exception ex)
            {
                OnError("Unable to log in", ex, true);
            }
            return false;
        }

        private bool StartWriter()
        {
            try
            {
                m_writer.OnInfo += GargoyleMessageUtilities_OnInfo;
                m_writer.OnError += GargoyleMessageUtilities_OnError;
                m_writer.OnSubscription += GargoyleMessageUtilities_OnSubscription;
                m_writer.OnUnsubscription += GargoyleMessageUtilities_OnUnsubscription;
                m_writer.OnPublisherStopped += GargoyleMessageUtilities_OnPublisherStopped;
//              m_writer.OnDebug += GargoyleMessageUtilities_OnInfo;


                m_bWriterStarted = m_writer.StartPublisher(m_parms.WriterPort);
                if (m_bWriterStarted)
                    return true;
                else
                    OnInfo("Unable to start publisher", true);
            }
            catch (Exception ex)
            {
                OnError("Unable to start publisher", ex, true);
            }
            return false;
        }

        void GargoyleMessageUtilities_OnPublisherStopped(object sender, ServiceStoppedEventArgs e)
        {
            m_bWriterStarted = false;
            StopListener();
        }

        void GargoyleMessageUtilities_OnUnsubscription(object sender, UnsubscriptionEventArgs e)
        {
            try
            {
                UnsubscriptionEventArgs args = e as UnsubscriptionEventArgs;
                QuoteReader.Unsubscribe(new string[] { args.Ticker });
            }
            catch (Exception ex)
            {
                OnError("Error in OnUnsubscription method", ex);
            }
        }

        void GargoyleMessageUtilities_OnSubscription(object sender, SubscriptionEventArgs e)
        {
            try
            {
                switch (e.QuoteType)
                {
                    case QuoteType.Stock:
                        QuoteReader.SubscribeToStocks(new KeyValuePair<string, object>[] { new KeyValuePair<string, object>(e.Ticker, e.SubscriptionObject) });
                        break;
                    case QuoteType.Option:
                        QuoteReader.SubscribeToOptions(new KeyValuePair<string, object>[] { new KeyValuePair<string, object>(e.Ticker, e.SubscriptionObject) });
                        break;
                    case QuoteType.Future:
                        QuoteReader.SubscribeToFutures(new KeyValuePair<string, object>[] { new KeyValuePair<string, object>(e.Ticker, e.SubscriptionObject) });
                        break;
                    case QuoteType.Index:
                        QuoteReader.SubscribeToIndices(new KeyValuePair<string, object>[] { new KeyValuePair<string, object>(e.Ticker, e.SubscriptionObject) });
                        break;
                    default:
                        OnInfo("Unsupported QuoteType " + e.QuoteType.ToString());
                        break;
                }
            }
            catch (Exception ex)
            {
                OnError("Error in OnSubscription method", ex);
            }

        }

        private void GargoyleMessageUtilities_OnError(object sender, LoggingEventArgs e)
        {
             OnError(e.Message, e.Exception, true);
        }

        private void GargoyleMessageUtilities_OnInfo(object sender, LoggingEventArgs e)
        {
            OnInfo(e.Message);
        }

        private bool StartReader(string reader)
        {
            try
            {
               if (QuoteReader.StartQuoteReader())
                    return true;
                else
                    OnInfo("Unable to start quote reader", true);
            }
            catch (Exception ex)
            {
                OnError("Unable to start quote reader", ex, true);
            }
            return false;
        }

        void m_reader_OnReaderStopped(object sender, ServiceStoppedEventArgs e)
        {
            m_bReaderStarted = false;
            StopListener();
        }

        void m_reader_OnQuote(object sender, QuoteEventArgs e)
        {
            m_writer.PublishQuote(e.Quote, e.ClientObject);
        }
          #endregion

        #region Stop Listener
        public void StopListener()
        {
            m_initialized = false;
            StopReader();
            StopWriter();
        }
        private void StopReader()
        {
            if (m_bReaderStarted)
            {
                try
                {
                    m_bReaderStarted = false;
                    if (QuoteReader.StopQuoteReader())
                    {
                        m_writer.OnSubscription -= GargoyleMessageUtilities_OnSubscription;
                        m_writer.OnUnsubscription += GargoyleMessageUtilities_OnUnsubscription;
                    }
                }
                catch (Exception ex)
                {
                    OnError("Unable to stop reader", ex);
                }
            }
        }
        private void StopWriter()
        {
            if (m_bWriterStarted)
            {
                try
                {
                    m_bWriterStarted = false;
                    m_writer.StopPublisher();
                    m_writer.StopSubscriber();
                }
                catch (Exception ex)
                {
                    OnError("Unable to stop writer", ex);
                }
            }
        }
        #endregion

        #region Wait
        // returns true if terminated because we reached stopTime, false if terminated early
        private bool WaitForCompletion()
        {
            DateTime stopDateTime = DateTime.Today + m_stopTime;

            int tickTime = (int)(stopDateTime - DateTime.Now).TotalMilliseconds;
            if (tickTime <= 120000)
                tickTime = 120000;  // make sure stop time is at least 2 minutes from now

            OnInfo(String.Format("Waiting for {0} ms", tickTime));
            if (WaitAny(tickTime, m_waitHandle))
            {
                OnInfo("Job terminated early");
                return false;
            }
            else
            {
                OnInfo("Job terminated on schedule");
                return true;
            }
        }
        private bool WaitAny(int millisecondsTimeout, params System.Threading.WaitHandle[] successConditionHandles)
        {
            int n;
            if (millisecondsTimeout == 0)
                n = System.Threading.WaitHandle.WaitAny(successConditionHandles);
            else
                n = System.Threading.WaitHandle.WaitAny(successConditionHandles, millisecondsTimeout);
            if (n == System.Threading.WaitHandle.WaitTimeout)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Logging
        // events from LoggingUtilities and TaskUtilities
        private void Utilities_OnInfo(object sender, LoggingEventArgs eventArgs)
        {
            OnInfo(eventArgs.Message);
        }

        private void Utilities_OnError(object sender, LoggingEventArgs eventArgs)
        {
            OnError(eventArgs.Message, eventArgs.Exception);
        }

        // helper methods to write to log
        private void OnInfo(string msg, bool updateLastErrorMsg = false)
        {
            if (updateLastErrorMsg)
                m_lastErrorMessage = msg;
            if (m_logger != null)
            {
                lock (m_logger)
                {
                    m_logger.Info(msg);
                }
            }

        }
        private void OnError(string msg, Exception e, bool updateLastErrorMsg = false)
        {
            if (updateLastErrorMsg && e != null)
                m_lastErrorMessage = msg + "->" + e.Message;
            else
                m_lastErrorMessage = msg;

            if (m_logger != null)
            {
                lock (m_logger)
                {
                    m_logger.Error(msg, e);
                }
            }
        }
        #endregion

        #region Task Management
        // returns 0 if task is successfully started
        // returns 1 if we should not start the task because it should not be run today (because this task should not run on a holiday, for example)
        // returns 4 if we cannot find the task name
        // returns some number higher than 4 on an unexpected failure
        private int StartTask(string taskName)
        {
            try
            {
                using (TaskUtilities taskUtilities = new TaskUtilities(m_hugoConnection, m_parms.Timeout))
                {
                    return taskUtilities.TaskStarted(taskName, null);
                }
            }
            catch (Exception ex)
            {
                OnError("Unable to start task", ex);
                return 16;
            }
        }

        private bool EndTask(string taskName, bool succeeded)
        {
            try
            {
                using (TaskUtilities taskUtilities = new TaskUtilities(m_hugoConnection, m_parms.Timeout))
                {
                    if (succeeded)
                        return (0 != taskUtilities.TaskCompleted(taskName, ""));
                    else
                        return (0 != taskUtilities.TaskFailed(taskName, m_lastErrorMessage));
                }
            }
            catch (Exception ex)
            {
                OnError("Unable to stop task", ex);
                return false;
            }
        }
        #endregion

        #region Initialize
        public bool Initialize()
        {
            if (!m_initialized)
            {
                if (!GetDatabaseConnections())
                    return false;

                if (!SelectReader(m_parms.Reader))
                    return false;

            }
            m_initialized = true;
           return true;
        }
        private bool GetDatabaseConnections()
        {
            DBAccess dbAccess = DBAccess.GetDBAccessOfTheCurrentUser(m_parms.ProgramName);
            m_hugoConnection = dbAccess.GetConnection("Hugo");
            if (m_hugoConnection == null)
            {
                OnInfo("Unable to connect to Hugo", true);
                return false;
            }
            return true;
        }
        private bool SelectReader(string reader)
        {
            try
            {
                if (QuoteReader != null)
                {
                    QuoteReader.OnError -= GargoyleMessageUtilities_OnError;
                    QuoteReader.OnInfo -= GargoyleMessageUtilities_OnInfo;
                    QuoteReader.OnQuote -= m_reader_OnQuote;
                    QuoteReader.OnReaderStopped -= m_reader_OnReaderStopped;
                    QuoteReader.Dispose();

                }

                QuoteReader = QuoteReaderFactory.GetReader(reader);
                if (QuoteReader == null)
                {
                    OnInfo(String.Format("Unable to instantiate quote reader {0}", reader), true);
                    return false;
                }
                else
                {
                    QuoteReader.OnError += GargoyleMessageUtilities_OnError;
                    QuoteReader.OnInfo += GargoyleMessageUtilities_OnInfo;
                    QuoteReader.OnQuote += m_reader_OnQuote;
                    QuoteReader.OnReaderStopped += m_reader_OnReaderStopped;

                    return true;
                }
            }
            catch (Exception ex)
            {
                OnError("Unable to instantiate quote reader", ex, true);
                return false;
            }
        }
         #endregion
    }
}

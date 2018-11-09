using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gargoyle.Common;
using QuoteListenerConsole;

namespace TWSQuoteReaderDebugger
{
    public partial class Form1 : Form
    {
        Listener m_listener;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = String.Format("{0} {1}",
                  System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                 System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

            comboBoxQuoteType.SelectedIndex = 0;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                buttonStart.Enabled = false;
                CommandLineParameters parms = new CommandLineParameters()
                {
                    Reader = "TWS",
                    ProgramName = textBoxProgramName.Text,
                    TaskName = textBoxTaskName.Text,
                    ReaderHost = textBoxReaderHost.Text,
                    ReaderPort = textBoxReaderPort.Text,
                    WriterPort = int.Parse(textBoxWriterPort.Text),
                    Timeout = int.Parse(textBoxTImeOut.Text),
                    QuoteType = comboBoxQuoteType.Text,
                    StopTime = textBoxStopTime.Text
                };

                m_listener = new Listener(parms);
                if (m_listener.Initialize())
                {
                    m_listener.QuoteReader.OnInfo += OnInfo;
                    m_listener.QuoteReader.OnError += OnError;
                    buttonStop.Enabled = true;

                    BackgroundWorker listenerProcess = new BackgroundWorker();
                    listenerProcess.DoWork += new DoWorkEventHandler(ListenerWorker);
                    listenerProcess.RunWorkerAsync();
                }
                else
                {
                    AddMessage("Unable to initialzie listener - see log");
                    buttonStart.Enabled = true;
                    buttonStop.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                AddMessage("Error in Start: " + ex.Message);
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
            }
        }

        private void ListenerWorker(object sender, DoWorkEventArgs e)
        {
            try
            {
                m_listener.Run();
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
            }
            catch (Exception ex)
            {
                AddMessage("Error in Start: " + ex.Message);
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
            }
        }

        private void AddMessage(string message)
        {
            Action a = delegate
            {
                listBox1.Items.Add(message);
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }
        private void OnInfo(object sender, LoggingEventArgs eventArgs)
        {
            AddMessage(eventArgs.Message);
        }

        private void OnError(object sender, LoggingEventArgs eventArgs)
        {
            AddMessage(eventArgs.Message + "=>" + eventArgs.Exception.Message);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                m_listener.StopListener();
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
            }
            catch (Exception ex)
            {
                AddMessage("Error in Start: " + ex.Message);
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FillMiniLog();
        }

        private void FillMiniLog()
        {
            listBox2.Items.Clear();
            if (comboBoxSubscriptions.SelectedIndex >= 0)
            {
                listBox2.Items.AddRange(((TWSLib.TWSSubscription)comboBoxSubscriptions.SelectedItem).GetLog());
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            int saveIndex = comboBoxSubscriptions.SelectedIndex;
            comboBoxSubscriptions.SuspendLayout();
            comboBoxSubscriptions.Items.Clear();
            comboBoxSubscriptions.Items.AddRange(TWSLib.TWSSubscriptionManager.GetAllSubscriptions());
            comboBoxSubscriptions.SelectedIndex = saveIndex;
            comboBoxSubscriptions.ResumeLayout();

            FillMiniLog();
        }
    }
}

namespace TWSQuoteReaderDebugger
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxProgramName = new System.Windows.Forms.TextBox();
            this.textBoxTaskName = new System.Windows.Forms.TextBox();
            this.textBoxReaderHost = new System.Windows.Forms.TextBox();
            this.textBoxReaderPort = new System.Windows.Forms.TextBox();
            this.textBoxWriterPort = new System.Windows.Forms.TextBox();
            this.textBoxTImeOut = new System.Windows.Forms.TextBox();
            this.textBoxStopTime = new System.Windows.Forms.TextBox();
            this.comboBoxQuoteType = new System.Windows.Forms.ComboBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.comboBoxSubscriptions = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Program Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Task Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Reader Host";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Reader Port";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Writer Port";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "TimeOut";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Quote Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(38, 228);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "StopTime";
            // 
            // textBoxProgramName
            // 
            this.textBoxProgramName.Location = new System.Drawing.Point(96, 21);
            this.textBoxProgramName.Name = "textBoxProgramName";
            this.textBoxProgramName.Size = new System.Drawing.Size(139, 20);
            this.textBoxProgramName.TabIndex = 8;
            this.textBoxProgramName.Text = "QuoteListener";
            this.textBoxProgramName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTaskName
            // 
            this.textBoxTaskName.Location = new System.Drawing.Point(96, 50);
            this.textBoxTaskName.Name = "textBoxTaskName";
            this.textBoxTaskName.Size = new System.Drawing.Size(139, 20);
            this.textBoxTaskName.TabIndex = 9;
            this.textBoxTaskName.Text = "QuoteListener - IB";
            this.textBoxTaskName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxReaderHost
            // 
            this.textBoxReaderHost.Location = new System.Drawing.Point(96, 79);
            this.textBoxReaderHost.Name = "textBoxReaderHost";
            this.textBoxReaderHost.Size = new System.Drawing.Size(139, 20);
            this.textBoxReaderHost.TabIndex = 10;
            this.textBoxReaderHost.Text = "gargoyle-mw20.gsi.com";
            this.textBoxReaderHost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxReaderPort
            // 
            this.textBoxReaderPort.Location = new System.Drawing.Point(96, 108);
            this.textBoxReaderPort.Name = "textBoxReaderPort";
            this.textBoxReaderPort.Size = new System.Drawing.Size(139, 20);
            this.textBoxReaderPort.TabIndex = 11;
            this.textBoxReaderPort.Text = "7496";
            this.textBoxReaderPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxWriterPort
            // 
            this.textBoxWriterPort.Location = new System.Drawing.Point(96, 137);
            this.textBoxWriterPort.Name = "textBoxWriterPort";
            this.textBoxWriterPort.Size = new System.Drawing.Size(139, 20);
            this.textBoxWriterPort.TabIndex = 12;
            this.textBoxWriterPort.Text = "20000";
            this.textBoxWriterPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTImeOut
            // 
            this.textBoxTImeOut.Location = new System.Drawing.Point(96, 166);
            this.textBoxTImeOut.Name = "textBoxTImeOut";
            this.textBoxTImeOut.Size = new System.Drawing.Size(139, 20);
            this.textBoxTImeOut.TabIndex = 13;
            this.textBoxTImeOut.Text = "10000";
            this.textBoxTImeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxStopTime
            // 
            this.textBoxStopTime.Location = new System.Drawing.Point(96, 225);
            this.textBoxStopTime.Name = "textBoxStopTime";
            this.textBoxStopTime.Size = new System.Drawing.Size(139, 20);
            this.textBoxStopTime.TabIndex = 14;
            this.textBoxStopTime.Text = "16:30";
            this.textBoxStopTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // comboBoxQuoteType
            // 
            this.comboBoxQuoteType.FormattingEnabled = true;
            this.comboBoxQuoteType.Items.AddRange(new object[] {
            "REALTIME",
            "DELAYED"});
            this.comboBoxQuoteType.Location = new System.Drawing.Point(96, 195);
            this.comboBoxQuoteType.Name = "comboBoxQuoteType";
            this.comboBoxQuoteType.Size = new System.Drawing.Size(139, 21);
            this.comboBoxQuoteType.TabIndex = 15;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(96, 264);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(139, 23);
            this.buttonStart.TabIndex = 16;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(96, 293);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(139, 23);
            this.buttonStop.TabIndex = 17;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(12, 343);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(847, 173);
            this.listBox1.TabIndex = 18;
            // 
            // comboBoxSubscriptions
            // 
            this.comboBoxSubscriptions.FormattingEnabled = true;
            this.comboBoxSubscriptions.Location = new System.Drawing.Point(351, 20);
            this.comboBoxSubscriptions.Name = "comboBoxSubscriptions";
            this.comboBoxSubscriptions.Size = new System.Drawing.Size(419, 21);
            this.comboBoxSubscriptions.TabIndex = 19;
            this.comboBoxSubscriptions.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(275, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Subscriptions";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.HorizontalScrollbar = true;
            this.listBox2.Location = new System.Drawing.Point(292, 53);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(567, 277);
            this.listBox2.TabIndex = 21;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(776, 19);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 22;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 520);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBoxSubscriptions);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.comboBoxQuoteType);
            this.Controls.Add(this.textBoxStopTime);
            this.Controls.Add(this.textBoxTImeOut);
            this.Controls.Add(this.textBoxWriterPort);
            this.Controls.Add(this.textBoxReaderPort);
            this.Controls.Add(this.textBoxReaderHost);
            this.Controls.Add(this.textBoxTaskName);
            this.Controls.Add(this.textBoxProgramName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "TWS Quote Reader Debbugger";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxProgramName;
        private System.Windows.Forms.TextBox textBoxTaskName;
        private System.Windows.Forms.TextBox textBoxReaderHost;
        private System.Windows.Forms.TextBox textBoxReaderPort;
        private System.Windows.Forms.TextBox textBoxWriterPort;
        private System.Windows.Forms.TextBox textBoxTImeOut;
        private System.Windows.Forms.TextBox textBoxStopTime;
        private System.Windows.Forms.ComboBox comboBoxQuoteType;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ComboBox comboBoxSubscriptions;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button buttonRefresh;
    }
}


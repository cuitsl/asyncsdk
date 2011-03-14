using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASync.MiddleWare;
using eTerm.AsyncSDK;

namespace ASync.eTermAddIn {
    public partial class ASyncLogViewer : BaseAddIn {
        /// <summary>
        /// Initializes a new instance of the <see cref="ASyncLogViewer"/> class.
        /// </summary>
        public ASyncLogViewer() {
            InitializeComponent();
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        dateTimeInput1.Value = DateTime.Now.AddDays(-5);
                        dateTimeInput2.Value = DateTime.Now;
                        this.richTextBox1.BackColor = Color.Black;
                        this.richTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
                        this.richTextBox1.Font = new Font("Consolas", 12.0f);
                        this.columnHeader1.Width = 50;
                        this.columnHeader1.Text = @"编号";
                        this.columnHeader2.Width = 80;
                        this.columnHeader1.Text = @"客户端";
                        this.columnHeader3.Width = 80;
                        this.columnHeader1.Text = @"配置帐号";
                        this.columnHeader4.Width = 150;
                        this.columnHeader1.Text = @"原始指令";
                        this.columnHeader5.Width = 120;
                        this.columnHeader1.Text = @"记录时间";
                        this.listViewEx1.DoubleClick += new EventHandler(this.listViewEx1_SelectedIndexChanged);
                        foreach (TSessionSetup session in AsyncStackNet.Instance.ASyncSetup.SessionCollection)
                            this.comboBoxEx1.Items.Add(session.SessionCode);
                        foreach (ConnectSetup setup in AsyncStackNet.Instance.ASyncSetup.AsynCollection)
                            this.comboBoxEx2.Items.Add(setup.userName);
                    }
                );
        }

        /// <summary>
        /// Queries the log.
        /// </summary>
        private void QueryLog() {
            listViewEx1.Items.Clear();
            ContextInstance.Instance.providerName = LicenceManager.Instance.LicenceBody.providerName;
            ContextInstance.Instance.connectionString = LicenceManager.Instance.LicenceBody.connectionString;
            int RecordCount= Async_Log.Find(Log =>
                    (string.IsNullOrEmpty(comboBoxEx1.Text.Trim()) ? true : Log.ClientSession == comboBoxEx1.Text.Trim())
                    &&
                    (string.IsNullOrEmpty(comboBoxEx2.Text.Trim()) ? true : Log.eTermSession == comboBoxEx2.Text.Trim())
                    &&
                    (string.IsNullOrEmpty(textBoxX1.Text.Trim()) ? true : Log.ASynCommand.Contains(textBoxX1.Text.Trim()))
                    &&
                    Log.LogDate >= dateTimeInput1.Value
                    &&
                    Log.LogDate <= dateTimeInput2.Value).Count<Async_Log>();
            slider1.Maximum = RecordCount % 30 == 0 ? RecordCount / 30 : RecordCount / 30 + 1;
            slider1.Text = string.Format(@"总记录数：{0} 页次：{1} /{2}", RecordCount, this.slider1.Value, slider1.Maximum);
            foreach (Async_Log log in Async_Log.Find(Log =>
                    (string.IsNullOrEmpty(comboBoxEx1.Text.Trim()) ? true : Log.ClientSession == comboBoxEx1.Text.Trim())
                    &&
                    (string.IsNullOrEmpty(comboBoxEx2.Text.Trim()) ? true : Log.eTermSession == comboBoxEx2.Text.Trim())
                    &&
                    (string.IsNullOrEmpty(textBoxX1.Text.Trim()) ? true : Log.ASynCommand.Contains(textBoxX1.Text.Trim()))
                    &&
                    Log.LogDate >= dateTimeInput1.Value
                    &&
                    Log.LogDate <= dateTimeInput2.Value).OrderByDescending < Async_Log,DateTime?>(Log=>Log.LogDate).Skip<Async_Log>((this.slider1.Value-1)*30).Take<Async_Log>(30)) {
                ListViewItem logItem = new ListViewItem(new string[] { 
                    log.Id.ToString(),
                    log.ClientSession.ToString(),
                    log.eTermSession.ToString(),
                    log.ASynCommand,
                    log.LogDate.Value.ToString(@"yyyy-MM-dd HH:mm:ss")
                });
                logItem.Tag = log;
                listViewEx1.Items.Add(logItem);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnQuery control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnQuery_Click(object sender, EventArgs e) {
            if (!LicenceManager.Instance.LicenceBody.AllowDatabase)
                throw new Exception(@"授权不允许使用数据库，请联系开发商获取对应的授权！");
            //pageNavigator1.pa
            QueryLog();
            tabControl2.SelectedTab = tabItem1;
        }

        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        public override string ButtonName {
            get { return "eTerm日志查询分析器"; }
        }

        /// <summary>
        /// Gets the image icon.
        /// </summary>
        /// <value>The image icon.</value>
        public override string ImageIcon {
            get { return "Hourglass.png"; }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the listViewEx1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void listViewEx1_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.listViewEx1.SelectedItems.Count != 1) return;
            this.richTextBox1.Text = (this.listViewEx1.SelectedItems[0].Tag as Async_Log).ASyncResult.Replace("\r","\r\n");
            tabControl2.SelectedTab = tabItem3;
        }

    }
}

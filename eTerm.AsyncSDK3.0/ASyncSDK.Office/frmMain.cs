using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using eTerm.AsyncSDK.Net;
using eTerm.AsyncSDK;
using eTerm.AsyncSDK.Core;
using System.IO;
using System.Net;
using eTerm.AsyncSDK.Base;
using DevComponents.DotNetBar;
using ASync.MiddleWare;
using System.Reflection;
using eTerm.AsyncSDK.Util;

namespace ASyncSDK.Office {
    public partial class frmMain : Office2007RibbonForm {
        #region 初始化
        ListViewGroup group1 = new ListViewGroup("活动连接");
        ListViewGroup group2 = new ListViewGroup("非活动连接");
        public frmMain() {
            InitializeComponent();
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        lstAsync.Groups.AddRange(new ListViewGroup[] { group1, group2 });
                            foreach (object itemValue in Enum.GetValues(DevComponents.DotNetBar.eStyle.Windows7Blue.GetType())) {
                                ButtonItem btnItem = new ButtonItem() { ButtonStyle = eButtonStyle.ImageAndText, Text = itemValue.ToString(), Tag=itemValue };
                                btnItem.Image = global::ASyncSDK.Office.Properties.Resources.Flag2_Green;
                                btnItem.Click += new EventHandler(
                                        delegate(object btnSender, EventArgs btnEvent) {
                                            this.styleManager1.ManagerStyle = (eStyle)(btnItem as ButtonItem).Tag;
                                        }
                                    );
                                this.btnSkin.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
                                btnItem
                            });
                        }
                    }
                );
        }
        #endregion

        #region UI代理定义
        public delegate void UpdateListViewItem(ListViewEx ViewEx, string ImageKey, string Id, float TotalBytes, string Reconnect);

        public delegate void AppendSessionLogDelegate(ListView View, string SessionName, string operationType, string SessionCommand, string flag);

        public delegate void UpdateStatusTextDelegate(ToolStripStatusLabel targetLable, string Text);

        public delegate void ASynConnectCallback(eTerm443Async ASync);

        /// <summary>
        /// Appends the A syn connect.
        /// </summary>
        /// <param name="ASync">The A sync.</param>
        private void appendASynConnect(eTerm443Async ASync) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new ASynConnectCallback(appendASynConnect), ASync);
                return;
            }
            try {
                string SessionId = string.Format(@"{0}{1}{2}", ASync.RemoteEP.ToString(), ASync.userName, ASync.IsSsl);
                ListViewItem connectItem;
                if (this.lstAsync.Items.ContainsKey(SessionId)) {
                    connectItem = this.lstAsync.Items[SessionId];
                    connectItem.ImageKey = @"Circle_Green.png";
                    connectItem.Group = group1;
                    return;
                }

                connectItem = new ListViewItem(
                        new string[] {
                    ASync.RemoteEP.ToString(),
                    ASync.userName,
                    ASync.TotalBytes.ToString("f2"),
                    ASync.TotalCount.ToString(),
                    ASync.ReconnectCount.ToString(),
                    ASync.CurrentBytes.ToString(@"f2"),
                    ASync.LastActive.ToString(@"HH:mm:ss")
                }, group1) { Name = SessionId };
                    connectItem.Tag = ASync;
                    connectItem.ImageKey = @"Circle_Green.png";
                    this.lstAsync.Items.Add(connectItem);
            }
            catch { }
        }

        /// <summary>
        /// Disconnects the A sync.
        /// </summary>
        /// <param name="ASync">The A sync.</param>
        private void disconnectASync(eTerm443Async ASync) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new ASynConnectCallback(disconnectASync), ASync);
                return;
            }
            try {
                string SessionId = string.Format(@"{0}{1}{2}", ASync.RemoteEP.ToString(), ASync.userName, ASync.IsSsl);
                ListViewItem item = this.lstAsync.Items[SessionId];
                item.ImageKey = @"Circle_Red.png";
                item.Group = group2;
            }
            catch { }
        }

        /// <summary>
        /// Updates the A sync.
        /// </summary>
        /// <param name="ASync">The A sync.</param>
        private void updateASync(eTerm443Async ASync) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new ASynConnectCallback(updateASync), ASync);
                return;
            }
            try {
                string SessionId = string.Format(@"{0}{1}{2}", ASync.RemoteEP.ToString(), ASync.userName, ASync.IsSsl);

            }
            catch { }
        }
        #endregion

        #region 关闭事件
        /// <summary>
        /// Handles the Click event of the btnExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        /// <summary>
        /// Handles the FormClosing event of the frmMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            if (MessageBox.Show("确实想退出该软件，退出后将导致客户端无法连接！", "关闭提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                e.Cancel = false;
                Application.ExitThread();
            }
            else { e.Cancel = true; }
        }
        #endregion

        #region AppendistViewItem
        /// <summary>
        /// Updates the status text.
        /// </summary>
        /// <param name="targetLable">The target lable.</param>
        /// <param name="Text">The text.</param>
        private void UpdateStatusText(ToolStripStatusLabel targetLable, string Text) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new UpdateStatusTextDelegate(UpdateStatusText), targetLable, Text);
                return;
            }
            try {
                targetLable.Text = Text;
            }
            catch { }
        }

        /// <summary>
        /// Appends the session log.
        /// </summary>
        /// <param name="SessionName">Name of the session.</param>
        /// <param name="operationType">Type of the operation.</param>
        /// <param name="SessionCommand">The session command.</param>
        /// <param name="flag">The flag.</param>
        private void AppendSessionLog(ListView View, string SessionName, string operationType, string SessionCommand, string flag) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new AppendSessionLogDelegate(AppendSessionLog), View, SessionName, operationType, SessionCommand, flag);
                return;
            }
            try {
                if (View.Items.Count >= 250) View.Items.Clear();
                View.Items.Insert(0, new ListViewItem(new string[]{
                    (View.Items.Count+1).ToString(),
                    SessionName,
                    operationType,
                    SessionCommand,
                    flag,
                    DateTime.Now.ToString("HH:mm:ss")
                }));
            }
            catch { }
        }

        #endregion

        #region 服务方法

        /// <summary>
        /// Stops the service.
        /// </summary>
        private void StopService() {
            AsyncStackNet.Instance.EndAsync();

            //按钮控制
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;
            this.btnReset.Enabled = true;


            btnStartService.Enabled = true;
            btnStopService.Enabled = false;
            btnResetService.Enabled = true;
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        private void StartService() {
            AsyncStackNet.Instance.CrypterKey = TEACrypter.GetDefaultKey; 
            AsyncStackNet.Instance.ASyncSetupFile = new FileInfo(@"Setup.Bin").FullName;

            AsyncStackNet.Instance.AfterIntercept = new InterceptCallback(delegate(AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e)
            {
                AppendSessionLog(listView1, e.Session.userName, "AfterIntercept", Encoding.GetEncoding("gb2312").GetString(e.Session.UnOutPakcet(e.InPacket)), "SUCCESS");
                e.Session.SendPacket(__eTerm443Packet.BuildSessionPacket(e.Session.SID, e.Session.RID, "指令被禁止"));
            });


            AsyncStackNet.Instance.RID = 0x51;
            AsyncStackNet.Instance.SID = 0x27;
            AsyncStackNet.Instance.OnExecuteException += new EventHandler<ErrorEventArgs>(
                    delegate(object sender, ErrorEventArgs e)
                    {
                        AppendSessionLog(listView1, (sender as eTerm363Session).userName, "ExecuteException", e.GetException().Message, "ERROR");
                    }
                );

            AsyncStackNet.Instance.OnAsyncConnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e) {
                        appendASynConnect(e.Session);
                    }
                );

            AsyncStackNet.Instance.OnRateEvent += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        AsyncStackNet.Instance.BeginRateUpdate(new AsyncCallback(delegate(IAsyncResult iar) {
                            AsyncStackNet.Instance.EndRateUpdate(iar);
                        }));
                    }
                );

            AsyncStackNet.Instance.OnCoreConnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        UpdateStatusText(statusInfo, string.Format(@"与中心服务器{{{0}:{1}}}连接已连接！", AsyncStackNet.Instance.ASyncSetup.CoreServer, AsyncStackNet.Instance.ASyncSetup.CoreServerPort));
                    }
                );

            AsyncStackNet.Instance.OnCoreDisconnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        UpdateStatusText(statusInfo, string.Format(@"与中心服务器{{{0}:{1}}}连接已经断开！", AsyncStackNet.Instance.ASyncSetup.CoreServer, AsyncStackNet.Instance.ASyncSetup.CoreServerPort));
                    }
                );
            AsyncStackNet.Instance.OnSDKTimeout += new EventHandler<ErrorEventArgs>(
                    delegate(object sender, ErrorEventArgs e) {
                        UpdateStatusText(statusInfo, @"授权已到期！");
                    }
                );
            AsyncStackNet.Instance.OnSystemException += new EventHandler<ErrorEventArgs>(
                    delegate(object sender, ErrorEventArgs e) {
                        UpdateStatusText(statusInfo, e.GetException().Message);
                    }
                );
            AsyncStackNet.Instance.OnAsyncDisconnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        disconnectASync(e.Session);
                        //AppendSessionLog(listView2, e.Session.userName, "AsyncDisconnect", string.Empty, "SUCCESS");
                        //UpdateListByThread(lstAsync, "Circle_Green.png", e.Session.SessionId.ToString(), 0f, string.Empty);
                    }
                );

            AsyncStackNet.Instance.OnTSessionPacketSent += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e)
                    {
                        //UpdateListByThread(lstSession, "Circle_Green.png", e.Session.SessionId.ToString(), e.Session.TotalBytes, e.Session.TotalCount.ToString());
                    }
                );

            AsyncStackNet.Instance.OnAsyncPacketSent += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> e)
                    {
                        updateASync(e.Session);
                        //UpdateListByThread(lstAsync, "Circle_Red.png", e.Session.SessionId.ToString(), e.Session.TotalBytes, e.Session.TotalCount.ToString());
                    }
                );

            AsyncStackNet.Instance.OnAsyncReadPacket += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> e)
                    {
                        //UpdateListByThread(lstAsync, "Circle_Green.png", e.Session.SessionId.ToString(), e.Session.TotalBytes, e.Session.TotalCount.ToString());
                        AppendSessionLog(listView2, e.Session.userName, "AsyncReadPacket", Encoding.GetEncoding("gb2312").GetString(e.Session.UnInPakcet(e.OutPacket)), "SUCCESS");
                        if (e.Session.TSession == null) return;
                    }
                );
            AsyncStackNet.Instance.OnAsyncValidated += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Async> e)
                    {
                        UpdateStatusText(lableLocalIp, string.Format(@"本机IP：{0}", (e.Session.AsyncSocket.LocalEndPoint as IPEndPoint).Address.ToString()));
                        ListViewItem ViewItem = new ListViewItem(new string[] { e.Session.AsyncSocket.RemoteEndPoint.ToString(), e.Session.userName, e.Session.TotalBytes.ToString("f2"), e.Session.ReconnectCount.ToString(), e.Session.TotalBytes.ToString("f2") });
                        ViewItem.Name = e.Session.SessionId.ToString();
                        ViewItem.ImageKey = "Circle_Green.png";
                        e.Session.OnReadPacket += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                                delegate(object Session, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> SessionArg)
                                {
                                    //UpdateListByThread(lstAsync, "Circle_Green.png", SessionArg.Session.SessionId.ToString(), SessionArg.Session.TotalBytes, SessionArg.Session.TotalCount.ToString());
                                }
                            );
                        ViewItem.Tag = e.Session;
                        //ViewItem.Group = new ListViewGroup(;
                        //AppendListByThread(lstAsync, ViewItem);
                        AppendSessionLog(listView2, e.Session.userName, "AsyncValidated", "", "SUCCESS");
                    }
                );
            AsyncStackNet.Instance.OnTSessionValidated += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e) {
                        ListViewItem ViewItem = new ListViewItem(new string[] { e.Session.AsyncSocket.RemoteEndPoint.ToString(), e.Session.userName, e.Session.TotalBytes.ToString("f2"), e.Session.ReconnectCount.ToString() });
                        ViewItem.Name = e.Session.SessionId.ToString();
                        ViewItem.ImageKey = "Circle_Green.png";
                        ViewItem.Tag = e.Session;
                        //AppendListByThread(lstSession, ViewItem);
                    }
                );

            AsyncStackNet.Instance.OnTSessionAssign += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e)
                    {
                        //UpdateListByThread(lstSession, "Circle_Yellow.png", e.Session.SessionId.ToString(), e.Session.TotalBytes, e.Session.TotalCount.ToString());
                        //UpdateListByThread(lstAsync, "Circle_Yellow.png", e.Session.Async443.SessionId.ToString(), e.Session.Async443.TotalBytes, e.Session.Async443.TotalCount.ToString());
                    }
                );
            AsyncStackNet.Instance.OnTSessionAccept += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e)
                    {
                        AppendSessionLog(listView1, e.Session.SessionId.ToString(), "TSessionAccept", "", "SUCCESS");
                    }
                );
            AsyncStackNet.Instance.OnTSessionClosed += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e)
                    {
                        //UpdateListByThread(lstSession, "Circle_Red.png", e.Session.SessionId.ToString(), 0f, e.Session.TotalBytes.ToString("f2"));
                        AppendSessionLog(listView1, e.Session.userName, "TSessionClosed", "", "SUCCESS");
                    }
                );
            AsyncStackNet.Instance.OnAsyncTimeout += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        //stateAsync.Image = AsyncResource.Circle_Orange;
                        //SetToolbarImg(stateAsync, AsyncResource.Circle_Orange,string.Empty);
                        //Console.WriteLine("OnAsyncTimeout {0}", e.Session);
                    }
                );
            AsyncStackNet.Instance.OnTSessionRelease += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e)
                    {
                        AppendSessionLog(listView1, e.Session.userName, "TSessionRelease", "", "SUCCESS");
                        if (e.Session.Async443 != null && !e.Session.IsCompleted)
                            e.Session.SendPacket(__eTerm443Packet.BuildSessionPacket(e.Session.SID, e.Session.RID, "无数据返回或读取超时"));
                    }
                );

            AsyncStackNet.Instance.OnTSessionReadPacket += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e)
                    {
                        try {
                            //UpdateListByThread(lstSession, "Circle_Red.png", e.Session.SessionId.ToString(), e.Session.TotalBytes, e.Session.TotalCount.ToString());
                            AppendSessionLog(listView1, e.Session.userName, "ReadPacket", Encoding.GetEncoding("gb2312").GetString(e.Session.UnOutPakcet(e.InPacket)), "SUCCESS");
                        }
                        catch (Exception Ex) {
                            AppendSessionLog(listView1, e.Session.userName, "ReadPacket", Ex.Message, "ERROR");
                        }
                    }
                );


            LicenceManager.Instance.BeginValidate(new AsyncCallback(
                delegate(IAsyncResult iar)
                {
                    try {
                        if (!LicenceManager.Instance.EndValidate(iar)) {

                        }
                        else {
                            //激活配置
                            AsyncStackNet.Instance.BeginAsync();
                            UpdateStatusText(statusServer, string.Format(@"中心服务器为{{{0}:{1}}}", AsyncStackNet.Instance.ASyncSetup.CoreServer, AsyncStackNet.Instance.ASyncSetup.CoreServerPort));
                            if ((AsyncStackNet.Instance.ASyncSetup.AllowPlugIn ?? false)) {
                                AsyncStackNet.Instance.BeginReflectorPlugIn(new AsyncCallback(delegate(IAsyncResult iar1)
                                {
                                    AsyncStackNet.Instance.EndReflectorPlugIn(iar1);
                                }));
                            }
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }), new FileInfo(@"Key.Bin").FullName);
            if(Directory.Exists(AsyncStackNet.Instance.ASyncSetup.PlugInPath))
                loadAddIn(AsyncStackNet.Instance.ASyncSetup.PlugInPath);
            
            //按钮控制
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.btnReset.Enabled = true;

            btnStartService.Enabled = false;
            btnStopService.Enabled = true;
            btnResetService.Enabled = true;
        }
        #endregion

        #region 按钮事件
        /// <summary>
        /// Handles the Click event of the btnStart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnStart_Click(object sender, EventArgs e) {
            StartService();
        }

        /// <summary>
        /// Handles the Click event of the btnStop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnStop_Click(object sender, EventArgs e) {
            StopService();
        }


        /// <summary>
        /// Handles the Click event of the btnReset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnReset_Click(object sender, EventArgs e) {
            StopService();

            StartService();
        }
        #endregion

        #region UI更新
        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstAsync control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstAsync_SelectedIndexChanged(object sender, EventArgs e) {
            btnDispose.Enabled = lstAsync.SelectedItems.Count > 0;
            btnRelease.Enabled = lstAsync.SelectedItems.Count > 0;
        }

        /// <summary>
        /// Handles the Click event of the btnDispose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDispose_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in this.lstAsync.SelectedItems) {
                eTerm443Async Async = item.Tag as eTerm443Async;
                Async.Close();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnRelease control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnRelease_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in this.lstAsync.SelectedItems) {
                eTerm443Async Async = item.Tag as eTerm443Async;
                Async.TSession = null;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDisconnectSession control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDisconnectSession_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in this.lstSession.SelectedItems) {
                eTerm363Session Session = item.Tag as eTerm363Session;
                Session.Close();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstSession control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstSession_SelectedIndexChanged(object sender, EventArgs e) {
            btnSendMessage.Enabled = lstSession.SelectedItems.Count > 0;
            btnDisconnectSession.Enabled = lstSession.SelectedItems.Count > 0;
        }
        #endregion

        #region 加载UI插件

        /// <summary>
        /// 查询插件.
        /// </summary>
        private void loadAddIn(string PlugInPath) {
            foreach (FileInfo file in new DirectoryInfo(PlugInPath).GetFiles(@"*.AddIn", SearchOption.TopDirectoryOnly)) {
                LoadPlugIn(file);
            }
        }

        /// <summary>
        /// Loads the plug in.
        /// </summary>
        /// <param name="File">The file.</param>
        private void LoadPlugIn(FileInfo File) {
            Assembly ass;
            try {
                ass = Assembly.LoadFrom(File.FullName);
                foreach (Type t in ass.GetTypes()) {
                    foreach (Type i in t.GetInterfaces()) {
                        if (i.FullName == typeof(IAddIn).FullName) {
                            IAddIn plugIn = (IAddIn)System.Activator.CreateInstance(t);
                            BuildButton(plugIn);
                        }
                    }
                }
            }
            catch { throw; }
        }


        /// <summary>
        /// Builds the button.
        /// </summary>
        /// <param name="PlugIn">The plug in.</param>
        private void BuildButton(IAddIn PlugIn) {
            ButtonItem PlugInButton = new ButtonItem();

            PlugInButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            PlugInButton.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            if (!string.IsNullOrEmpty(PlugIn.ImageIcon))
                PlugInButton.Image = new Bitmap(new FileStream(PlugIn.ImageIcon, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            PlugInButton.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.F5);
            PlugInButton.Size = new System.Drawing.Size(158, 23);
            PlugInButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            PlugInButton.Text = PlugIn.ButtonName;
            PlugInButton.Tag = PlugIn;
            PlugInButton.Click += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        
                        dockAddIn.Visible = true;
                        dockAddIn.Control.Controls.Clear();
                        if (!string.IsNullOrEmpty(((sender as ButtonItem).Tag as BaseAddIn).ImageIcon))
                            dockAddIn.Image = (sender as ButtonItem).Image;
                        dockAddIn.Text = (sender as ButtonItem).Text;
                        ((sender as ButtonItem).Tag as BaseAddIn).ASyncSetup = LicenceManager.Instance.LicenceBody;
                        dockAddIn.Control.Controls.Add((sender as ButtonItem).Tag as BaseAddIn);
                    }
                );
            PlugInButton.MouseHover += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        PlugInBar.Text = ((sender as ButtonItem).Tag as BaseAddIn).ButtonName;
                    }
                );
            PlugInButton.ButtonStyle = eButtonStyle.ImageAndText;
            PlugInButton.Name = string.Format(@"btn_{0}", PlugIn.ButtonName);
            AppendAddInButton(PlugInButton);
        }

        /// <summary>
        /// Appends the add in button.
        /// </summary>
        /// <param name="PlugInButton">The plug in button.</param>
        private void AppendAddInButton(ButtonItem PlugInButton) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new AppendAddInButtonDelegate(AppendAddInButton), PlugInButton);
                return;
            }
            this.PlugInBar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            PlugInButton});
        }

        /// <summary>
        /// 线程UI代理
        /// </summary>
        private delegate void AppendAddInButtonDelegate(ButtonItem PlugInButton);
        #endregion

        private void btnSendMessage_Click(object sender, EventArgs e) {
            List<eTerm363Session> sessionLst = new List<eTerm363Session>();
            foreach (ListViewItem item in this.lstSession.SelectedItems) {
                sessionLst.Add(item.Tag as eTerm363Session);
            }
            new frmSender(sessionLst).ShowDialog();
        }

    }
}

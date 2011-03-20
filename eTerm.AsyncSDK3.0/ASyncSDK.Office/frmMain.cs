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
        public frmMain() {
            InitializeComponent();
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
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
        public delegate void AppendistViewItem(ListViewEx ViewEx, ListViewItem Item);

        public delegate void UpdateListViewItem(ListViewEx ViewEx, string ImageKey, string Id, float TotalBytes, string Reconnect);

        public delegate void AppendSessionLogDelegate(ListView View, string SessionName, string operationType, string SessionCommand, string flag);
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
        /// Appends the list by thread.
        /// </summary>
        /// <param name="ViewEx">The view ex.</param>
        /// <param name="Item">The item.</param>
        private void AppendListByThread(ListViewEx ViewEx, ListViewItem Item) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new AppendistViewItem(AppendListByThread), ViewEx, Item);
                return;
            }
            try {
                ViewEx.Items.Add(Item);
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

        /// <summary>
        /// Updates the list by thread.
        /// </summary>
        /// <param name="ViewEx">The view ex.</param>
        /// <param name="Item">The item.</param>
        /// <param name="Id">The id.</param>
        private void UpdateListByThread(ListViewEx ViewEx, string ImageKey, string Id, float TotalBytes, string Reconnect) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new UpdateListViewItem(UpdateListByThread), ViewEx, ImageKey, Id, TotalBytes, Reconnect);
                return;
            }
            try {
                if (TotalBytes <= 0) {
                    //if (!ViewEx.Items.Contains(new ListViewItem() {Name=Id })) return;
                    eTerm443Async Async = ViewEx.Items[Id].Tag as eTerm443Async;
                    ViewEx.Items[Id].Remove();
                    //Thread.Sleep(5000);
                    AsyncStackNet.Instance.ReconnectAsync(Async);
                    return;
                }
                ViewEx.Items[Id].SubItems[2].Text = TotalBytes.ToString("f2");
                ViewEx.Items[Id].SubItems[3].Text = Reconnect;
                ViewEx.Items[Id].ImageKey = ImageKey;
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

            //stateAsync.Visible = false;
            //stateAsync.Image = AsyncResource.Circle_Green;

            //按钮控制
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;
            this.btnReset.Enabled = true;


            btnStartService.Enabled = true;
            btnStopService.Enabled = false;
            btnResetService.Enabled = true;

            //this.toolStripStart.Enabled = true;
            //this.toolStripStop.Enabled = false;
            //this.toolStripReset.Enabled = true;

        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        private void StartService() {
            //stateAsync.Visible = true;
            //stateAsync.Image = AsyncResource.Circle_Green;


            //byte[] Buffer = new AsyncLicenceKey().XmlSerialize(keys, @"C:\XIAOFANG.Bin");
            //Key = Key.DeXmlSerialize(keys, Buffer);


            AsyncStackNet.Instance.ASyncSetupFile = new FileInfo(@"SETUP.BIN").FullName;

            AsyncStackNet.Instance.AfterIntercept = new InterceptCallback(delegate(AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e)
            {
                AppendSessionLog(listView1, e.Session.userName, "AfterIntercept", Encoding.GetEncoding("gb2312").GetString(e.Session.UnOutPakcet(e.InPacket)), "SUCCESS");
                e.Session.SendPacket(__eTerm443Packet.BuildSessionPacket(e.Session.SID, e.Session.RID, "指令被禁止"));
            });

            /*
            AsyncStackNet.Instance.AfterPacket = new AfterPacketCallback(delegate(AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> Args, PlugInSetup PlugIn)
            {
                PlugIn.PlugInstance.BeginExecute(new AsyncCallback(delegate(IAsyncResult iar)
                {
                    PlugIn.PlugInstance.EndExecute(iar);
                }), Args.Session, Args.InPacket, Args.OutPacket);
            });
            */
            //AsyncStackNet.Instance.StackNetPoint = new IPEndPoint(IPAddress.Any, 360);
            AsyncStackNet.Instance.RID = 0x51;
            AsyncStackNet.Instance.SID = 0x27;
            AsyncStackNet.Instance.CrypterKey = new byte[]{};
            AsyncStackNet.Instance.OnExecuteException += new EventHandler<ErrorEventArgs>(
                    delegate(object sender, ErrorEventArgs e)
                    {
                        AppendSessionLog(listView1, (sender as eTerm363Session).userName, "ExecuteException", e.GetException().Message, "ERROR");
                    }
                );

            AsyncStackNet.Instance.OnRateEvent += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        AsyncStackNet.Instance.BeginRateUpdate(new AsyncCallback(delegate(IAsyncResult iar) {
                            AsyncStackNet.Instance.EndRateUpdate(iar);
                        }));
                    }
                );

            AsyncStackNet.Instance.OnAsyncDisconnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        AppendSessionLog(listView2, e.Session.userName, "AsyncDisconnect", string.Empty, "SUCCESS");
                        //AsyncStackNet.Instance.ReconnectAsync(e.Session);
                        UpdateListByThread(lstAsync, "Circle_Green.png", e.Session.SessionId.ToString(), 0f, string.Empty);
                        //AsyncStackNet.Instance.ReconnectAsync(e.Session);
                        //SetToolbarImg(toolStripSplitPlugIn, AsyncResource.Circle_Red, string.Format(@"{0} 断开于 {1}", sender, DateTime.Now));
                    }
                );

            AsyncStackNet.Instance.OnTSessionPacketSent += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e)
                    {
                        UpdateListByThread(lstSession, "Circle_Green.png", e.Session.SessionId.ToString(), e.Session.TotalBytes, e.Session.TotalCount.ToString());
                    }
                );

            AsyncStackNet.Instance.OnAsyncPacketSent += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> e)
                    {
                        UpdateListByThread(lstAsync, "Circle_Red.png", e.Session.SessionId.ToString(), e.Session.TotalBytes, e.Session.TotalCount.ToString());
                    }
                );

            AsyncStackNet.Instance.OnAsyncReadPacket += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> e)
                    {
                        UpdateListByThread(lstAsync, "Circle_Green.png", e.Session.SessionId.ToString(), e.Session.TotalBytes, e.Session.TotalCount.ToString());
                        AppendSessionLog(listView2, e.Session.userName, "AsyncReadPacket", Encoding.GetEncoding("gb2312").GetString(e.Session.UnInPakcet(e.OutPacket)), "SUCCESS");
                        if (e.Session.TSession == null) return;
                    }
                );
            AsyncStackNet.Instance.OnAsyncValidated += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Async> e)
                    {
                        ListViewItem ViewItem = new ListViewItem(new string[] { e.Session.AsyncSocket.RemoteEndPoint.ToString(), e.Session.userName, e.Session.TotalBytes.ToString("f2"), e.Session.ReconnectCount.ToString(), e.Session.TotalBytes.ToString("f2") });
                        ViewItem.Name = e.Session.SessionId.ToString();
                        ViewItem.ImageKey = "Circle_Green.png";
                        e.Session.OnReadPacket += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                                delegate(object Session, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> SessionArg)
                                {
                                    UpdateListByThread(lstAsync, "Circle_Green.png", SessionArg.Session.SessionId.ToString(), SessionArg.Session.TotalBytes, SessionArg.Session.TotalCount.ToString());
                                }
                            );
                        ViewItem.Tag = e.Session;
                        AppendListByThread(lstAsync, ViewItem);
                        AppendSessionLog(listView2, e.Session.userName, "AsyncValidated", "", "SUCCESS");
                    }
                );
            AsyncStackNet.Instance.TSessionValidate = new AsyncBaseServer<eTerm363Session, eTerm363Packet>.ValidateCallback(delegate(eTerm363Session s, eTerm363Packet p)
            {
                s.UnpakcetSession(p);
                TSessionSetup TSession = AsyncStackNet.Instance.ASyncSetup.SessionCollection.Single<TSessionSetup>(Fun => Fun.SessionPass == s.userPass && Fun.SessionCode == s.userName && Fun.IsOpen == true);
                if (TSession == null) return false;
                s.TSessionInterval = TSession.SessionExpire;
                s.UnallowableReg = TSession.ForbidCmdReg;
                s.SpecialIntervalList = TSession.SpecialIntervalList;
                s.userGroup = TSession.GroupCode;
                ListViewItem ViewItem = new ListViewItem(new string[] { s.AsyncSocket.RemoteEndPoint.ToString(), s.userName, s.TotalBytes.ToString("f2"), s.ReconnectCount.ToString() });
                ViewItem.Name = s.SessionId.ToString();
                ViewItem.ImageKey = "Circle_Green.png";
                s.OnReadPacket += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                        delegate(object Session, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> SessionArg)
                        {
                            UpdateListByThread(lstSession, "Circle_Red.png", SessionArg.Session.SessionId.ToString(), SessionArg.Session.TotalBytes, SessionArg.Session.TotalCount.ToString());
                        }
                    );
                ViewItem.Tag = s;
                AppendListByThread(lstSession, ViewItem);

                return true;
            });

            AsyncStackNet.Instance.TSessionReconnectValidate = new AsyncBase<eTerm443Async, eTerm443Packet>.ValidateTSessionCallback(
                delegate(eTerm443Packet Packet, eTerm443Async Async)
                {
                    UpdateListByThread(lstSession, "Circle_Grey.png", Async.SessionId.ToString(), Async.TotalBytes, Async.TotalCount.ToString());
                    return Async.ReconnectCount <= 15;
                });

            AsyncStackNet.Instance.OnTSessionAssign += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e)
                    {
                        UpdateListByThread(lstSession, "Circle_Yellow.png", e.Session.SessionId.ToString(), e.Session.TotalBytes, e.Session.TotalCount.ToString());
                        UpdateListByThread(lstAsync, "Circle_Yellow.png", e.Session.Async443.SessionId.ToString(), e.Session.Async443.TotalBytes, e.Session.Async443.TotalCount.ToString());
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
                        UpdateListByThread(lstSession, "Circle_Red.png", e.Session.SessionId.ToString(), 0f, e.Session.TotalBytes.ToString("f2"));
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
                        //UpdateListByThread(lstSession, "Circle_Green.png", e.Session.SessionId.ToString(), e.Session.TotalBytes,e.Session.TotalBytes.ToString("f2"));
                        //UpdateListByThread(lstAsync, "Circle_Green.png", e.Session.Async443.SessionId.ToString(), e.Session.Async443.TotalBytes, e.Session.Async443.TotalCount.ToString());
                        if (e.Session.Async443 != null && !e.Session.IsCompleted)
                            e.Session.SendPacket(__eTerm443Packet.BuildSessionPacket(e.Session.SID, e.Session.RID, "无数据返回或读取超时"));
                        //Console.WriteLine("Session {0} Release {2} On {1}", e.Session, DateTime.Now, e.Session.Async443);
                    }
                );

            AsyncStackNet.Instance.OnTSessionReadPacket += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e)
                    {
                        //stateAsync.Image = AsyncResource.Circle_Yellow;
                        //SetToolbarImg(stateAsync, AsyncResource.Circle_Yellow,string.Empty);
                        try {
                            UpdateListByThread(lstSession, "Circle_Red.png", e.Session.SessionId.ToString(), e.Session.TotalBytes, e.Session.TotalCount.ToString());
                            AppendSessionLog(listView1, e.Session.userName, "ReadPacket", Encoding.GetEncoding("gb2312").GetString(e.Session.UnOutPakcet(e.InPacket)), "SUCCESS");
                        }
                        catch (Exception Ex) {
                            AppendSessionLog(listView1, e.Session.userName, "ReadPacket", Ex.Message, "ERROR");
                        }
                        //Console.WriteLine("OnReadPacket From {0} Packet Sequence:{1} Total Bytes:{2:f2} KBytes ", e.Session.AsyncSocket.RemoteEndPoint, e.InPacket.Sequence, e.Session.TotalBytes);
                    }
                );


            LicenceManager.Instance.BeginValidate(new AsyncCallback(
                delegate(IAsyncResult iar)
                {
                    try {
                        if (!LicenceManager.Instance.EndValidate(iar)) {
                            //Console.WriteLine("Validate Error");
                        }
                        else {
                            //激活配置
                            AsyncStackNet.Instance.BeginAsync();
                            loadAddIn(AsyncStackNet.Instance.ASyncSetup.PlugInPath);
                            AsyncStackNet.Instance.BeginReflectorPlugIn(new AsyncCallback(delegate(IAsyncResult iar1)
                            {
                                AsyncStackNet.Instance.EndReflectorPlugIn(iar1);
                            }));
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }), new FileInfo(@"Key.Bin").FullName);


            
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
            //if (this.lstAsync.SelectedItems.Count == 1) {
            //    this.btnDispose.Enabled = true;
            //    this.btnRelease.Enabled = true;
            //}
            //else {
            //    this.btnDispose.Enabled = false;
            //    this.btnRelease.Enabled = false;
            //}
            btnDispose.Enabled = lstAsync.SelectedItems.Count > 0;
            btnRelease.Enabled = lstAsync.SelectedItems.Count > 0;
        }

        /// <summary>
        /// Handles the Click event of the btnDispose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDispose_Click(object sender, EventArgs e) {
            //(this.lstAsync.SelectedItems[0].Tag as eTerm443Async).Close();
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

        private void btnDisconnectSession_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in this.lstSession.SelectedItems) {
                eTerm363Session Session = item.Tag as eTerm363Session;
                Session.Close();
            }
        }

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
            //PlugInButton.Dock = System.Windows.Forms.DockStyle.Top;
            if (!string.IsNullOrEmpty(PlugIn.ImageIcon))
                PlugInButton.Image = new Bitmap(new FileStream(PlugIn.ImageIcon, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            //PlugInButton.Location = new System.Drawing.Point(0, 0);
            PlugInButton.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.F5);
            PlugInButton.Size = new System.Drawing.Size(158, 23);
            //PlugInButton.TextAlignment = eButtonTextAlignment.Left;
            //PlugInButton.Padding = new System.Windows.Forms.Padding(20);
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
                        //MessageBox.Show((sender as ButtonX).Text);
                        
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
            //this.PlugInBar.Items.Add(PlugInButton);
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

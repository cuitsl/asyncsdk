using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ICSharpCode.TextEditor.Document;
using eTerm.AsyncSDK.Net;
using eTerm.AsyncSDK;
using System.Text.RegularExpressions;


namespace eTerm.ASyncActiveX {
    [Guid("7FCBBFE7-C95D-488E-B1A7-7978BB9E08C5"), ProgId("eTerm.ASyncActiveX.ASynClient"), ComVisible(true)]
    [ToolboxItem(true)]
    public partial class ASynClient : UserControl, IObjectSafety {

        private eTerm443Async __ClientSocket;

        #region 构造函数
        /// <summary>
        /// Initializes a new instance of the <see cref="ASynClient"/> class.
        /// </summary>
        public ASynClient() {
            InitializeComponent();

            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        this.textEditorControlWrapper1.Text = string.Empty;
                        this.textEditorControlWrapper1.BackColor = SystemColors.Control;
                        this.textEditorControlWrapper1.Dock = DockStyle.Fill;
                        this.textEditorControlWrapper1.ForeColor = Color.Green;
                        this.textEditorControlWrapper1.Location = new Point(0, 0);
                        this.textEditorControlWrapper1.SelectedText = "";
                        this.textEditorControlWrapper1.SelectionStart = 0;
                        this.textEditorControlWrapper1.ShowEOLMarkers = true;
                        this.textEditorControlWrapper1.ShowInvalidLines = false;
                        this.textEditorControlWrapper1.ShowLineNumbers = false;
                        this.textEditorControlWrapper1.ShowSpaces = true;
                        this.textEditorControlWrapper1.ShowTabs = true;
                        this.textEditorControlWrapper1.ShowVRuler = true;

                        this.textEditorControlWrapper1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("BAT");
                        this.textEditorControlWrapper1.ShowEOLMarkers = false;
                        this.textEditorControlWrapper1.ShowHRuler = false;
                        this.textEditorControlWrapper1.ShowMatchingBracket = false;
                        this.textEditorControlWrapper1.ShowVRuler = false;
                        this.textEditorControlWrapper1.ShowSpaces = false;
                        this.textEditorControlWrapper1.ShowTabs = false;
                        this.textEditorControlWrapper1.Enabled = false;
                    }
                );
        }
        #endregion

        #region IObjectSafety

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;

        public int GetInterfaceSafetyOptions(ref Guid riid,
                             ref int pdwSupportedOptions,
                             ref int pdwEnabledOptions) {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID) {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid,
                             int dwOptionSetMask,
                             int dwEnabledOptions) {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            switch (strGUID) {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) &&
                         (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) &&
                         (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }
        #endregion

        #region UI Thread
        public delegate void PacketPushCallback(string PacketString,bool flag,byte RowNumber,byte ColumnNumber);

        public delegate void ResetButtonEnableCallback(bool flag);

        /// <summary>
        /// Resets the button.
        /// </summary>
        /// <param name="flag">if set to <c>true</c> [flag].</param>
        private void ResetButton(bool flag) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new ResetButtonEnableCallback(ResetButton), flag);
                return;
            }
            try {
                this.btnConnect.Enabled = flag;
            }
            catch { }
        }

        /// <summary>
        /// Packets the push callback.
        /// </summary>
        /// <param name="PacketString">The packet string.</param>
        private void PacketPush(string PacketString, bool flag, byte RowNumber, byte ColumnNumber) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new PacketPushCallback(PacketPush), PacketString, flag,RowNumber,ColumnNumber);
                return;
            }
            try {
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Caret.Line = RowNumber - 0x20;
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Caret.Column = 0x00;
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.InsertString(PacketString); ;
                this.textEditorControlWrapper1.Enabled = flag;
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Caret.Line = RowNumber - 0x20 + Regex.Matches(PacketString, "\r", RegexOptions.Multiline | RegexOptions.IgnoreCase).Count+1;
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Caret.Column = 0x00;
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Select();
                
            }
            catch { }
        }
        #endregion

        #region 连接方法
        /// <summary>
        /// Handles the Click event of the btnConnect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnConnect_Click(object sender, EventArgs e) {
            //this.btnConnect.Enabled = false;
            eTerm.AsyncSDK.LicenceManager.Instance.BeginValidate(new AsyncCallback(delegate(IAsyncResult iar)
            {
                try {
                    ResetButton(false);
                    if (!eTerm.AsyncSDK.LicenceManager.Instance.EndValidate(iar)) {
                        //PacketPush(@"认证失败", false, 0x00, 0x00);
                    }
                    else {
                        //激活配置
                        //PacketPush(@"认证成功", true, 0x00, 0x00);
                        this.__ClientSocket = new eTerm443Async(this.txtAddress.Text, int.Parse(this.txtPort.Value.ToString()), this.txtUserName.Text.Trim(), this.txtPassword.Text.Trim(), 0x00, 0x00) { IsSsl = false };
                        this.__ClientSocket.OnAsynConnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                                delegate(object sender1, AsyncEventArgs<eTerm443Async> e1)
                                {
                                    //PacketPush(string.Format(@"会话{0}开始连接", e1.Session.SessionId), false, 0x00, 0x00);
                                }
                            );
                        this.__ClientSocket.OnValidated += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Async>>(
                                delegate(object sender1, AsyncEventArgs<eTerm443Packet, eTerm443Async> e1)
                                {
                                    //PacketPush(string.Format(@"会话{0}认证完成", e1.Session.SessionId), true, 0x00, 0x00);
                                    ResetButton(false);
                                    e1.Session.SendPacket(@"FD:SHACSX");
                                }
                            );
                        this.__ClientSocket.OnReadPacket += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                                delegate(object sender1, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> e1)
                                {
                                    PacketPush(Encoding.GetEncoding("gb2312").GetString(e1.Session.UnOutPakcet(e1.InPacket)), true, e1.InPacket.OriginalBytes[0x0e], e1.InPacket.OriginalBytes[0x0f]);
                                }
                            );
                        this.__ClientSocket.OnAsyncDisconnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                                delegate(object sender1, AsyncEventArgs<eTerm443Async> e1)
                                {
                                    //PacketPush(string.Format(@"会话{0}连接断开", e1.Session.SessionId), false, 0x00, 0x00);
                                    ResetButton(true);
                                }
                            );
                        this.__ClientSocket.Connect(this.txtAddress.Text, int.Parse(this.txtPort.Value.ToString()), false);
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }), @"D:\SouceCode\Personal\eTerm.AsyncSDK3.0\ASyncSDK.Office\bin\Release\Key.Bin");
        }
        #endregion
    }
}

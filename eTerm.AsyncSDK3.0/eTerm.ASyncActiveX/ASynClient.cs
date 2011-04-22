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
using eTerm.AsyncSDK.Util;


namespace eTerm.ASyncActiveX {
    [Guid("7FCBBFE7-C95D-488E-B1A7-7978BB9E08C5"), ProgId("eTerm.ASyncActiveX.ASynClient"), ComVisible(true)]
    [ToolboxItem(true)]
    public partial class ASynClient : UserControl, IObjectSafety {

        private eTerm443Async __ClientSocket;

        private const char SOE = '';

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

                        this.textEditorControlWrapper1.Font = new Font(@"Consolas", 12, FontStyle.Regular);

                        this.textEditorControlWrapper1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("BAT");
                        this.textEditorControlWrapper1.ShowEOLMarkers = false;
                        this.textEditorControlWrapper1.ShowHRuler = false;
                        this.textEditorControlWrapper1.ShowMatchingBracket = false;
                        this.textEditorControlWrapper1.ShowVRuler = false;
                        this.textEditorControlWrapper1.ShowSpaces = false;
                        this.textEditorControlWrapper1.ShowTabs = false;
                        this.textEditorControlWrapper1.Enabled = false;

                        textEditorControlWrapper1.ActiveTextAreaControl.TextArea.KeyUp += new KeyEventHandler(
                                delegate(object sender1, KeyEventArgs e1) {
                                    if (e1.KeyValue == 27) {
                                        textEditorControlWrapper1.ActiveTextAreaControl.TextArea.InsertString(SOE.ToString());
                                        return;
                                    }
                                    if (!(e1.Shift && e1.KeyValue == 13)) return;
                                    StringBuilder sbCmd = new StringBuilder();
                                    foreach (char keyValue in
                                                            from key in textEditorControlWrapper1.Text.ToCharArray().Reverse<char>()
                                                            select key) {
                                        if (keyValue == SOE) break;
                                        sbCmd.Append(keyValue);
                                    }
                                    string Command=sbCmd.ToString();
                                    sbCmd=new StringBuilder();
                                    foreach (char keyValue in Command.ToCharArray().Reverse<char>()) {
                                        sbCmd.Append(keyValue);
                                    }
                                    this.__ClientSocket.SendPacket(EnCodeBuffer(GbToUsas(sbCmd.Replace("\r\n", "\r").ToString())));
                                }
                            );
                    }
                );
        }
        #endregion

        #region 编码
        /// <summary>
        /// 编码需发送的数据包.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        private byte[] EnCodeBuffer(byte[] buffer) {
            //byte len;
            byte[] bytes1;
            List<byte> Command = new List<byte>();
            bytes1 = new byte[2];
            bytes1[0] = 1;
            Command.AddRange(bytes1);
            bytes1 = BitConverter.GetBytes((ushort)((0x13 + buffer.Length) + 2));
            Array.Reverse(bytes1);
            Command.AddRange(bytes1);
            //协议兼容
            //O157F4A1  O74523A1    O7452281    O7452291
            //Int32 tmp = Int32.Parse(this.RID, System.Globalization.NumberStyles.HexNumber);
            //Int32 tmp1 = 1; //Int32.Parse(base.userName.Substring(base.userName.Length - 1, 1), System.Globalization.NumberStyles.HexNumber);
            Command.AddRange(new byte[] { 0, 0, 0, 0x01, this.__ClientSocket.SID, this.__ClientSocket.RID, 0x70, 0x02, 0x1b, 0x0B, 0x2C, 0x20, 0, 0x0f, 0x1e });
            //len = Convert.ToByte((int)((0x13 + buffer.Length) + 2));
            Command.AddRange(buffer);
            Command.AddRange(new byte[] { 0x20, 0x03 });


            return Command.ToArray();
        }


        /// <summary>
        /// 中文编码到Usas转换.
        /// </summary>
        /// <param name="inputString">输入字符串.</param>
        /// <returns></returns>
        protected virtual byte[] GbToUsas(string inputString) {
            StringBuilder sb = new StringBuilder(inputString);
            foreach (Match m in Regex.Matches(inputString, @"[\u4e00-\u9fa5]+")) {
                sb.Replace(m.Value, string.Format(@"{0}{1}", Cn2PyUtil.FullConvert(m.Value), m.Value));
            }

            byte[] org = Encoding.GetEncoding("gb2312").GetBytes(sb.ToString());
            List<byte> result = new List<byte>();
            bool flag = false;
            foreach (byte b in org) {
                if (!flag && b > 128) {
                    result.AddRange(new byte[] { 0x1B, 0x0E });
                    flag = true;
                }
                else if (flag && b <= 128) {
                    result.AddRange(new byte[] { 0x1B, 0x0F });
                    flag = false;
                }
                if (flag) {
                    result.Add((byte)(b - 128));
                }
                else {
                    result.Add(b);
                }
            }
            if (flag)
                result.AddRange(new byte[] { 0x1B, 0x0F });
            return result.ToArray();
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
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.InsertString("\r");
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Caret.Line = textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Caret.Line + 1;
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Caret.Column = 0x00;
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.InsertString(PacketString + SOE);
                this.textEditorControlWrapper1.Enabled = flag;
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Caret.Line = textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Caret.Line  + Regex.Matches(PacketString, "\r", RegexOptions.Multiline | RegexOptions.IgnoreCase).Count+1;
                textEditorControlWrapper1.ActiveTextAreaControl.TextArea.Caret.Column = 1;
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
                        this.__ClientSocket = new eTerm443Async(this.txtAddress.Text, int.Parse(this.txtPort.Value.ToString()), this.txtUserName.Text.Trim(), this.txtPassword.Text.Trim(), 0x00, 0x00) { IsSsl = chkIsSsl.Checked };
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
                                    e1.Session.SendPacket(@"IG");
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
                        this.__ClientSocket.Connect(this.txtAddress.Text, int.Parse(this.txtPort.Value.ToString()), chkIsSsl.Checked);
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

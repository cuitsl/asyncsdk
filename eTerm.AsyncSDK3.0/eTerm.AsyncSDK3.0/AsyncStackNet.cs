using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Net;
using System.Net;
using eTerm.AsyncSDK.Core;
using eTerm.AsyncSDK.Base;
using System.Management;
using eTerm.AsyncSDK.Util;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace eTerm.AsyncSDK {

    #region 代理定义
    /// <summary>
    /// 插件查询异步代理
    /// </summary>
    public delegate void InvokePlugInCallback();

    /// <summary>
    /// 拦截处理代理服务
    /// </summary>
    public delegate void InterceptCallback(AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> Args);

    /// <summary>
    /// 配置后续结果代理服务
    /// </summary>
    public delegate void AfterPacketCallback(AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> Args, PlugInSetup PlugIn);

    /// <summary>
    /// 配置后续结果代理服务
    /// </summary>
    public delegate PlugInSetup AfterPacketValidCallback(eTerm443Packet InPakcet, eTerm443Packet OutPacket);
    #endregion

    /// <summary>
    /// 转发服务器类
    /// <remarks>
    ///     本类不可多副本运行，采用单例模式
    /// </remarks>
    /// </summary>
    public sealed class AsyncStackNet {

        #region 变量定义
        private static readonly AsyncStackNet __instance = new AsyncStackNet();
        private List<eTerm443Async> __asyncList = new List<eTerm443Async>();
        private eTermAsyncServer __asyncServer;
        private SystemSetup __Setup;
        private InvokePlugInCallback _Execute;
        private InvokePlugInCallback __RateExecute;
        private string __SetupFile = string.Empty;
        private Timer __rateAsync;

        /// <summary>
        /// Occurs when [on rate event].
        /// </summary>
        public event EventHandler OnRateEvent;
        #endregion

        #region 构造函数
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncStackNet"/> class.
        /// </summary>
        private AsyncStackNet() {
            _Execute = new InvokePlugInCallback(QueryPlugIn);
            __RateExecute = new InvokePlugInCallback(RateUpdate);
        }
        #endregion

        #region 属性定义
        /// <summary>
        /// 采用单例模式.
        /// </summary>
        /// <value>The instance.</value>
        public static AsyncStackNet Instance { get { return __instance; } }

        /// <summary>
        /// 服务器实体端口信息.
        /// </summary>
        /// <value>The stack net point.</value>
        public IPEndPoint StackNetPoint { private get; set; }

        /// <summary>
        /// 默认SID.
        /// </summary>
        /// <value>The SID.</value>
        public byte SID { set; private get; }

        /// <summary>
        /// 配置文件加解密KEY.
        /// </summary>
        /// <value>The crypter key.</value>
        public byte[] CrypterKey { set; get; }

        /// <summary>
        /// 系统配置文件路径设置.
        /// </summary>
        /// <value>The A sync setup.</value>
        public string ASyncSetupFile {
            set {
                using (FileStream fs = new FileStream(value, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    BinaryReader br = new BinaryReader(fs);
                    byte[] buffer = new byte[fs.Length];
                    br.Read(buffer, 0, buffer.Length);
                    __Setup = new SystemSetup().DeXmlSerialize(CrypterKey, buffer);
                    __SetupFile = value;
                }
            }
            get { return __SetupFile; }
        }

        /// <summary>
        /// 系统配置获取.
        /// </summary>
        /// <value>The A sync setup.</value>
        public SystemSetup ASyncSetup {
            get {
                return __Setup;
            }
        }

        /// <summary>
        /// 默认RID.
        /// </summary>
        /// <value>The RID.</value>
        public byte RID { set; private get; }

        /// <summary>
        /// Gets or sets the after intercept.
        /// </summary>
        /// <value>The after intercept.</value>
        public InterceptCallback AfterIntercept { private get; set; }

        #endregion

        #region Events
        /// <summary>
        /// 资源验证代理事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Async>> OnAsyncValidated;

        /// <summary>
        /// 资源读取事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm443Packet,eTerm443Packet, eTerm443Async>> OnAsyncReadPacket;

        /// <summary>
        /// 资源发送事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>> OnAsyncPacketSent;

        /// <summary>
        /// 资源超时事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm443Async>> OnAsyncTimeout;

        /// <summary>
        /// 资源断线事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm443Async>> OnAsyncDisconnect;

        /// <summary>
        /// 会话断线事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Session>> OnTSessionClosed;

        /// <summary>
        /// 新会话事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Session>> OnTSessionAccept;

        /// <summary>
        /// 会话读取事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Packet,eTerm363Packet, eTerm363Session>> OnTSessionReadPacket;

        /// <summary>
        /// 会话资源释放事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Session>> OnTSessionRelease;

        /// <summary>
        /// 资源分事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Session>> OnTSessionAssign;

        /// <summary>
        /// 数据发送事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>> OnTSessionPacketSent;

        /// <summary>
        /// 导常通知 .
        /// </summary>
        public event EventHandler<ErrorEventArgs> OnExecuteException;

        /// <summary>
        /// Fires the excetion.
        /// </summary>
        /// <param name="e">The <see cref="System.IO.ErrorEventArgs"/> instance containing the event data.</param>
        /// <param name="Session">The session.</param>
        private void FireExcetion(ErrorEventArgs e,eTerm363Session Session) {
            if (this.OnExecuteException != null)
                this.OnExecuteException(Session, e);
        }


        /// <summary>
        /// 会话认证代理.
        /// </summary>
        /// <value>The T session validate.</value>
        public AsyncBaseServer<eTerm363Session,eTerm363Packet>.ValidateCallback TSessionValidate { get; set; }

        /// <summary>
        /// 重连接次数验证.
        /// </summary>
        /// <value>The T session reconnect validate.</value>
        public AsyncBase<eTerm443Async, eTerm443Packet>.ValidateTSessionCallback TSessionReconnectValidate { get; set; }

        #endregion

        #region 重连机制
        /// <summary>
        /// Reconnects the async.
        /// </summary>
        /// <param name="Async">The async.</param>
        public void ReconnectAsync(eTerm443Async Async) {
            if (this.TSessionReconnectValidate != null && this.TSessionReconnectValidate(new eTerm443Packet(), Async)) {
                AppendAsync(
                    new eTerm443Async(Async.RemoteEP.Address.ToString(), Async.RemoteEP.Port, Async.userName, Async.userPass, (byte)Async.SID, (byte)Async.RID) { SiText = Async.SiText, IsSsl = Async.IsSsl, ReconnectCount = ++Async.ReconnectCount, GroupCode=Async.GroupCode }
                    );
            }
        }
        #endregion

        #region 配置管理
        /// <summary>
        /// Gets the active async.
        /// </summary>
        /// <param name="TSession">The T session.</param>
        /// <returns></returns>
        private void GetActiveAsync(eTerm363Session TSession) {
            if (TSession.Async443 != null) return ;
            lock (this.__asyncList) {
                foreach (var connect in
                        from entry in __asyncList
                        where entry.Connected && entry.SessionId > 0 && entry.TSession == null && entry.GroupCode == TSession.userGroup
                        orderby entry.TotalCount ascending
                        select entry) {
                    TSession.Async443 = connect;
                    connect.TSession = TSession;
                    if (OnTSessionAssign != null)
                        OnTSessionAssign(TSession, new AsyncEventArgs<eTerm363Session>(TSession));
                    return;
                }
            }
        }

        /// <summary>
        /// 通过配置文件添加配置.
        /// </summary>
        private void AppendAsync() {
            foreach (ConnectSetup T in ASyncSetup.AsynCollection) {
                if (!T.IsOpen) continue;
                AppendAsync(
                    new eTerm443Async(T.Address, T.Port, T.userName, T.userPass, (byte)T.SID, (byte)T.RID) { SiText = T.SiText, IsSsl = T.IsSsl,OfficeCode=T.OfficeCode, GroupCode=T.GroupCode });
            }
        }
        #endregion

        #region 追加活动配置
        /// <summary>
        /// 追加活动配置.
        /// </summary>
        /// <param name="Async">配置实体.</param>
        public void AppendAsync(eTerm443Async Async) {
            #region 授权校验
            if (__asyncList.Count >= LicenceManager.Instance.LicenceBody.MaxAsync) return;
            #endregion

            #region OnReadPacket
            Async.OnReadPacket += new EventHandler<eTerm.AsyncSDK.AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> e)
                    {
                        if (e.Session.TSession != null) {
                            byte[] PacketBytes = e.InPacket.OriginalBytes;
                            PacketBytes[8] = e.Session.TSession.SID;
                            PacketBytes[9] = e.Session.TSession.RID;
                            e.Session.TSession.IsCompleted = true;
                            e.Session.TSession.SendPacket(PacketBytes);
                        }
                        if (LicenceManager.Instance.LicenceBody.AllowAfterValidate) {
                            try {
                                string Command = Encoding.GetEncoding("gb2312").GetString(e.Session.UnInPakcet(e.OutPacket)).Trim().ToLower();
                                foreach (var PlugIn in
                                        from entry in AsyncStackNet.Instance.ASyncSetup.PlugInCollection
                                        where Command.ToLower().StartsWith(entry.PlugInName.ToLower())
                                        orderby entry.PlugInName ascending
                                        select entry) {
                                    if (PlugIn.ASyncInstance == null) continue;
                                    PlugIn.ASyncInstance.BeginExecute(new AsyncCallback(delegate(IAsyncResult iar)
                                    {
                                        PlugIn.ASyncInstance.EndExecute(iar);
                                    }), e.Session, e.InPacket, e.OutPacket, LicenceManager.Instance.LicenceBody);
                                }
                            }
                            catch { }

                        }
                        //e.OutPacket.OriginalBytes = e.Session.UnInPakcet(e.OutPacket);
                        //e.InPacket.OriginalBytes = e.Session.UnOutPakcet(e.InPacket);
                        if (this.OnAsyncReadPacket != null)
                            this.OnAsyncReadPacket(sender, e);
                    }
                );
            #endregion

            #region OnAsyncTimeout
            Async.OnAsyncTimeout += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        if (e.Session.TSession != null) {
                            e.Session.TSession.SendPacket(__eTerm443Packet.AsyncSocketTimeoutInfo(e.Session.TSession.SID, e.Session.TSession.RID));
                        }
                        if (this.OnAsyncTimeout != null)
                            this.OnAsyncTimeout(sender, e);
                    }
                );
            #endregion

            #region OnAsyncPacketSent
            Async.OnPacketSent += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> e) {
                        if (this.OnAsyncPacketSent != null)
                            this.OnAsyncPacketSent(sender, e);
                    }
                );
            #endregion

            #region OnValidated
            Async.OnValidated += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Async> e)
                    {
                        if (this.OnAsyncValidated != null)
                            this.OnAsyncValidated(sender, e);
                        this.__asyncList.Add(Async);
                    }
                );
            #endregion

            #region CallBack
            Async.TSessionReconnectValidate = TSessionReconnectValidate;
            #endregion

            #region OnAsyncDisconnect
            Async.OnAsyncDisconnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        try {
                            ConnectSetup Setup = this.ASyncSetup.AsynCollection[this.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup() { userName = e.Session.userName, Address = (e.Session.AsyncSocket.RemoteEndPoint as IPEndPoint).Address.ToString() })];
                            if (!Setup.Traffics.Contains(new SocketTraffic() { MonthString = DateTime.Now.ToString(@"yyyyMM") }))
                                Setup.Traffics.Add(new SocketTraffic() { MonthString = DateTime.Now.ToString(@"yyyyMM") });

                            SocketTraffic Traffic = Setup.Traffics[Setup.Traffics.IndexOf(new SocketTraffic() { MonthString = DateTime.Now.ToString(@"yyyyMM") })];
                            Traffic.Traffic = e.Session.TotalCount + (Traffic.Traffic ?? 0);
                        }
                        catch { }



                        this.__asyncList.Remove(sender as eTerm443Async);
                        if (this.OnAsyncDisconnect != null)
                            this.OnAsyncDisconnect(sender, e);
                        if (e.Session.TSession == null) return;
                        if (!e.Session.TSession.IsCompleted)
                            e.Session.TSession.SendPacket(__eTerm443Packet.BuildSessionPacket(e.Session.TSession.SID, e.Session.TSession.RID, "指令错误!"));
                        e.Session.TSession.Async443 = null;
                    }
                );
            #endregion

            #region 调用
            //this.__asyncList.Add(Async);
            Async.Connect();
            #endregion
        }
        #endregion

        #region 数据发送
        /// <summary>
        /// 给指令客户端发送数据包.
        /// </summary>
        /// <param name="TSession">指定会话.</param>
        /// <param name="Pakcet">数据包.</param>
        public void SendPacket(eTerm363Session TSession, byte[] Pakcet) {
            if (Pakcet.Length <= 0) return;
            __asyncServer.SendPacket(TSession, Pakcet);
        }
        #endregion

        #region 结束服务
        /// <summary>
        /// 结束服务.
        /// </summary>
        public void EndAsync() {
            lock (this) {
                foreach (eTerm443Async Async in this.__asyncList) {
                    Async.Dispose();
                }
                __asyncServer.Dispose();
                __asyncList.Clear();
            }
        }
        #endregion

        #region 开始服务
        /// <summary>
        /// 开始服务.
        /// </summary>
        public void BeginAsync() {
            if (!LicenceManager.Instance.ValidateResult) throw new OverflowException(__eTerm443Packet.AUTHERROR_MES);
            StackNetPoint = new IPEndPoint(IPAddress.Any, this.__Setup.ExternalPort??360);
            __asyncServer = new eTermAsyncServer(StackNetPoint, SID, RID);
            __asyncServer.MaxSession = LicenceManager.Instance.LicenceBody.MaxTSession;
            __asyncServer.OnPacketSent += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e)
                    {
                        if (OnTSessionPacketSent != null)
                            OnTSessionPacketSent(sender, e);
                    }
                );
            __asyncServer.OnReadPacket += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e)
                    {
                        if (LicenceManager.Instance.LicenceBody.AllowAfterValidate) {
                            try {
                                string Command = Encoding.GetEncoding("gb2312").GetString(e.Session.UnInPakcet(e.InPacket)).Trim().ToLower();
                                foreach (var PlugIn in
                                        from entry in AsyncStackNet.Instance.ASyncSetup.PlugInCollection
                                        where Command.ToLower().StartsWith(entry.PlugInName.ToLower())
                                        orderby entry.PlugInName ascending
                                        select entry) {
                                    if (PlugIn.ClientSessionInstance == null) continue;
                                    PlugIn.ClientSessionInstance.BeginExecute(new AsyncCallback(delegate(IAsyncResult iar)
                                    {
                                        PlugIn.ClientSessionInstance.EndExecute(iar);
                                    }), e.Session, e.InPacket, e.OutPacket, LicenceManager.Instance.LicenceBody);
                                    return;
                                }
                            }
                            catch(Exception ex) {
                                FireExcetion(new ErrorEventArgs( ex),e.Session);
                                //return;
                            }

                        }

                        GetActiveAsync(e.Session);
                        if (e.Session.Async443 == null) {
                            e.Session.SendPacket(__eTerm443Packet.NoAsyncSocketInfo(e.Session.SID, e.Session.RID));
                        }
                        else {
                            byte[] PacketBytes;
                            PacketBytes = e.InPacket.OriginalBytes;
                            PacketBytes[8] = e.Session.Async443.SID;
                            PacketBytes[9] = e.Session.Async443.RID;
                            e.Session.Async443.SendPacket(PacketBytes);
                        }
                        if (this.OnTSessionReadPacket != null)
                            this.OnTSessionReadPacket(sender, e);
                    }
                );
            __asyncServer.TSessionValidate = TSessionValidate;
            __asyncServer.OnTSessionAccept += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e)
                    {
                        if (this.OnTSessionAccept != null)
                            this.OnTSessionAccept(sender, e);
                        
                        e.Session.ReleaseIntervalSet = new ReleaseIntervalSetDelegate(delegate(string packet,eTerm363Session TSession) {
                            try {
                                if (!string.IsNullOrEmpty(TSession.SpecialIntervalList)) {
                                    string SpecialPacket = Regex.Replace(TSession.SpecialIntervalList, @"\d+\,", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                                    SpecialPacket = SpecialPacket.Substring(0, SpecialPacket.Length - 1);
                                    Match specialMatch = Regex.Match(packet, SpecialPacket, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                                    if (!string.IsNullOrEmpty(packet) && Regex.IsMatch(packet, SpecialPacket, RegexOptions.Multiline | RegexOptions.IgnoreCase)) {
                                        return int.Parse(Regex.Match(TSession.SpecialIntervalList, string.Format(@"\^({0})\|(\d+)", specialMatch.Value), RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups[2].Value);
                                    }
                                    return TSession.TSessionInterval;
                                }
                            }
                            catch { }
                            finally { }
                            return TSession.TSessionInterval;
                        });
                        
                        e.Session.OnTSessionRelease += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                                delegate(object Session, AsyncEventArgs<eTerm363Session> ie)
                                {
                                    if (OnTSessionRelease != null)
                                        OnTSessionRelease(Session, ie);
                                    ie.Session.SendPacket(__eTerm443Packet.BuildSessionPacket(ie.Session.SID, ie.Session.RID, "注意,配置已释放,指令上下文可能已经丢失."));
                                    if (ie.Session.Async443 == null) return;
                                    ie.Session.Async443.SendPacket("IG");
                                    ie.Session.Async443.TSession = null;
                                    ie.Session.Async443 = null;
                                }
                            );
                    }
                );
            __asyncServer.OnTSessionClosed += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e)
                    {
                        try {
                            TSessionSetup Setup = this.ASyncSetup.SessionCollection[this.ASyncSetup.SessionCollection.IndexOf(new TSessionSetup() { SessionCode = e.Session.userName })];
                            if (!Setup.Traffics.Contains(new SocketTraffic() { MonthString = DateTime.Now.ToString(@"yyyyMM") }))
                                Setup.Traffics.Add(new SocketTraffic() { MonthString = DateTime.Now.ToString(@"yyyyMM") });

                            SocketTraffic Traffic = Setup.Traffics[Setup.Traffics.IndexOf(new SocketTraffic() { MonthString = DateTime.Now.ToString(@"yyyyMM") })];
                            Traffic.Traffic = e.Session.TotalCount + (Traffic.Traffic ?? 0);
                        }
                        catch { }

                        if (this.OnTSessionClosed != null)
                            this.OnTSessionClosed(sender, e);
                        if (e.Session.Async443 == null) return;
                        e.Session.Async443.TSession = null;
                    }
                );
            __asyncServer.Start();
            __rateAsync = new Timer(
                                    new TimerCallback(
                                            delegate(object sender)
                                            {
                                                if (OnRateEvent != null)
                                                    OnRateEvent(sender, EventArgs.Empty);
                                            }),
                                        null, (this.ASyncSetup.StatisticalFrequency ?? 10 * 1000 * 60), (this.ASyncSetup.StatisticalFrequency ?? 10 * 1000 * 60));
            AppendAsync();
        }

        #endregion

        #region 插件处理
        /// <summary>
        /// 查询插件.
        /// </summary>
        /// <param name="callBack">The call back.</param>
        /// <returns></returns>
        public IAsyncResult BeginReflectorPlugIn(AsyncCallback callBack) {
            try {
                return _Execute.BeginInvoke(callBack, null);
            }
            catch (Exception e) {
                // Hide inside method invoking stack 
                throw e;
            }
        }

        /// <summary>
        /// 异步回调结束.
        /// </summary>
        /// <param name="iar">The iar.</param>
        /// <returns></returns>
        public void EndReflectorPlugIn(IAsyncResult iar) {
            if (iar == null)
                throw new NullReferenceException();
            try {
                _Execute.EndInvoke(iar);
                iar.AsyncWaitHandle.Close();
            }
            catch (Exception e) {
                // Hide inside method invoking stack 
                throw e;
            }
        }


        /// <summary>
        /// 查询插件.
        /// </summary>
        private void QueryPlugIn() {
            foreach (FileInfo file in new DirectoryInfo(this.ASyncSetup.PlugInPath).GetFiles(@"*.PlugIn", SearchOption.TopDirectoryOnly)) {
                LoadPlugIn(file);
            }
        }

        /// <summary>
        /// Rates the update.
        /// </summary>
        private void RateUpdate() {
            ASyncSetup.XmlSerialize(CrypterKey, ASyncSetupFile);
        }

        /// <summary>
        /// Begins the rate update.
        /// </summary>
        /// <param name="callBack">The call back.</param>
        public IAsyncResult BeginRateUpdate(AsyncCallback callBack) {
            try {
                return __RateExecute.BeginInvoke(callBack, null);
            }
            catch (Exception e) {
                // Hide inside method invoking stack 
                throw e;
            }
        }

        /// <summary>
        /// Ends the rate update.
        /// </summary>
        /// <param name="iar">The iar.</param>
        public void EndRateUpdate(IAsyncResult iar) {
            if (iar == null)
                throw new NullReferenceException();
            try {
                __RateExecute.EndInvoke(iar);
                iar.AsyncWaitHandle.Close();
            }
            catch (Exception e) {
                // Hide inside method invoking stack 
                throw e;
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
                        if (i.FullName == typeof(IAfterCommand<eTerm443Async, eTerm443Packet>).FullName) {
                            IAfterCommand<eTerm443Async, eTerm443Packet> plugIn = (IAfterCommand<eTerm443Async, eTerm443Packet>)System.Activator.CreateInstance(t);
                            object[] attris = t.GetCustomAttributes(typeof(AfterASynCommandAttribute), true);
                            foreach (AfterASynCommandAttribute att in attris) {
                                this.ASyncSetup.PlugInCollection.Add(new PlugInSetup() {
                                    AssemblyPath = File.FullName,
                                    PlugInName = att.ASynCommand,
                                    TypeFullName = i.FullName,
                                    ASyncInstance = plugIn
                                });
                            }
                        }
                        else if (i.FullName == typeof(IAfterCommand<eTerm363Session, eTerm363Packet>).FullName) {
                            IAfterCommand<eTerm363Session, eTerm363Packet> plugIn = (IAfterCommand<eTerm363Session, eTerm363Packet>)System.Activator.CreateInstance(t);
                            object[] attris = t.GetCustomAttributes(typeof(AfterASynCommandAttribute), true);
                            foreach (AfterASynCommandAttribute att in attris) {
                                this.ASyncSetup.PlugInCollection.Add(new PlugInSetup() {
                                    AssemblyPath = File.FullName,
                                    PlugInName = att.ASynCommand,
                                    TypeFullName = i.FullName,
                                    ClientSessionInstance = plugIn
                                });
                            }
                        }
                    }
                }
            }
            catch(Exception ex) {
                throw ex;
            }
        }
        #endregion
    }
}

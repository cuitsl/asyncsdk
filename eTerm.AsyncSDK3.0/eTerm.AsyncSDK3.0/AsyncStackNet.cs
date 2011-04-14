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
        private eTerm443Async __CoreASync = null;

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
            this.LocalEndPoint = new IPEndPoint(IPAddress.Any, 3900);
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
        /// 本地IP地址.
        /// </summary>
        /// <value>The local end point.</value>
        public IPEndPoint LocalEndPoint { get; private set; }

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
        public event EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>> OnAsyncReadPacket;

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
        /// 资源连接事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm443Async>> OnAsyncConnect;

        /// <summary>
        /// 会话断线事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Session>> OnTSessionClosed;

        /// <summary>
        /// 新会话事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Session>> OnTSessionAccept;

        /// <summary>
        /// 客户认证成功.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Session>> OnTSessionValidated;

        /// <summary>
        /// 会话读取事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>> OnTSessionReadPacket;

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
        /// 与中心服务器断开连接事件通知.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm443Async>> OnCoreDisconnect;

        /// <summary>
        /// 与中心服务器断开连接事件通知.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm443Async>> OnCoreConnect;

        /// <summary>
        /// 授授认证错误通知 .
        /// </summary>
        public event EventHandler<ErrorEventArgs> OnSystemException;

        /// <summary>
        /// 授权逾期事件通知.
        /// </summary>
        public event EventHandler<ErrorEventArgs> OnSDKTimeout;



        /// <summary>
        /// Fires the excetion.
        /// </summary>
        /// <param name="e">The <see cref="System.IO.ErrorEventArgs"/> instance containing the event data.</param>
        /// <param name="Session">The session.</param>
        private void FireExcetion(ErrorEventArgs e, eTerm363Session Session) {
            if (this.OnExecuteException != null)
                this.OnExecuteException(Session, e);
        }


        /// <summary>
        /// 会话认证代理.
        /// </summary>
        /// <value>The T session validate.</value>
        public AsyncBaseServer<eTerm363Session, eTerm363Packet>.ValidateCallback TSessionValidate { get; set; }


        #endregion

        #region 配置管理
        /// <summary>
        /// Gets the active async.
        /// </summary>
        /// <param name="TSession">The T session.</param>
        /// <returns></returns>
        private void GetActiveAsync(eTerm363Session TSession) {
            if (TSession.Async443 != null) return;
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
                    new eTerm443Async(T.Address, T.Port, T.userName, T.userPass, (byte)T.SID, (byte)T.RID) { /*LocalEP=new*/  SiText = T.SiText, IsSsl = T.IsSsl, OfficeCode = T.OfficeCode, GroupCode = T.GroupCode, AutoSi=T.AutoSi??false });
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

            Async.Instruction = AsyncStackNet.Instance.ASyncSetup.SequenceDirective;
            Async.IgInterval = AsyncStackNet.Instance.ASyncSetup.SequenceRate ?? 5;
            Async.ReconnectDelay = ASyncSetup.ReconnectDelay;

            #region OnAsynConnect
            Async.OnAsynConnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        if (OnAsyncConnect != null)
                            OnAsyncConnect(sender, e);
                        //this.LocalEndPoint = e.Session.AsyncSocket.LocalEndPoint as IPEndPoint;
                    }
                );
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
                        UpdateASyncSession(e.Session);
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
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> e)
                    {
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
                        string CurrentMonth = DateTime.Now.ToString(@"yyyyMM");
                        ConnectSetup TSession = ASyncSetup.AsynCollection.SingleOrDefault<ConnectSetup>(Fun => Fun.userPass ==e.Session.userPass && Fun.userName ==e.Session.userName);
                        if (!TSession.Traffics.Contains(new SocketTraffic(CurrentMonth)))
                            TSession.Traffics.Add(new SocketTraffic() { MonthString = CurrentMonth, Traffic = 0.0, UpdateDate = DateTime.Now });
                        SocketTraffic Traffic = TSession.Traffics[TSession.Traffics.IndexOf(new SocketTraffic(CurrentMonth))];
                        if (Traffic.Traffic >= TSession.FlowRate) {
                            e.Session.ObligatoryReconnect = false;
                            e.Session.Close();
                            return;
                        }
                        this.__asyncList.Add(Async);
                    }
                );
            #endregion

            #region CallBack
            Async.TSessionReconnectValidate =new AsyncBase<eTerm443Async,eTerm443Packet>.ValidateTSessionCallback(delegate(eTerm443Packet Packet,eTerm443Async ASync)
                {
                    return (ASyncSetup.AutoReconnect??false)&&Async.ReconnectCount<(ASyncSetup.MaxReconnect??10);
                });
            #endregion

            #region OnAsyncDisconnect
            Async.OnAsyncDisconnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        UpdateASyncSession(e.Session);
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
            Async.Connect(Async.Address, Async.Port, Async.IsSsl);
            #endregion
        }
        #endregion

        #region 连接中心指令服务器
        /// <summary>
        /// Connects the core.
        /// </summary>
        private void ConnectCore() {
            if (
                string.IsNullOrEmpty(AsyncStackNet.Instance.ASyncSetup.CoreServer)
                ||
                string.IsNullOrEmpty(AsyncStackNet.Instance.ASyncSetup.CoreAccount)
                ||
                string.IsNullOrEmpty(AsyncStackNet.Instance.ASyncSetup.CorePass)
                ||
                (AsyncStackNet.Instance.ASyncSetup.CoreServerPort ?? 0) == 0
            )
                throw new NotImplementedException(@"系统缺少中心指令服务器相关设置,请联系发开商!");
            ///TODO:初始化认证连接，认证频率计时器为：30分钟
            __CoreASync = new eTerm443Async(
                                ASyncSetup.CoreServer,
                                ASyncSetup.CoreServerPort.Value,
                                ASyncSetup.CoreAccount,
                                ASyncSetup.CorePass, 0x00, 0x00) { IsSsl = false, Instruction = string.Format(  @"!UpdateDate {0}",LicenceManager.Instance.SerialNumber), IgInterval = 30, ReconnectDelay=ASyncSetup.ReconnectDelay };
            __CoreASync.OnAsyncDisconnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        //this.LocalEndPoint = e.Session.AsyncSocket.LocalEndPoint as IPEndPoint;
                        if (this.OnCoreDisconnect != null)
                            this.OnCoreDisconnect(sender, e);
                    }
                );
            __CoreASync.TSessionReconnectValidate = new AsyncBase<eTerm443Async, eTerm443Packet>.ValidateTSessionCallback(
                delegate(eTerm443Packet Packet, eTerm443Async ASync)
                {
                    return true;
                });
            __CoreASync.OnAsynConnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e)
                    {
                        this.LocalEndPoint = e.Session.AsyncSocket.LocalEndPoint as IPEndPoint;
                        if (this.OnCoreConnect != null)
                            this.OnCoreConnect(sender, e);
                    }
                );
            __CoreASync.OnReadPacket += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> e)
                    {
                        try {
                            if (e.InPacket.OriginalBytes[0] == 0x00) return;
                            string coreDate = Regex.Match(Encoding.GetEncoding("gb2312").GetString(e.Session.UnOutPakcet(e.InPacket)), @"(\d{4}\-\d{1,2}\-\d{1,2})\s+\d{2,2}\:\d{2,2}\:\d{2,2}").Value;
                            DateTime serverDate = DateTime.Parse(coreDate);
                            if (((TimeSpan)(serverDate - DateTime.Now)).Days != 0) {
                                SystemUtil.SetSysTime(serverDate);
                                //日期比较
                                if (this.OnSystemException != null)
                                    this.OnSystemException(sender, new ErrorEventArgs(new DataMisalignedException(@"为防止授权错误，不允许手工修改系统日期，请联系发开发商")));
                                return;
                            }

                            if (
                                ((TimeSpan)(LicenceManager.Instance.LicenceBody.ExpireDate - DateTime.Now)).TotalDays <= 3
                                &&
                                ((TimeSpan)(LicenceManager.Instance.LicenceBody.ExpireDate - DateTime.Now)).TotalDays >= 1
                                ) {
                                if (this.OnSystemException != null)
                                    this.OnSystemException(sender, new ErrorEventArgs(new ArithmeticException(@"系统授权即将到期，如需继续使用请从开发商获取新授权")));
                                return;
                            }
                            else if (((TimeSpan)(LicenceManager.Instance.LicenceBody.ExpireDate - DateTime.Now)).TotalDays <= 0) {
                                LicenceManager.Instance.LicenceBody.RemainingMinutes = 0;
                                LicenceManager.Instance.LicenceBody.ExpireDate = serverDate;
                                BeginRateUpdate(new AsyncCallback(delegate(IAsyncResult iar)
                                {
                                    EndRateUpdate(iar);
                                }));
                                if (OnSDKTimeout != null)
                                    OnSDKTimeout(sender, new ErrorEventArgs(new TimeZoneNotFoundException(@"系统授权已到期，如需继续使用请从开发商获取新授权")));
                            }
                            else {
                                if (this.OnSystemException != null)
                                    this.OnSystemException(sender, new ErrorEventArgs(new ExecutionEngineException(@"认证信息验证完成")));
                            }
                        }
                        catch (Exception ex) {
                            if (this.OnSystemException != null)
                                this.OnSystemException(sender, new ErrorEventArgs(ex));
                        }
                    }
                );
            __CoreASync.Connect(ASyncSetup.CoreServer, ASyncSetup.CoreServerPort??350,false);
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
            StackNetPoint = new IPEndPoint(IPAddress.Any, this.__Setup.ExternalPort ?? 360);
            __asyncServer = new eTermAsyncServer(StackNetPoint, SID, RID);
            __asyncServer.MaxSession = LicenceManager.Instance.LicenceBody.MaxTSession;
            __asyncServer.OnPacketSent += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e)
                    {
                        if (OnTSessionPacketSent != null)
                            OnTSessionPacketSent(sender, e);
                    }
                );

            this.TSessionValidate = new AsyncBaseServer<eTerm363Session, eTerm363Packet>.ValidateCallback(delegate(eTerm363Session s, eTerm363Packet p, out string ValidateMessage)
            {
                s.UnpakcetSession(p);
                /*
                string value= Encoding.GetEncoding(@"gb2312").GetString( s.UnOutPakcet(new eTerm363Packet() { OriginalBytes=new byte[]{
                    0x00,0x37,0x00,0x31,0x30,0x30,0x30,0x31,0x3A,0x20,0xB5,0xC7,0xC2,0xBC,0xCA,0xA7
                            ,0xB0,0xDC,0xA3,0xBA,0xC7,0xEB,0xBC,0xEC,0xB2,0xE9,0xD3,0xC3,0xBB,0xA7,0xC3,0xFB 
                            ,0xBA,0xCD,0xBF,0xDA,0xC1,0xEE,0xA3,0xAC,0xBB,0xF2,0xD5,0xDF,0xC8,0xCF,0xD6,0xA4 
                            ,0xC0,0xE0,0xD0,0xCD,0xA3,0xA1,0x00 
                } }));
                */
                TSessionSetup TSession=ASyncSetup.SessionCollection.SingleOrDefault<TSessionSetup>(Fun => Fun.SessionPass == s.userPass && Fun.SessionCode == s.userName && Fun.IsOpen == true);
                if (TSession == null) { ValidateMessage = string.Format(@"{0} 登录帐号或密码错误", s.userName); return false; }
                //TSessionSetup TSession = AsyncStackNet.Instance.ASyncSetup.SessionCollection.Single<TSessionSetup>(Fun => Fun.SessionPass == s.userPass && Fun.SessionCode == s.userName && Fun.IsOpen == true);
                //if (__asyncServer.TSessionCollection.Count<eTerm363Session>(Session => Session.userName == s.userName) > 1) { ValidateMessage = string.Format(@"{0} 已经在其它IP登录", s.userName); return false; }

                s.TSessionInterval = TSession.SessionExpire;
                s.UnallowableReg = TSession.ForbidCmdReg;
                s.SpecialIntervalList = TSession.SpecialIntervalList;
                s.userGroup = TSession.GroupCode;
                ValidateMessage = string.Format(@"欢迎使用 {0} 共享终端,时限:{1}秒 {2}.", LicenceManager.Instance.LicenceBody.Company, s.TSessionInterval,DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss"));
                string currentMonth = string.Format(@"{0}", DateTime.Now.ToString(@"yyyyMM"));
                if (!TSession.Traffics.Contains(new SocketTraffic(currentMonth)))
                    TSession.Traffics.Add(new SocketTraffic() { MonthString = currentMonth, Traffic = 0.0, UpdateDate = DateTime.Now });
                SocketTraffic Traffic = TSession.Traffics[TSession.Traffics.IndexOf(new SocketTraffic(currentMonth))];
                if (Traffic.Traffic >= TSession.FlowRate) {
                    return false;
                }

                #region 关闭其它登录终端
                if (!(TSession.AllowDuplicate??false)) {
                    foreach (var connect in
                            from entry in __asyncServer.TSessionCollection
                            where entry.userName == s.userName && entry.SessionId != s.SessionId
                            orderby entry.LastActive ascending
                            select entry) {
                        connect.SendPacket(__eTerm443Packet.BuildSessionPacket(this.SID, this.RID, string.Format(@"登录退出[{0}],该帐号已在另外的地址登录[{1} {2}]", (connect.AsyncSocket.RemoteEndPoint as IPEndPoint).Address.ToString(), (s.AsyncSocket.RemoteEndPoint as IPEndPoint).Address.ToString(), DateTime.Now.ToString(@"MM dd HH:mm:ss"))));
                        connect.ObligatoryReconnect = false;
                        new Timer(new TimerCallback(
                            delegate(object sender)
                            {
                                connect.Close();
                            }), null, 1000, Timeout.Infinite);
                    }
                }
                #endregion

                /*
                s.OnReadPacket += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                        delegate(object Session, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> SessionArg)
                        {
                            if (this.OnTSessionReadPacket != null)
                                this.OnTSessionReadPacket(Session, SessionArg);
                        }
                    );
                */
                if (this.OnTSessionValidated != null)
                    this.OnTSessionValidated(s, new AsyncEventArgs<eTerm363Session>(s));
                return true;
            });

            //__asyncServer.OnTSessionValidated += new EventHandler<AsyncEventArgs<eTerm363Session>>(
            //        delegate(object sender, AsyncEventArgs<eTerm363Session> e) {
            //            if (this.OnTSessionValidated != null)
            //                this.OnTSessionValidated(sender, e);
            //            string currentMonth = string.Format(@"{0}", DateTime.Now.ToString(@"yyyyMM"));
            //            if (!TSession.Traffics.Contains(new SocketTraffic(currentMonth)))
            //                TSession.Traffics.Add(new SocketTraffic() { MonthString = currentMonth, Traffic = 0.0, UpdateDate = DateTime.Now });
            //            SocketTraffic Traffic = TSession.Traffics[TSession.Traffics.IndexOf(new SocketTraffic(currentMonth))];
            //            if (Traffic.Traffic >= TSession.FlowRate) {
            //                return false;
            //            }
            //        }
            //    );

            __asyncServer.OnReadPacket += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e)
                    {
                        string Command = Encoding.GetEncoding("gb2312").GetString(e.Session.UnInPakcet(e.InPacket)).Trim().ToLower();
                        if (this.OnTSessionReadPacket != null)
                            this.OnTSessionReadPacket(sender, e);
                        #region 指令拦截
                        if (!string.IsNullOrEmpty(e.Session.UnallowableReg)&& Regex.IsMatch(Command, e.Session.UnallowableReg, RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                            e.Session.SendPacket(__eTerm443Packet.BuildSessionPacket(e.Session.SID, e.Session.RID, string.Format(@"{0} 指令未授权", Command)));
                            return;
                        }
                        #endregion

                        UpdateSession(e.Session);

                        #region 后台处理插件
                        if (LicenceManager.Instance.LicenceBody.AllowAfterValidate) {
                            try {
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
                            catch (Exception ex) {
                                FireExcetion(new ErrorEventArgs(ex), e.Session);
                                //return;
                            }

                        }
                        #endregion

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
                    }
                );
            __asyncServer.TSessionValidate = TSessionValidate;
            __asyncServer.OnTSessionAccept += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e)
                    {
                        if (this.OnTSessionAccept != null)
                            this.OnTSessionAccept(sender, e);

                        e.Session.ReleaseIntervalSet = new ReleaseIntervalSetDelegate(delegate(string packet, eTerm363Session TSession)
                        {
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
                                    //UpdateSession(e.Session);
                                    //UpdateASyncSession(e.Session.Async443);
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
                        UpdateSession(e.Session);
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
                                        null, (this.ASyncSetup.StatisticalFrequency ?? 10) * 1000 * 60, (this.ASyncSetup.StatisticalFrequency ?? 10) * 1000 * 60);

            ConnectCore();

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
            if (!Directory.Exists(this.ASyncSetup.PlugInPath)) return;
            foreach (FileInfo file in new DirectoryInfo(this.ASyncSetup.PlugInPath).GetFiles(@"*.PlugIn", SearchOption.TopDirectoryOnly)) {
                LoadPlugIn(file);
            }
        }

        /// <summary>
        /// Rates the update.
        /// </summary>
        private void RateUpdate() {
            lock (this) {
                ASyncSetup.XmlSerialize(CrypterKey, ASyncSetupFile);
                LicenceManager.Instance.LicenceBody.RemainingMinutes -= (this.ASyncSetup.StatisticalFrequency ?? 10) / (1000 * 60);
                LicenceManager.Instance.LicenceBody.XmlSerialize(LicenceManager.Instance.SecreteKey, LicenceManager.Instance.AuthorizationFile);
            }
        }

        /// <summary>
        /// Updates the session.
        /// </summary>
        /// <param name="TSession">The T session.</param>
        private void UpdateSession(eTerm363Session TSession) {
            try {
                TSessionSetup Seup = ASyncSetup.SessionCollection.SingleOrDefault<TSessionSetup>(Fun => Fun.SessionPass == TSession.userPass && Fun.SessionCode == TSession.userName);
                SocketTraffic Traffic = Seup.Traffics[Seup.Traffics.IndexOf(new SocketTraffic(DateTime.Now.ToString(@"yyyyMM")))];
                Traffic.Traffic++;
                Traffic.UpdateDate = DateTime.Now;
            }
            catch { }
        }

        /// <summary>
        /// Updates the A sync session.
        /// </summary>
        /// <param name="ASync">The A sync.</param>
        private void UpdateASyncSession(eTerm443Async ASync) {
            try {
                ConnectSetup TSession = ASyncSetup.AsynCollection.SingleOrDefault<ConnectSetup>(Fun => Fun.userPass == ASync.userPass && Fun.userName == ASync.userName);
                SocketTraffic Traffic = TSession.Traffics[TSession.Traffics.IndexOf(new SocketTraffic(DateTime.Now.ToString(@"yyyyMM")))];
                Traffic.Traffic++;
                Traffic.UpdateDate = DateTime.Now;
            }
            catch { }
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
            catch (Exception ex) {
                throw ex;
            }
        }
        #endregion
    }
}
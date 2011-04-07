using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using eTerm.AsyncSDK.Net;
namespace eTerm.AsyncSDK.Base {
    /// <summary>
    /// 可连接会话基类
    /// </summary>
    /// <typeparam name="T">会话类型</typeparam>
    /// <typeparam name="P">数据包类型</typeparam>
    public abstract class AsyncBase<T,P>:_Session
    where T:_Session,new ()
    where P:_Packet<T>,new () {

        #region Delegate
        /// <summary>
        /// 客户端登录验证
        /// </summary>
        public delegate bool ValidateTSessionCallback(P Packet, T Tsession);
        #endregion

        #region Fields
        const int bufferSize = 512;//一次只读取512 byte
        byte[] data;
        P __inPacket = new P();
        P __outPacket = new P();
        long __sequence = 0;
        private const int __DefaultTimeout = 1000 * 10;
        ManualResetEvent allDone = new ManualResetEvent(true);
        #endregion

        #region Property
        /// <summary>
        /// 是否为SSL安全连接.
        /// </summary>
        /// <value><c>true</c> if this instance is SSL; otherwise, <c>false</c>.</value>
        public bool IsSsl { set; get; }

        /// <summary>
        /// 远程主机地址.
        /// </summary>
        /// <value>The remote ip.</value>
        public IPEndPoint RemoteEP {protected set;  get; }

        /// <summary>
        /// 远程主机地址
        /// </summary>
        public string Address {internal get; set; }

        /// <summary>
        /// 主机端口号.
        /// </summary>
        /// <value>The port.</value>
        public int Port { internal get; set; }

        /// <summary>
        /// 重连延时.
        /// </summary>
        /// <value>The reconnect delay.</value>
        public int? ReconnectDelay { set; protected get; }

        /// <summary>
        /// 是否允许重连.
        /// </summary>
        /// <value><c>true</c> if [obligatory reconnect]; otherwise, <c>false</c>.</value>
        internal bool ObligatoryReconnect { get; set; }

        /// <summary>
        /// 上一次收到的数据包.
        /// </summary>
        /// <value>The last packet.</value>
        public P LastPacket { get; protected set; }

        /// <summary>
        /// 结果包.
        /// </summary>
        /// <value>The packet.</value>
        protected P InPacket { get { return this.__inPacket; } set { this.__inPacket = value; } }

        /// <summary>
        /// 解包入口数据包.
        /// </summary>
        /// <param name="Pakcet">The pakcet.</param>
        /// <returns></returns>
        public virtual byte[] UnInPakcet(P Pakcet) {
            return Pakcet.OriginalBytes;
        }

        /// <summary>
        /// 指令包.
        /// </summary>
        /// <value>The out pakcet.</value>
        protected P OutPakcet { get { return this.__outPacket; } }

        /// <summary>
        /// 解包出口数据包.
        /// </summary>
        /// <param name="Pakcet">The pakcet.</param>
        /// <returns></returns>
        public virtual byte[] UnOutPakcet(P Pakcet) {
            return Pakcet.OriginalBytes;
        }

        /// <summary>
        /// 会话超时时间（单位：毫秒）.
        /// </summary>
        /// <value>The packet timeout.</value>
        public int? PacketTimeout { get; set; }

        /// <summary>
        /// 重连接次数.
        /// </summary>
        /// <value>The reconnect count.</value>
        public ushort ReconnectCount { get; set; }

        /// <summary>
        /// 重连接次数验证.
        /// </summary>
        /// <value>The T session reconnect validate.</value>
        public ValidateTSessionCallback TSessionReconnectValidate { get; set; }
        #endregion

        #region Events
        /// <summary>
        /// 已连接事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<T>> OnAsynConnect;

        /// <summary>
        /// 连接断开事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<T>> OnAsyncDisconnect;

        /// <summary>
        /// 数据收取事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<P,P, T>> OnReadPacket;

        /// <summary>
        /// 数据发送事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<P,P,T>> OnPacketSent;

        /// <summary>
        /// 数据通信超时事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<T>> OnAsyncTimeout;

        /// <summary>
        /// 认证代理.
        /// </summary>
        /// <value>The M_P certificate callback.</value>
        protected virtual RemoteCertificateValidationCallback m_pCertificateCallback { set; private get; }
        #endregion

        #region Connect
        /// <summary>
        /// 激活连接事件.
        /// </summary>
        protected virtual void FireOnAsynConnect() {
            if (this.OnAsynConnect != null)
                OnAsynConnect(this, new AsyncEventArgs<T>(this as T));
        }


        /// <summary>
        /// Connects the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="ssl">if set to <c>true</c> [SSL].</param>
        public virtual void Connect(string host, int port, bool ssl) {
            if (string.IsNullOrEmpty(host)) {
                throw new ArgumentException("Argument 'host' value may not be null or empty.");
            }
            if (port < 1) {
                throw new ArgumentException("Argument 'port' value must be >= 1.");
            }

            IPAddress[] ips = System.Net.Dns.GetHostAddresses(host);
            for (int i = 0; i < ips.Length; i++) {
                try {
                    //Connect(null, new IPEndPoint(ips[i], port), ssl);
                    this.RemoteEP = new IPEndPoint(ips[i], port);
                    this.IsSsl = ssl;
                    Connect();
                    break;
                }
                catch (System.Exception x) {
                    throw x;
                }
            }
        }

        /// <summary>
        /// 开始连接远程主机.
        /// </summary>
        public virtual void Connect() {
            AsyncSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                //AsyncSocket.Connect(this.RemoteEP);
                ReconnectCount++;
                AsyncSocket.BeginConnect(this.RemoteEP, new AsyncCallback(delegate(IAsyncResult iar) {
                    try {
                        Socket clientSocket = (Socket)iar.AsyncState;
                        clientSocket.EndConnect(iar);
                        if (clientSocket.Connected)
                            FireOnAsynConnect();
                        else
                            FireOnDisconnect();
                    }
                    catch (SocketException ex) { 
                        FireOnDisconnect(); 
#if DEBUG
                        WriteLog(ex);
#endif
                    }
                    catch (Exception ex) {
#if DEBUG
                        WriteLog(ex);
#endif
                    }
                }), AsyncSocket);
                
            }
            catch (Exception ex) {
#if DEBUG
                        WriteLog(ex);
#endif
            }
        }

        /// <summary>
        /// 建立主机通信流.
        /// <remarks>
        ///     主要区别是否为安全“SSL”连接建立网络流
        /// </remarks>
        /// </summary>
        protected virtual void BuildStream() {

            #region 在线侦听
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);//是否启用Keep-Alive
            BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));//多长时间开始第一次探测
            BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);//探测时间间隔
            AsyncSocket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
            #endregion

            if (IsSsl)
                this.AsyncStream = GetSslStream();
            else
                this.AsyncStream = new NetworkStream(this.AsyncSocket, true);

            //this.AsyncStream.ReadTimeout = 15 * 1000;
            //this.AsyncStream.WriteTimeout = 15 * 1000;

            ReadPacket();
        }

        /// <summary>
        /// 切换为SSL安全连接.
        /// </summary>
        /// <returns>是否成功切换</returns>
        protected virtual Stream GetSslStream() {
            SslStream _sslStream = new SslStream(new NetworkStream(this.AsyncSocket,true), true, RemoteCertificateValidationCallback);
            try {
                _sslStream.AuthenticateAsClient("eTerm.AsyncSDK3.0");
                if (_sslStream.IsAuthenticated)
                    this.AsyncStream = _sslStream;
#if DEBUG
                showSslInfo(this.RemoteEP.Address.ToString(), _sslStream, true);
#endif
            }
            catch (Exception ex) {
#if DEBUG
                        WriteLog(ex);
#endif
            }
            return _sslStream;
        }

        /// <summary>
        /// Remotes the certificate validation callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="chain">The chain.</param>
        /// <param name="sslPolicyErrors">The SSL policy errors.</param>
        /// <returns></returns>
        private bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            // User will handle it.
            if (m_pCertificateCallback != null) {
                return m_pCertificateCallback(sender, certificate, chain, sslPolicyErrors);
            }
            else {
                if (sslPolicyErrors == SslPolicyErrors.None || ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) > 0)) {
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncBase&lt;T, P&gt;"/> class.
        /// </summary>
        public AsyncBase() {
            if (!LicenceManager.Instance.ValidateResult) throw new OverflowException(__eTerm443Packet.AUTHERROR_MES);
            this.ObligatoryReconnect = true;
        }
        #endregion

        #region DEBUG
        /// <summary>
        /// Shows the certificate info.
        /// </summary>
        /// <param name="remoteCertificate">The remote certificate.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        private void showCertificateInfo(X509Certificate remoteCertificate, bool verbose) {
            Console.WriteLine("Certficate Information for:\n{0}\n", remoteCertificate.GetName());
            Console.WriteLine("Valid From: \n{0}", remoteCertificate.GetEffectiveDateString());
            Console.WriteLine("Valid To: \n{0}", remoteCertificate.GetExpirationDateString());
            Console.WriteLine("Certificate Format: \n{0}\n", remoteCertificate.GetFormat());

            Console.WriteLine("Issuer Name: \n{0}", remoteCertificate.GetIssuerName());

            if (verbose) {
                Console.WriteLine("Serial Number: \n{0}", remoteCertificate.GetSerialNumberString());
                Console.WriteLine("Hash: \n{0}", remoteCertificate.GetCertHashString());
                Console.WriteLine("Key Algorithm: \n{0}", remoteCertificate.GetKeyAlgorithm());
                Console.WriteLine("Key Algorithm Parameters: \n{0}", remoteCertificate.GetKeyAlgorithmParametersString());
                Console.WriteLine("Public Key: \n{0}", remoteCertificate.GetPublicKeyString());
            }
        }


        /// <summary>
        /// Shows the SSL info.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="sslStream">The SSL stream.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        private void showSslInfo(string serverName, SslStream sslStream, bool verbose) {
            showCertificateInfo(sslStream.RemoteCertificate, verbose);

            Console.WriteLine("\n\nSSL Connect Report for : {0}\n", serverName);
            Console.WriteLine("Is Authenticated: {0}", sslStream.IsAuthenticated);
            Console.WriteLine("Is Encrypted: {0}", sslStream.IsEncrypted);
            Console.WriteLine("Is Signed: {0}", sslStream.IsSigned);
            Console.WriteLine("Is Mutually Authenticated: {0}\n", sslStream.IsMutuallyAuthenticated);

            Console.WriteLine("Hash Algorithm: {0}", sslStream.HashAlgorithm);
            Console.WriteLine("Hash Strength: {0}", sslStream.HashStrength);
            Console.WriteLine("Cipher Algorithm: {0}", sslStream.CipherAlgorithm);
            Console.WriteLine("Cipher Strength: {0}\n", sslStream.CipherStrength);

            Console.WriteLine("Key Exchange Algorithm: {0}", sslStream.KeyExchangeAlgorithm);
            Console.WriteLine("Key Exchange Strength: {0}\n", sslStream.KeyExchangeStrength);
            Console.WriteLine("SSL Protocol: {0}", sslStream.SslProtocol);
        }
        #endregion

        #region 接收数据
        /// <summary>
        /// 发送数据包(上次发送后未取回数据包时不可再发送数据包).
        /// </summary>
        public virtual void ReadPacket() {
            try {
                data = new byte[bufferSize];
                //异步读取文件,把FileStream对象作为异步的参数// <-
                if (AsyncStream.CanRead) {
                    LastPacket = __inPacket;
                    data = new byte[bufferSize];
                    IAsyncResult result= AsyncStream.BeginRead(data, 0, bufferSize,
                                                                 new AsyncCallback(OnReadCompletion),
                                                                 AsyncStream);
                    if (!result.AsyncWaitHandle.SafeWaitHandle.IsClosed) {
                        // this line impliments the timeout, if there is a timeout, the callback fires and the request becomes aborted
                        ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(
                                delegate(object state, bool timedOut)
                                {
                                    if (timedOut) {
                                        if (this.OnAsyncTimeout != null)
                                            this.OnAsyncTimeout(this, new AsyncEventArgs<T>(this as T));
                                    }
                                }
                            ), AsyncStream, PacketTimeout ?? __DefaultTimeout, true);
                    }
                    // The response came in the allowed time. The work processing will happen in the 
                    // callback function.
                    allDone.WaitOne();
                }
                else {
#if DEBUG
                    Console.WriteLine("Sorry.  You cannot read from this NetworkStream.");
#endif
                }
            }
            catch (SocketException ex) {
                /*
                 1、客户端程序异常。
　　             对于这种情况，我们很好处理，因为客户端程序异常退出会在服务端引发ConnectionReset的Socket异常（就是WinSock2中的10054异常）。
                     只要在服务端处理这个异常就可以了。 
                 2、网络链路异常。
                      如：网线拔出、交换机掉电、客户端机器掉电。当出现这些情况的时候服务端不会出现任何异常。
                      这样的话上面的代码就不能处理这种情况了。对于这种情况在MSDN里面是这样处理的，我在这里贴出MSDN的原文：
                      如果您需要确定连接的当前状态，请进行非阻止、零字节的 Send 调用。
                      如果该调用成功返回或引发 WAEWOULDBLOCK 错误代码 (10035)，则该套接字仍然处于连接状态；否则，该套接字不再处于连接状态。
                */
                if (ex.ErrorCode == 10054)
                    FireOnDisconnect();
#if DEBUG
                        WriteLog(ex);
#endif
            }
            catch (Exception ex) {
#if DEBUG
                        WriteLog(ex);
#endif
            }
        }

        /// <summary>
        /// 激活断线事件通知.
        /// </summary>
        protected virtual void FireOnDisconnect() {
            Dispose();
            if (this.OnAsyncDisconnect != null)
                this.OnAsyncDisconnect(this, new AsyncEventArgs<T>(this as T));
            __sequence = 0;
            //Thread.Sleep(60 * 1000 * 5);
            if (this.ObligatoryReconnect&& this.TSessionReconnectValidate != null && this.TSessionReconnectValidate(new P(), this as T)) {
                new Timer(new TimerCallback(
                    delegate(object sender)
                    {
                        Connect();
                    }),null,(ReconnectDelay??1)*1000,Timeout.Infinite);
            }
        }

        /// <summary>
        /// 关闭连接.
        /// </summary>
        public virtual void Close() {
            FireOnDisconnect();
        }

        /// <summary>
        /// Called when [read completion].
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        void OnReadCompletion(IAsyncResult asyncResult)
        {
            Stream fs = asyncResult.AsyncState as Stream;
            try {
                // Shutdown 时将调用 ReceiveData，此时也可能收到 0 长数据包
                int readBytesLength = fs.EndRead(asyncResult);
                asyncResult.AsyncWaitHandle.Close();

                if (readBytesLength == 0) {
                    FireOnDisconnect();
                }
                else  // 正常数据包
                    {
                    // 合并报文，按报文头、尾字符标志抽取报文，将包交给数据处理器
                    base.TotalBytes += readBytesLength / 1024.0f;
                    base.CurrentBytes = readBytesLength;
                    base.LastActive = DateTime.Now;
                    __inPacket.Put(data, 0, readBytesLength);
                    if (readBytesLength < bufferSize && readBytesLength > 0 && __inPacket.ValidatePacket()) {
                        this.__inPacket.PacketDateTime = DateTime.Now;
                        Interlocked.Increment(ref __sequence);
                        this.__inPacket.Sequence = __sequence;
                        base.TotalCount++;
                        this.__inPacket.Session = this as T;
                        FireOnPacketReceive();
                    }
                    //IAsyncResult async = AsyncStream.BeginRead(data, 0, bufferSize,new AsyncCallback(OnReadCompletion), AsyncStream); // <-
                }
                //继续接收来自来客户端的数据 
                fs.BeginRead(data, 0, bufferSize, new AsyncCallback(OnReadCompletion), fs);

            }
            catch (SocketException ex) {
                if (ex.ErrorCode == 10054) {
                    FireOnDisconnect();
                    //Dispose();
                }
#if DEBUG
                        WriteLog(ex);
#endif
            }
            catch (IOException ex) {
                FireOnDisconnect();
                //Dispose();
#if DEBUG
                        WriteLog(ex);
#endif
            }
            catch (Exception ex) {  // 读 socket 异常，关闭该会话，系统不认为是错误（这种错误可能太多）
                //throw;
#if DEBUG
                        WriteLog(ex);
#endif
            }
            allDone.Set();
        }

        /// <summary>
        /// 激活数据收取事件通知.
        /// </summary>
        protected virtual void FireOnPacketReceive() {
            //if (__Packet == null) 
            if (OnReadPacket != null)
                OnReadPacket(this, new AsyncEventArgs<P,P, T>(this.__inPacket,this.__outPacket, this as T));
            this.__inPacket.Dispose();
            __inPacket = new P();
        }
        #endregion

        #region 读取数据
        /// <summary>
        /// 发送字节流.
        /// </summary>
        /// <param name="Packet">The packet.</param>
        public virtual void SendPacket(byte[] Packet) {
            try {
                if (AsyncStream.CanWrite) {
                    __outPacket = new P();
                    __outPacket.Put(Packet, 0, Packet.Length);
                    //异步读取文件,把FileStream对象作为异步的参数// <-
                    AsyncCallback callback = new AsyncCallback(OnWriteCompletion);
                    IAsyncResult async = AsyncStream.BeginWrite(Packet, 0, Packet.Length, callback, AsyncStream); // <-
                }
            }
            catch (IOException ex) { 
                FireOnDisconnect();
#if DEBUG
                        WriteLog(ex);
#endif
            }
            catch (Exception ex) {
#if DEBUG
                        WriteLog(ex);
#endif
            }
        }

        /// <summary>
        /// 发送字符串流.
        /// <remarks>
        ///     当需要时重写
        /// </remarks>
        /// </summary>
        /// <param name="Packet">The packet.</param>
        public abstract void SendPacket(string Packet);

        /// <summary>
        /// Called when [write completion].
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        void OnWriteCompletion(IAsyncResult asyncResult) {
            try {
                Stream fs = asyncResult.AsyncState as Stream;
                fs.EndWrite(asyncResult);
                asyncResult.AsyncWaitHandle.Close();
                FireOnPacketSent();
            }
            catch { }
        }

        /// <summary>
        /// 激活发送流事件通知.
        /// </summary>
        protected virtual void FireOnPacketSent() {
            if (OnPacketSent != null)
                OnPacketSent(this, new AsyncEventArgs<P, P, T>(__inPacket, __outPacket, this as T));
        }
        #endregion
    }
}

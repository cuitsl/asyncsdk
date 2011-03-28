using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
namespace eTerm.AsyncSDK.Base {
    /// <summary>
    /// 服务器端实例基类
    /// </summary>
    /// <typeparam name="T">会话端类型</typeparam>
    /// <typeparam name="P">会话端数据包类型</typeparam>
    public abstract class AsyncBaseServer<T, P> : _Session
        where T : AsyncBase<T,P>, new()
        where P : _Packet<T>, new() {

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncBaseServer&lt;T, P&gt;"/> class.
        /// </summary>
        public AsyncBaseServer():base() { 
            
        }

        #region Fields
        const int bufferSize = 512;//一次只读取512 byte
        P __Packet = new P();
        List<T> __TSessionCollection = new List<T>();
        private const int __MAXSESSION = 50;
        //private Socket __SvrSocket;
        private bool __isRun = false;
        #endregion

        #region 事件定义
        /// <summary>
        /// 会话端到达最大连接数.
        /// </summary>
        public event EventHandler<AsyncEventArgs<T>> OnTSessionFull;

        /// <summary>
        /// 会话端关闭事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<T>> OnTSessionClosed;

        /// <summary>
        /// 新会话端事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<T>> OnTSessionAccept;

        /// <summary>
        /// 数据收取事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<P,P, T>> OnReadPacket;


        /// <summary>
        /// 数据发送事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<P, P, T>> OnPacketSent;

        #endregion

        /// <summary>
        /// 客户端登录逻辑
        /// </summary>
        public delegate bool ValidateCallback(T TSession, P Packet);

        #region 属性
        /// <summary>
        /// 绑定地址.
        /// </summary>
        /// <value>The async end point.</value>
        public IPEndPoint AsyncEndPoint { protected get; set; }

        /// <summary>
        /// 客户端认证成功.
        /// </summary>
        public event EventHandler<AsyncEventArgs<T>> OnTSessionValidated;

        /// <summary>
        /// 最大连接数.
        /// </summary>
        /// <value>The max session.</value>
        public int? MaxSession { get; set; }

        /// <summary>
        /// 会话集合.
        /// </summary>
        /// <value>The T session collection.</value>
        public List<T> TSessionCollection { get { return this.__TSessionCollection; } protected set { this.__TSessionCollection = value; } }

        /// <summary>
        /// 当前会话端总数.
        /// </summary>
        /// <value>The T session count.</value>
        public int TSessionCount { get { return this.__TSessionCollection.Count; } }

        /// <summary>
        /// 会话端认证代理.
        /// </summary>
        /// <value>The T session validate.</value>
        public ValidateCallback TSessionValidate { get; set; }

        /// <summary>
        /// Fires the T session validated.
        /// </summary>
        /// <param name="TSession">The T session.</param>
        protected virtual void FireTSessionValidated(T TSession) {
            if (this.OnTSessionValidated != null)
                this.OnTSessionValidated(TSession, new AsyncEventArgs<T>(TSession));
        }
        #endregion

        #region 发送数据 
        /// <summary>
        /// 向指令会话端发送字节流.
        /// </summary>
        /// <param name="TSession">指定会话端.</param>
        /// <param name="Packet">字节流.</param>
        public virtual void SendPacket(T TSession, byte[] Packet) { 
            
        }

        /// <summary>
        /// 向指定会话面发送字符串流.
        /// </summary>
        /// <param name="TSession">指定会话端.</param>
        /// <param name="Packet">字符流.</param>
        public abstract void SendPacket(T TSession, string Packet);
        #endregion

        #region 开启服务
        /// <summary> 
        /// 停止服务器程序,所有与客户端的连接将关闭 
        /// </summary> 
        public virtual void Stop() {
            if (!__isRun) {
                throw (new ApplicationException("TcpSvr已经停止"));
            }

            //这个条件语句，一定要在关闭所有客户端以前调用 
            //否则在EndConn会出现错误 
            __isRun = false;

            Close();

            this.__TSessionCollection = new List<T>();

        }

        /// <summary>
        /// 开启服务实例.
        /// </summary>
        public virtual void Start() {
            if (__isRun) throw new NotSupportedException("服务已经启动，无法重复启动！");
            //初始化socket 
            AsyncSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            //绑定端口 
            AsyncSocket.Bind(AsyncEndPoint);
            //开始监听 
            AsyncSocket.Listen(5);

            //设置异步方法接受客户端连接 
            AsyncSocket.BeginAccept(new AsyncCallback(EndAcceptConnect), AsyncSocket);

            __isRun = true;

        }

        /// <summary>
        /// Ends the accept connect.
        /// </summary>
        /// <param name="iar">The iar.</param>
        private void EndAcceptConnect(IAsyncResult iar) {
            //接受一个客户端的连接请求 
            try {
                Socket oldserver = (Socket)iar.AsyncState;
                Socket client = oldserver.EndAccept(iar);
                T TSession = new T() { AsyncSocket = client };
                TSession.OnAsyncDisconnect += new EventHandler<AsyncEventArgs<T>>(
                        delegate(object sender, AsyncEventArgs<T> e)
                        {
                            if (this.OnTSessionClosed != null)
                                this.OnTSessionClosed(sender, e);
                            Close(e.Session);
                        }
                    );
                TSession.OnAsynConnect += new EventHandler<AsyncEventArgs<T>>(
                        delegate(object sender, AsyncEventArgs<T> e)
                        {
                            if (this.OnTSessionAccept != null)
                                this.OnTSessionAccept(sender, e);
                        }
                    );
                TSession.OnReadPacket += new EventHandler<AsyncEventArgs<P, P, T>>(
                        delegate(object sender, AsyncEventArgs<P, P, T> e)
                        {
                            FireOnReadPacket(e);
                        }
                    );
                TSession.OnPacketSent += new EventHandler<AsyncEventArgs<P, P, T>>(
                        delegate(object sender, AsyncEventArgs<P, P, T> e)
                        {
                            if (this.OnPacketSent != null)
                                this.OnPacketSent(sender, e);
                        }
                    );
                if ((this.MaxSession ?? 50) <= this.TSessionCount) {
                    if (this.OnTSessionFull != null)
                        this.OnTSessionFull(TSession, new AsyncEventArgs<T>(TSession));
                    return;
                }
                TSession.Connect();//开始读取线程
                lock (__TSessionCollection) {
                    this.__TSessionCollection.Add(TSession);
                }
                //继续接受客户端 
                AsyncSocket.BeginAccept(new AsyncCallback(EndAcceptConnect), AsyncSocket);
            }
            catch { return; }
        }

        /// <summary>
        /// 激活读取事件通知.
        /// </summary>
        protected virtual void FireOnReadPacket(AsyncEventArgs<P,P, T> e) {
            if (this.OnReadPacket != null)
                this.OnReadPacket(e.Session, e);
        }
        #endregion

        #region 关闭客户端
        /// <summary>
        /// 关闭指定会话端.
        /// </summary>
        /// <param name="TSession">会话端.</param>
        public virtual void Close(T TSession) {
            lock (__TSessionCollection) {
                this.__TSessionCollection.Remove(TSession);
            }
            TSession.Dispose();
        }

        /// <summary>
        /// 关闭所有会话端.
        /// </summary>
        protected virtual void Close() {
            while(this.__TSessionCollection.Count>0) {
                Close(this.__TSessionCollection[0]);
            }
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing) {
            Stop();
            base.Dispose(disposing);
        }
        #endregion

        #region 打包协议数据
        /// <summary>
        /// 生成内部协议数据.
        /// </summary>
        /// <param name="Pakcet">数据原始包.</param>
        /// <returns></returns>
        public virtual byte[] Pakcet(P Pakcet) {
            return Packet(Pakcet.OriginalBytes);
        }

        /// <summary>
        /// 生成内部协议数据.
        /// </summary>
        /// <param name="Pakcet">数据原始包.</param>
        /// <returns></returns>
        public virtual byte[] Packet(byte[] Pakcet) {
            using (MemoryStream ms = new MemoryStream()) {
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(0xFF);
                bw.Write(0x20);
                bw.Write(sizeof(byte) * 2 + sizeof(ushort) + Pakcet.Length);
                bw.Write(Pakcet);
                bw.Flush();
                bw.Close();
                return ms.ToArray();
            }
        }
        #endregion

    }
}

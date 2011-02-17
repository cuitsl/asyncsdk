using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
namespace eTerm.ASynClientSDK.Base
{
    /// <summary>
    /// 连接基类
    /// </summary>
    public abstract class ASyncBase : IDisposable
    {
        #region 变量定义
        private static object lockObj = new object();
        private bool __disposed = false;
        private DateTime __lastActive = DateTime.Now;
        private bool m_Connected = false;
        private string m_ID = "";
        private DateTime m_ConnectTime;
        private IPEndPoint m_pLocalEP = null;
        private IPEndPoint m_pRemoteEP = null;
        private bool m_IsSecure = false;
        private Stream m_pTcpStream = null;
        private RemoteCertificateValidationCallback m_pCertificateCallback = null;
        private double m_receiveCount = 0;
        private double m_receiveBytes = 0;
        private string m_sessionId = Guid.NewGuid().ToString();
        private bool m_StreamReceived = true;
        private long __IncrementCode = 0;
        private Socket socket = null;
        #endregion

        #region Connect
        /// <summary>
        /// Connects the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="ssl">if set to <c>true</c> [SSL].</param>
        public virtual void Connect(string host, int port, bool ssl)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentException("Argument 'host' value may not be null or empty.");
            }
            if (port < 1)
            {
                throw new ArgumentException("Argument 'port' value must be >= 1.");
            }

            IPAddress[] ips = System.Net.Dns.GetHostAddresses(host);
            for (int i = 0; i < ips.Length; i++)
            {
                try
                {
                    Connect(null, new IPEndPoint(ips[i], port), ssl);
                    break;
                }
                catch (System.Exception x)
                {
                    throw x;
                }
            }
        }


        /// <summary>
        /// Connects the specified local EP.
        /// </summary>
        /// <param name="localEP">The local EP.</param>
        /// <param name="remoteEP">The remote EP.</param>
        /// <param name="ssl">if set to <c>true</c> [SSL].</param>
        protected virtual void Connect(IPEndPoint localEP, IPEndPoint remoteEP, bool ssl)
        {
            if (remoteEP == null)
            {
                throw new ArgumentNullException("remoteEP");
            }

            
            if (remoteEP.AddressFamily == AddressFamily.InterNetwork)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            else if (remoteEP.AddressFamily == AddressFamily.InterNetworkV6)
            {
                socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                throw new ArgumentException("Remote end point has invalid AddressFamily.");
            }

            try
            {
                socket.SendTimeout = SendTimeout;
                socket.ReceiveTimeout = ReceiveTimeout;
                //socket.Blocking = false;
                //socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.SendTimeout, 15000/*in   milli   second*/);
                //socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.ReceiveTimeout, 15000/*in   milli   second*/);
                if (localEP != null)
                {
                    socket.Bind(localEP);
                }


                socket.Connect(remoteEP);

                m_ID = Guid.NewGuid().ToString();
                m_ConnectTime = DateTime.Now;
                m_pLocalEP = (IPEndPoint)socket.LocalEndPoint;
                m_pRemoteEP = (IPEndPoint)socket.RemoteEndPoint;
                m_pTcpStream = new NetworkStream(socket, true);
                m_pTcpStream.ReadTimeout = ReceiveTimeout;
                m_pTcpStream.WriteTimeout = SendTimeout;
                m_Connected = true;
                if (ssl)
                {
                    SwitchToSecure();
                }
            }
            catch (System.Exception x)
            {
                throw x;
            }
        }
        #endregion

        #region 重写
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            return (obj as ASyncBase).SessionId == this.m_sessionId;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region 属性定义
        /// <summary>
        /// 会话编号（唯一编号）.
        /// </summary>
        /// <value>The session id.</value>
        public string SessionId { get { return this.m_sessionId; } set { this.m_sessionId = value; } }

        /// <summary>
        /// 上次发送后是否取回数据包.
        /// </summary>
        /// <value><c>true</c> if [stream received]; otherwise, <c>false</c>.</value>
        protected bool StreamReceived { get { return this.m_StreamReceived; } }

        /// <summary>
        /// Gets the increment.
        /// </summary>
        /// <value>The increment.</value>
        public long Increment { get { return this.__IncrementCode; } }

        /// <summary>
        /// 连接数据网络流.
        /// </summary>
        /// <value>The TCP stream.</value>
        protected Stream TcpStream { get { return this.m_pTcpStream; } }


        /// <summary>
        /// SSL证书认证代理..
        /// Value null means not sepcified.
        /// </summary>
        protected RemoteCertificateValidationCallback ValidateCertificateCallback
        {
            get { return m_pCertificateCallback; }

            set { m_pCertificateCallback = value; }
        }
        #endregion

        #region method SwitchToSecure

        /// <summary>
        /// 切换至安全SSL网络SOCKET.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected or is already secure.</exception>
        protected void SwitchToSecure()
        {

            // FIX ME: if ssl switching fails, it closes source stream or otherwise if ssl successful, source stream leaks.

            SslStream sslStream = new SslStream(m_pTcpStream, true, this.RemoteCertificateValidationCallback);
            sslStream.AuthenticateAsClient("dummy");

            // Close old stream, but leave source stream open.
            //m_pTcpStream.Dispose();

            m_IsSecure = true;
            m_pTcpStream = sslStream;
        }

        #region method RemoteCertificateValidationCallback

        private bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // User will handle it.
            if (m_pCertificateCallback != null)
            {
                return m_pCertificateCallback(sender, certificate, chain, sslPolicyErrors);
            }
            else
            {
                if (sslPolicyErrors == SslPolicyErrors.None || ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) > 0))
                {
                    return true;
                }

                // Do not allow this client to communicate with unauthenticated servers.
                return false;
            }
        }

        #endregion

        #endregion

        #region SendStream
        /// <summary>
        /// 发送数据包(上次发送后未取回数据包时不可再发送数据包).
        /// </summary>
        /// <param name="buffer">待发送的数据包.</param>
        protected virtual void SendStream(byte[] buffer) {
            this.m_pTcpStream.Write(buffer, 0, buffer.Length);
            this.m_pTcpStream.Flush();
        }
        #endregion

        #region 接收
        /// <summary>
        /// 接收数据包.
        /// </summary>
        /// <returns>接收到的数据包</returns>
        public virtual byte[] GetStream()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ReadAll(ms);
                ms.Seek(0, SeekOrigin.Begin);
                BinaryReader reader = new BinaryReader(ms);
                byte[] buffer = reader.ReadBytes(Convert.ToInt32(ms.Length));
                this.__lastActive = DateTime.Now;
                this.m_receiveBytes+= buffer.Length;
                Interlocked.Increment(ref __IncrementCode);
                this.m_StreamReceived = true;
                ms.Close();
                return buffer;
            }
        }



        /// <summary>
        /// 从网络流读取所有数据.
        /// </summary>
        /// <param name="stream">The stream.</param>
        protected virtual void ReadAll(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            byte[] buffer = new byte[2048];
            while (true)
            {
                int recv = 0;// GetCmdResultLength(out headLength);
                try
                {
                    recv = this.m_pTcpStream.Read(buffer, 0, buffer.Length);
                }
                //catch (SocketException ex) { throw ex; }
                //catch (IOException ex) { throw ex; }
                catch (System.Exception ex) { throw ex; }
                stream.Write(buffer, 0, recv);
                if (recv < buffer.Length) break;
            }
        }


        private int __SendTimeout = 15*1000;
        /// <summary>
        /// Gets or sets the send timeout.
        /// </summary>
        /// <value>The send timeout.</value>
        public virtual int SendTimeout { set { this.__SendTimeout = value; } protected get { return this.__SendTimeout; } }

        private int __ReceiveTimeout = 15 * 1000;
        /// <summary>
        /// Gets or sets the receive timeout.
        /// </summary>
        /// <value>The receive timeout.</value>
        public virtual int ReceiveTimeout { set { this.__ReceiveTimeout = value; } protected get { return this.__ReceiveTimeout; } }


        /// <summary>
        /// Gets the length of the CMD result.
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetCmdResultLength(out int CmdHeadLength) {
            byte[] buffer = new byte[2];
            this.m_pTcpStream.Read(buffer, 0, buffer.Length);
            Array.Reverse( buffer);
            CmdHeadLength = 2;
            return BitConverter.ToUInt32(buffer, 0);
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (lockObj) {
                if (!this.__disposed) {
                    try {
                        this.socket.Shutdown(SocketShutdown.Both);
                        this.socket.Close();
                        m_pTcpStream.Dispose();
                        this.DisposeResource();
                        m_Connected = false;
                    }
                    catch { }
                }
                this.__disposed = true;
            }
        }

        /// <summary>
        /// 释放附属资源.
        /// </summary>
        protected virtual void DisposeResource() { 
            
        }
        #endregion
    }
}

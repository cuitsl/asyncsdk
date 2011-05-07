using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace eTerm.ASynClientSDK.Base
{
    /// <summary>
    /// 客户端会话基类
    /// </summary>
    public abstract class _Session:IDisposable {

        #region 构造函数
        private int __SessionId = 0;
        private Socket __AsyncSocket = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="_Session"/> class.
        /// </summary>
        public _Session() { }
        #endregion

        #region 属性定义
        /// <summary>
        /// 会话端句柄号.
        /// </summary>
        /// <value>The session id.</value>
        public virtual int SessionId { get { return __SessionId; } }

        /// <summary>
        /// 会话端Socket有效连接.
        /// </summary>
        /// <value>The async socket.</value>
        public Socket AsyncSocket { get { return __AsyncSocket; } set { __AsyncSocket = value; __SessionId = value.Handle.ToInt32(); } }

        /// <summary>
        /// 会话是否处理连接状态.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected { get {
                return this.AsyncSocket == null ? false : this.AsyncSocket.Connected;            
        } }

        /// <summary>
        /// 会话端数据流对象.
        /// </summary>
        /// <value>The async stream.</value>
        protected Stream AsyncStream { get; set; }

        /// <summary>
        /// 最近通信时间.
        /// </summary>
        /// <value>The last active.</value>
        public DateTime LastActive { get; protected set; }

        /// <summary>
        /// 自建立链接以来总交互字节数(单位：KBytes).
        /// </summary>
        /// <value>The total bytes.</value>
        public float TotalBytes { get; protected set; }

        /// <summary>
        /// 最后接收字节数.
        /// </summary>
        /// <value>The current bytes.</value>
        public float CurrentBytes { get;protected set; }

        /// <summary>
        /// 建立连接以来总通信次数.
        /// </summary>
        /// <value>The total count.</value>
        public double TotalCount { get; protected set; }
        #endregion

        #region Override
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return this.SessionId.ToString();
        }

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
        public override bool Equals(object obj) {
            return (obj as _Session).SessionId == this.SessionId;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return this.SessionId;
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing) {
            {
                if (disposing) {
                    // Release managed resources
                    try {
                        if (AsyncSocket != null) {
                            if (AsyncSocket.Connected)
                                AsyncSocket.Shutdown(SocketShutdown.Both);
                            if (AsyncStream != null) {
                                AsyncStream.Close();
                                AsyncStream.Dispose();
                                AsyncStream = null;
                            }
                            AsyncSocket.Close();
                            AsyncSocket = null;
                        }
                    }
                    catch { }
                }
                // Release unmanaged resources
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace eTerm.AsyncSDK {
    /// <summary>
    /// 会话事件通知参数
    /// </summary>
    /// <typeparam name="I">协义进包类型</typeparam>
    /// <typeparam name="O">协义出包类型</typeparam>
    /// <typeparam name="TSession">会话类端类型.</typeparam>
    public class AsyncEventArgs<I,O,TSession>:EventArgs 
        where I:_Packet<TSession>,new ()
        where O:_Packet<TSession>,new ()
        where TSession:_Session,new ()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncEventArgs&lt;I, O&gt;"/> class.
        /// </summary>
        /// <param name="outPacket">The out packet.</param>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="Session">The session.</param>
        public AsyncEventArgs(I inPacket, O outPacket,TSession Session)
        {
            this.InPacket = inPacket;
            this.OutPacket = outPacket;
            this.Session = Session;
        }

        /// <summary>回复包
        /// 	<remark></remark>
        /// </summary>
        /// <value></value>
        public I InPacket { get; private set; }
        /// <summary>对应的发送包
        /// 	<remark></remark>
        /// </summary>
        /// <value></value>
        public O OutPacket { get; private set; }

        /// <summary>
        /// 会话端.
        /// </summary>
        /// <value>The session.</value>
        public TSession Session { get; private set; }
    }

    /// <summary>
    /// 会话事件通知参数
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    public class AsyncEventArgs<TSession> : EventArgs
        where TSession : _Session, new() {

        /// <summary>
        /// 会话端.
        /// </summary>
        /// <value>The session.</value>
        public TSession Session { private set; get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncEventArgs&lt;TSession&gt;"/> class.
        /// </summary>
        /// <param name="Session">The session.</param>
        public AsyncEventArgs(TSession Session) { this.Session = Session; }
    }

    /// <summary>
    /// 会话事件通知参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    public class AsyncEventArgs<T, TSession> : EventArgs
    where T : _Packet<TSession>,new ()
    where TSession:_Session,new ()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="Session">The session.</param>
        public AsyncEventArgs(T packet,TSession Session) {
            this.Packet = packet;
            this.Session = Session;
        }

        /// <summary>
        /// 数据包.
        /// </summary>
        /// <value>The packet.</value>
        public T Packet { get; set; }

        /// <summary>
        /// 会话端.
        /// </summary>
        /// <value>The session.</value>
        public TSession Session { get; private set; }
    }
}

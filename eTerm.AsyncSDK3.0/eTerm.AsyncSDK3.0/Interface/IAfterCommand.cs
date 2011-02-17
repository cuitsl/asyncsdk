using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace eTerm.AsyncSDK {


    /// <summary>
    /// 接收后续动作
    /// </summary>
    public interface IAfterCommand<TSession, P>
        where TSession : AsyncBase<TSession, P>, new()
        where P : _Packet<TSession>, new() {

        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="callBack">The call back.</param>
        /// <param name="Session">The session.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        IAsyncResult BeginExecute(AsyncCallback callBack,TSession Session,P InPacket,P OutPacket,AsyncLicenceKey Key);

        /// <summary>
        /// 结束线程.
        /// </summary>
        /// <param name="iar">The iar.</param>
        void EndExecute(IAsyncResult iar);

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }

        /// <summary>
        /// 插件开发者信息.
        /// </summary>
        /// <value>The copy right.</value>
        string CopyRight { get; }

    }

}

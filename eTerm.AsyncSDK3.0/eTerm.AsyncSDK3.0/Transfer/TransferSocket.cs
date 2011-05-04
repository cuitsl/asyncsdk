using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace eTerm.AsyncSDK.Transfer
{
    /// <summary>
    /// 文件传输通信类
    /// </summary>
    public sealed class TransferSocket : AsyncBase<TransferSocket, TransferPacket>
    {
        /// <summary>
        /// 发送字符串流.
        /// <remarks>
        /// 当需要时重写
        /// </remarks>
        /// </summary>
        /// <param name="Packet">The packet.</param>
        public override void SendPacket(string Packet)
        {
            throw new NotImplementedException();
        }
    }
}

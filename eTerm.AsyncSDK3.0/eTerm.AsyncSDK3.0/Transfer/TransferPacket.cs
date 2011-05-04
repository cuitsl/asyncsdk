using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace eTerm.AsyncSDK.Transfer
{
    /// <summary>
    /// 文件传办输包协议
    /// </summary>
    public sealed class TransferPacket : _Packet<TransferSocket>
    {
        /// <summary>
        /// 数据包协议版本号(仅供查看).
        /// <remarks>
        /// 基类必须重写
        /// </remarks>
        /// </summary>
        /// <value>The T session version.</value>
        protected override byte TSessionVersion
        {
            get { return 0x00; }
        }

        /// <summary>
        /// 解包分包.
        /// <remarks>
        /// 可在此处分数据包需要的内容，如：指令类型、指令长度等
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public override byte[] GetPacketBodyBytes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 数据包结尾标志字节.
        /// </summary>
        /// <value>The after body.</value>
        public override byte[] AfterBody
        {
            get { return new byte[]{}; }
        }

        /// <summary>
        /// 分析数据有效长度.
        /// </summary>
        /// <returns></returns>
        public override int GetPakcetLength()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 分析指令类型.
        /// </summary>
        public override void GetPacketCommand()
        {

        }

        /// <summary>
        /// 验证数据有效性.
        /// </summary>
        /// <returns></returns>
        public override bool ValidatePacket()
        {
            throw new NotImplementedException();
        }
    }
}

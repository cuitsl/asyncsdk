using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.IO;

namespace eTerm.AsyncSDK.Core {
    /// <summary>
    /// eTerm3.63版本客户通信数据协议包类型
    /// </summary>
    public sealed class eTerm363Packet : _Packet<eTerm363Session> {

        /// <summary>
        /// 数据包协议版本号(仅供查看).
        /// <remarks>
        /// 基类必须重写
        /// </remarks>
        /// </summary>
        /// <value>The T session version.</value>
        protected override byte TSessionVersion {
            get { return 0x00; }
        }

        /// <summary>
        /// 解包分包.
        /// <remarks>
        /// 可在此处分数据包需要的内容，如：指令类型、指令长度等
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public override byte[] GetPacketBodyBytes() {
            return base.PacketStream.ToArray();
        }

        /// <summary>
        /// 数据包结尾标志字节.
        /// </summary>
        /// <value>The after body.</value>
        public override byte[] AfterBody {
            get { return new byte[] { 0x03 }; ; }
        }

        /// <summary>
        /// 分析数据有效长度.
        /// </summary>
        /// <returns></returns>
        public override int GetPakcetLength() {
            return this.GetPacketBodyBytes().Length;
        }

        /// <summary>
        /// 分析指令类型.
        /// </summary>
        public override void GetPacketCommand() {
            base.PacketCommand = 0x00;
        }

        /// <summary>
        /// 验证数据有效性.
        /// </summary>
        /// <returns></returns>
        public override bool ValidatePacket() {
            return true;
        }
    }
}

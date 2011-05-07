using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.ASynClientSDK.Base;
using System.IO;
namespace eTerm.ASynClientSDK
{
    #region 通讯处理器
    public class WinSocket:EventAsyncBase<WinSocket, eTermApiPacket>
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
    #endregion

    #region 处理器对应的数据包
    /// <summary>
    /// eTerm SSL安全连接数据协议包
    /// </summary>
    public class eTermApiPacket : _Packet<WinSocket>
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

        #region 解码逻辑
        /// <summary>
        /// 数据解码(适用不同类型客户端).
        /// </summary>
        /// <returns></returns>
        public override byte[] UnpackPakcet()
        {
            return UnpacketStream(base.OriginalBytes);
        }


        /// <summary>
        /// Unpackets the stream.
        /// </summary>
        /// <param name="lpsBuf">The LPS buf.</param>
        /// <returns></returns>
        private byte[] UnpacketStream(byte[] lpsBuf)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = Unpacket(lpsBuf);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(0xFF);
                bw.Write(0x20);
                bw.Write(sizeof(byte) * 2 + sizeof(ushort) + buffer.Length);
                bw.Write(buffer);
                bw.Write(AfterBody);
                bw.Flush();
                bw.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Unpackets the specified LPS buf.
        /// </summary>
        /// <param name="lpsBuf">The LPS buf.</param>
        /// <returns></returns>
        private byte[] Unpacket(byte[] lpsBuf)
        {
            List<byte> UnPacketResult = new List<byte>();
            ushort nIndex = 18;
            ushort maxLength = BitConverter.ToUInt16(new byte[] { lpsBuf[3], lpsBuf[2] }, 0);
            while (nIndex++ < maxLength)
            {
                if (nIndex >= lpsBuf.Length) break;
                switch (lpsBuf[nIndex])
                {
                    case 0x1C:                          //红色标记
                    case 0x1D:
                        UnPacketResult.Add(0x20);
                        break;
                    case 0x62:
                    case 0x03:
                    case 0x1E:
                    case 0x1B:
                    case 0x00:
                        break;
                    case 0x0E:
                        while (true)
                        {
                            byte[] ch = new byte[] { lpsBuf[++nIndex], lpsBuf[++nIndex] };
                            if ((ch[0] == 0x1B) && (ch[1] == 0x0F))
                            {
                                break;
                            }
                            UsasToGb(ref ch[0], ref ch[1]);
                            UnPacketResult.AddRange(new byte[] { ch[0], ch[1] });
                        }
                        break;
                    default:
                        UnPacketResult.Add(lpsBuf[nIndex]);
                        break;
                }
            }
            return UnPacketResult.ToArray();
        }

        /// <summary>
        /// Usases to gb.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        private void UsasToGb(ref byte c1, ref byte c2)
        {
            if ((c1 > 0x24) && (c1 < 0x29))
            {
                byte tmp = c1;
                c1 = c2;
                c2 = (byte)(tmp + 10);
            }
            if (c1 > 0x24)
            {
                c1 = (byte)(c1 + 0x80);
            }
            else
            {
                c1 = (byte)(c1 + 0x8e);
            }
            c2 = (byte)(c2 + 0x80);
        }
        #endregion


        /// <summary>
        /// 解包分包.
        /// <remarks>
        /// 可在此处分数据包需要的内容，如：指令类型、指令长度等
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public override byte[] GetPacketBodyBytes()
        {
            return base.PacketStream.ToArray();
        }

        /// <summary>
        /// 数据包结尾标志字节.
        /// </summary>
        /// <value>The after body.</value>
        public override byte[] AfterBody
        {
            get { return new byte[] { 0x03 }; }
        }

        /// <summary>
        /// 分析数据有效长度.
        /// </summary>
        /// <returns></returns>
        public override int GetPakcetLength()
        {
            return this.GetPacketBodyBytes().Length;
        }

        /// <summary>
        /// 验证数据有效性.
        /// </summary>
        /// <returns></returns>
        public override bool ValidatePacket()
        {
            byte[] buffer = base.PacketStream.ToArray();

            if (buffer[0] == 0x00)
            {
#if DEBUG
                Console.WriteLine("握手包");
#endif
            }
            else
            {
                ushort PakcetSize = BitConverter.ToUInt16(new byte[] { buffer[3], buffer[2] }, 0);
#if DEBUG
                Console.WriteLine("交互包：包长{0}",PakcetSize);
#endif
                return PakcetSize == buffer.Length;
            }
            return true;
        }

        /// <summary>
        /// 分析指令类型.
        /// </summary>
        public override void GetPacketCommand()
        {
            base.PacketCommand = 0x00;
        }
    }
    #endregion
}

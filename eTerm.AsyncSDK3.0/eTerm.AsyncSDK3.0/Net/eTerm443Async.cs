using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using eTerm.AsyncSDK.Core;
using System.IO;

namespace eTerm.AsyncSDK.Net {

    /// <summary>
    /// SSL安全连接通信
    /// </summary>
    public class eTerm443Async : AsyncBase<eTerm443Async, eTerm443Packet> {

        #region 变量定义
        private Timer __IgAsync;
        private long __IgInterval = 1000 * 60 * 3;
        private string __DefendStatement = string.Empty;
        //private int __IgCount = 1;
        #endregion

        #region 构造函数
        /// <summary>
        /// Initializes a new instance of the <see cref="eTerm443Async"/> class.
        /// </summary>
        public eTerm443Async():base() {
            ValidateTSession = new AsyncBase<eTerm443Async, eTerm443Packet>.ValidateTSessionCallback(
                delegate(eTerm443Packet Packet, eTerm443Async Async) { 
                    byte[] OriginalBytes=Packet.OriginalBytes;
                    return !(OriginalBytes[OriginalBytes.Length - 1] == 0x00);
                });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="eTerm443Async"/> class.
        /// </summary>
        /// <param name="Ip">The ip.</param>
        /// <param name="Port">The port.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userPass">The user pass.</param>
        /// <param name="Sid">The sid.</param>
        /// <param name="Rid">The rid.</param>
        public eTerm443Async(string Ip, int Port,string userName,string userPass,byte Sid,byte Rid):this() {
            base.RemoteEP = new System.Net.IPEndPoint(IPAddress.Parse(Ip), Port);
            base.IsSsl = true;
            this.userName = userName;
            this.userPass = userPass;
            this.SID = Sid;
            this.RID = Rid;
        }
        #endregion

        #region 事件
        /// <summary>
        /// 验证代理.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Async>> OnValidated;
        #endregion

        #region 属性定义
        /// <summary>
        /// 配置连接维护频率（分钟）.
        /// </summary>
        /// <value>The ig interval.</value>
        public long IgInterval { set { __IgInterval = value * 1000 * 60; } }

        /// <summary>
        /// 配置连接维护指令.
        /// </summary>
        /// <value>The instruction.</value>
        public string Instruction { set { __DefendStatement = value; } }

        /// <summary>
        /// 用户名.
        /// </summary>
        /// <value>The name of the user.</value>
        public string userName { get; set; }

        /// <summary>
        /// 密码.
        /// </summary>
        /// <value>The user pass.</value>
        public string userPass {get; set; }

        /// <summary>
        /// SI工作号.
        /// </summary>
        /// <value>The si text.</value>
        public string SiText {get; set; }

        /// <summary>
        /// Office号.
        /// </summary>
        /// <value>The office code.</value>
        public string OfficeCode { set; get; }

        /// <summary>
        /// 认证代理.
        /// </summary>
        /// <value>The validate T session.</value>
        public ValidateTSessionCallback ValidateTSession { get; set; }

        /// <summary>
        /// 配置RID.
        /// </summary>
        /// <value>The RID.</value>
        public byte RID { get; set; }

        /// <summary>
        /// 分组号.
        /// </summary>
        /// <value>The group code.</value>
        public string GroupCode { get; set; }

        /// <summary>
        /// 配置SID.
        /// </summary>
        /// <value>The SID.</value>
        public byte SID { get; set; }

        /// <summary>
        /// 配置绑定客户端.
        /// </summary>
        /// <value>The T session.</value>
        public eTerm363Session TSession { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 发送字符串流.
        /// </summary>
        /// <param name="Cmd">The CMD.</param>
        public override void SendPacket(string Cmd) {
            SendPacket(EnCodeBuffer(Encoding.Default.GetBytes(Cmd)));
        }
        #endregion

        #region 重写

        /// <summary>
        /// 激活连接事件.
        /// </summary>
        protected override void FireOnAsynConnect() {
            base.FireOnAsynConnect();
            base.BuildStream();
            LogIn(this.userName,this.userPass);
        }

        /// <summary>
        /// Logs the in.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userPass">The user pass.</param>
        private void LogIn(string userName, string userPass) {
            SendPacket(
                SendIniData(this.GetAuthorCode(userName, userPass), 160, 0x01)
            );
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing && this.__IgAsync != null) {
                this.__IgAsync.Dispose();
                this.__IgAsync = null;
            }
        }

        /// <summary>
        /// 激活数据收取事件通知.
        /// </summary>
        protected override void FireOnPacketReceive() {
            if (base.InPacket.Sequence == 1) {
                if (ValidateTSession != null && ValidateTSession(base.InPacket,this)) {
                }
                else {
                    FireOnDisconnect();
                    return;
                }
                //base.InPacket = new eTerm443Packet();
                SendPacket(new byte[] { 
                                                               0x01,0xFE,0x00,0x11,0x14,0x10,0x00,0x02,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
                                                             });
                if (this.OnValidated != null)
                    this.OnValidated(this, new AsyncEventArgs<eTerm443Packet, eTerm443Async>(base.InPacket as eTerm443Packet, this));
            }
            else if (base.InPacket.Sequence == 2) {
                //协议改进版本（自动读取SID、RID）
                if(this.SID==0)
                    this.SID = base.InPacket.OriginalBytes[8];
                if(this.RID==0)
                    this.RID = base.InPacket.OriginalBytes[9];
                base.InPacket = new eTerm443Packet();
                if(!string.IsNullOrEmpty(this.SiText))
                    this.SendPacket(this.SiText);

                                    __IgAsync = new Timer(
                                    new TimerCallback(
                                            delegate(object sender)
                                            {
                                                ///TODO:2011-03-22 改用维持包
                                               if(!string.IsNullOrEmpty(__DefendStatement))
                                                    this.SendPacket(__DefendStatement);
                                                else
                                                    this.SendPacket(new byte[]{0x01,0xFB,0x00,0x05,0x00 });
                                                //this.SendPacket(__DefendStatement);
                                            }),
                                        null, 500, __IgInterval);
               
            }
            else {
                //__DefendStatement = __DefendStatement;
                __IgAsync.Change(__IgInterval, 0);
                base.FireOnPacketReceive();
            }
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// Gets the author code.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userPass">The user pass.</param>
        /// <returns></returns>
        private byte[] GetAuthorCode(string userName, string userPass) {
            //string localIp = GetIP();
            byte[] buffer = new byte[] { 
                0x4f, 0x31, 0x35, 0x37, 70, 0x34, 0x41, 0x31, 0, 0x54, 0x72, 0x61, 0x76, 0x65, 0x6c, 0x53, 
                0x41, 0x41, 50, 50, 0, 0x3a, 0x30, 50, 0x20, 0x20, 0x20, 0x20, 0x20, 0, 0x6d, 0x61, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0x20, 0x20, 0x20, 0x20, 0x33, 0x36, 0x33,
                0x31 , 0x33 , 0x31 , 0x30 , 0x00 , 0x30 , 0x30 , 0x30 , 0x30 , 0x30 , 0x30 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
             };
            Buffer.BlockCopy(Encoding.Default.GetBytes(userName.PadRight(16, '\0').ToCharArray()), 0, buffer, 0, 16);
            Buffer.BlockCopy(Encoding.Default.GetBytes(userPass.PadRight(16, '\0').ToCharArray()), 0, buffer, 16, 16);
            return buffer;
        }

        /// <summary>
        /// Sends the ini data.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="byLength">Length of the by.</param>
        /// <param name="nType">Type of the n.</param>
        private byte[] SendIniData(byte[] buffer, int byLength, byte nType) {
            byte[] ehit = new byte[2];
            int ehitLength = 2;
            ehit[0] = nType;
            ehit[1] = (byte)(byLength + ehitLength);
            byte[] buf = new byte[ehitLength + byLength];
            Buffer.BlockCopy(ehit, 0, buf, 0, ehitLength);
            Buffer.BlockCopy(buffer, 0, buf, ehitLength, buffer.Length);
            return buf;
        }
        #endregion

        #region 编码方法
        /// <summary>
        /// 编码需发送的数据包.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        private byte[] EnCodeBuffer(byte[] buffer) {
            //byte len;
            byte[] bytes1;
            List<byte> Command = new List<byte>();
            bytes1 = new byte[2];
            bytes1[0] = 1;
            Command.AddRange(bytes1);
            bytes1 = BitConverter.GetBytes((ushort)((0x13 + buffer.Length) + 2));
            Array.Reverse(bytes1);
            Command.AddRange(bytes1);
            //协议兼容
            //O157F4A1  O74523A1    O7452281    O7452291
            //Int32 tmp = Int32.Parse(this.RID, System.Globalization.NumberStyles.HexNumber);
            //Int32 tmp1 = 1; //Int32.Parse(base.userName.Substring(base.userName.Length - 1, 1), System.Globalization.NumberStyles.HexNumber);
            Command.AddRange(new byte[] { 0, 0, 0,0x01, SID, RID,0x70, 0x02, 0x1b, 0x0B, 0x2C, 0x20, 0, 0x0f, 0x1e });
            //len = Convert.ToByte((int)((0x13 + buffer.Length) + 2));
            Command.AddRange(buffer);
            Command.AddRange(new byte[] { 0x20, 3 });


            return Command.ToArray();
        }
        #endregion

        #region 解码逻辑
        /// <summary>
        /// 数据解码(适用不同类型客户端).
        /// </summary>
        /// <returns></returns>
        public byte[] UnpackPakcet(byte[] OriginalBytes) {
            return Unpacket(OriginalBytes);
        }


        /// <summary>
        /// Unpackets the specified LPS buf.
        /// </summary>
        /// <param name="lpsBuf">The LPS buf.</param>
        /// <returns></returns>
        private byte[] Unpacket(byte[] lpsBuf) {
            List<byte> UnPacketResult = new List<byte>();
            ushort nIndex = 18;
            ushort maxLength = BitConverter.ToUInt16(new byte[] { lpsBuf[3], lpsBuf[2] }, 0);
            while (nIndex++ < maxLength) {
                if (nIndex >= lpsBuf.Length) break;
                switch (lpsBuf[nIndex]) {
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
                        while (true) {
                            byte[] ch = new byte[] { lpsBuf[++nIndex], lpsBuf[++nIndex] };
                            if ((ch[0] == 0x1B) && (ch[1] == 0x0F)) {
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
        private void UsasToGb(ref byte c1, ref byte c2) {
            if ((c1 > 0x24) && (c1 < 0x29)) {
                byte tmp = c1;
                c1 = c2;
                c2 = (byte)(tmp + 10);
            }
            if (c1 > 0x24) {
                c1 = (byte)(c1 + 0x80);
            }
            else {
                c1 = (byte)(c1 + 0x8e);
            }
            c2 = (byte)(c2 + 0x80);
        }
        #endregion


        /// <summary>
        /// 解包入口数据包.
        /// </summary>
        /// <param name="Pakcet">The pakcet.</param>
        /// <returns></returns>
        public override byte[] UnInPakcet(eTerm443Packet Pakcet) {
            //byte[] VPakcet = new byte[Pakcet.OriginalBytes.Length - 2 - 19];
            //Buffer.BlockCopy(Pakcet.OriginalBytes, 19, VPakcet, 0, VPakcet.Length);
            //return VPakcet;
            return UnpackPakcet(Pakcet.OriginalBytes);
        }


        /// <summary>
        /// 解包出口数据包.
        /// </summary>
        /// <param name="Pakcet">The pakcet.</param>
        /// <returns></returns>
        public override byte[] UnOutPakcet(eTerm443Packet Pakcet) {
            return UnpackPakcet(Pakcet.OriginalBytes);
        }

    }
}

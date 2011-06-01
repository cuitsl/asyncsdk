using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.ASynClientSDK.Base;
using System.IO;
using System.Net;
using System.Threading;
using eTerm.ASynClientSDK.Utils;
using System.Text.RegularExpressions;
namespace eTerm.ASynClientSDK
{
    #region 通讯处理器
    /// <summary>
    /// 异步通信基类(适用于多线程、Windows App应用)
    /// </summary>
    public sealed class WinSocket:EventAsyncBase<WinSocket, eTermApiPacket>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="WinSocket"/> class.
        /// </summary>
        /// <param name="userName">用户名.</param>
        /// <param name="userPass">密码.</param>
        /// <param name="apiKey">Api授权码.</param>
        public WinSocket(string userName,string userPass,string apiKey)
            : base()
        {
            this.userName = userName;
            this.userPass = userPass;
            this.apiKey = apiKey;
            ValidateTSession = new EventAsyncBase<WinSocket, eTermApiPacket>.ValidateTSessionCallback(
                delegate(eTermApiPacket Packet, WinSocket Async)
                { 
                    byte[] OriginalBytes=Packet.OriginalBytes;
                    return !(OriginalBytes[OriginalBytes.Length - 1] == 0x00);
                });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinSocket"/> class.
        /// </summary>
        public WinSocket() : this(string.Empty, string.Empty,@"无授权码") {
            throw new NullReferenceException(@"无法使用该构造函数");
        }


        /// <summary>
        /// 认证代理.
        /// </summary>
        /// <value>The validate T session.</value>
        public ValidateTSessionCallback ValidateTSession { get; set; }


        /// <summary>
        /// 登录成功事件通知.
        /// </summary>
        public event EventHandler OnValidated;

        /// <summary>
        /// 配置RID.
        /// </summary>
        /// <value>The RID.</value>
        protected byte RID { get; set; }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>The API key.</value>
        private string apiKey { get; set; }

        /// <summary>
        /// 分组号.
        /// </summary>
        /// <value>The group code.</value>
        protected string GroupCode { get; set; }

        /// <summary>
        /// 配置SID.
        /// </summary>
        /// <value>The SID.</value>
        protected byte SID { get; set; }

        /// <summary>
        /// 服务器认证帐号.
        /// </summary>
        /// <value>The name of the user.</value>
        protected string userName { get;private set; }

        /// <summary>
        /// 服务器认证密码.
        /// </summary>
        /// <value>The user pass.</value>
        protected string userPass { get; private set; }



        /// <summary>
        /// 发送字符串流.
        /// <remarks>
        /// 当需要时重写
        /// </remarks>
        /// </summary>
        /// <param name="Packet">The packet.</param>
        public override void SendPacket(string Packet)
        {
            if (!CanSendPacket) return;
            SendPacket(EnCodeBuffer(Encoding.Default.GetBytes(Packet)));
            OutPakcet = new eTermApiPacket() { OriginalBytes = Encoding.Default.GetBytes(Packet) };
        }


        /// <summary>
        /// Sends the specified packet.
        /// </summary>
        /// <param name="Packet">The packet.</param>
        private void Send(string Packet) {
            SendPacket(EnCodeBuffer(Encoding.Default.GetBytes(Packet)));
            OutPakcet = new eTermApiPacket() { OriginalBytes = Encoding.Default.GetBytes(Packet) };
        }
        #region 编码方法
        /// <summary>
        /// 编码需发送的数据包.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        private byte[] EnCodeBuffer(byte[] buffer)
        {
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
            Command.AddRange(new byte[] { 0, 0, 0, 0x01, SID, RID, 0x70, 0x02, 0x1b, 0x0B, 0x2C, 0x20, 0, 0x0f, 0x1e });
            //len = Convert.ToByte((int)((0x13 + buffer.Length) + 2));
            Command.AddRange(buffer);
            Command.AddRange(new byte[] { 0x20, 3 });


            return Command.ToArray();
        }
        #endregion


        #region 重写

        /// <summary>
        /// 激活连接事件.
        /// </summary>
        protected override void FireOnAsynConnect()
        {
            base.FireOnAsynConnect();
            base.BuildStream();
            LogIn(this.userName, this.userPass);
        }

        /// <summary>
        /// Logs the in.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userPass">The user pass.</param>
        private void LogIn(string userName, string userPass)
        {                               
            if (this.LocalEP == null)
                SendPacket(
                    SendIniData(this.GetAuthorCode(userName, userPass), 160, 0x01)
                );
            else
                SendPacket(AddressPacket());
        }

        /// <summary>
        /// 激活数据收取事件通知.
        /// </summary>
        protected override void FireOnPacketReceive()
        {
            if (base.InPacket.Sequence == 1)
            {
                if (ValidateTSession != null && ValidateTSession(base.InPacket, this))
                {
                }
                else
                {
                    FireOnDisconnect();
                    return;
                }
                SendPacket(new byte[] { 
                                                               0x01,0xFE,0x00,0x11,0x14,0x10,0x00,0x02,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
                                                             });
                //if (OnValidated != null)OnValidated(this, EventArgs.Empty);
            }
            else if (base.InPacket.Sequence == 2)
            {
                //协议改进版本（自动读取SID、RID）
                if (this.SID == 0)
                    this.SID = base.InPacket.OriginalBytes[8];
                if (this.RID == 0)
                    this.RID = base.InPacket.OriginalBytes[9];
                base.InPacket = new eTermApiPacket();
                new Timer(new TimerCallback(delegate(object sender) {
                    Send(string.Format(@"!api {0}", this.apiKey));
                }), null, 500, Timeout.Infinite);
                //
            }
            else if (base.InPacket.Sequence == 3) {
                StringBuilder ApiKey = new StringBuilder();
                foreach (byte c in TEACrypter.MD5(Encoding.GetEncoding(@"gb2312").GetBytes( this.apiKey)))
                {
                    ApiKey.Append(String.Format("{0:X}", c).PadLeft(2, '0'));
                }
                if (Regex.IsMatch(Encoding.GetEncoding(@"gb2312").GetString(UnOutPakcet(InPacket)), ApiKey.ToString(), RegexOptions.IgnoreCase | RegexOptions.Multiline) && this.OnValidated != null)
                {
                    CanSendPacket = true;
                    OnValidated(this, EventArgs.Empty);
                }
                else
                    this.Close();
                base.InPacket = new eTermApiPacket();
            }
            else
            {
                //__DefendStatement = __DefendStatement;
                InPacket = new eTermApiPacket() { Sequence=InPacket.Sequence, PacketDateTime=DateTime.Now, Session=InPacket.Session, OriginalBytes=UnOutPakcet(InPacket) };
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
        private byte[] GetAuthorCode(string userName, string userPass)
        {
            //string localIp = GetIP();
            byte[] buffer = new byte[] { 
                0x4f, 0x31, 0x35, 0x37, 70, 0x34, 0x41, 0x31, 0, 0x54, 0x72, 0x61, 0x76, 0x65, 0x6c, 0x53, 
                0x41, 0x41, 50, 50, 0, 0x3a, 0x30, 50, 0x20, 0x20, 0x20, 0x20, 0x20, 0, 0x6d, 0x61, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0x20, 0x20, 0x20, 0x20, 0x33, 0x36, 0x33,
                0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x01, 0, 0
             };
            Buffer.BlockCopy(Encoding.Default.GetBytes(userName.PadRight(16, '\0').ToCharArray()), 0, buffer, 0, 16);
            Buffer.BlockCopy(Encoding.Default.GetBytes(userPass.PadRight(16, '\0').ToCharArray()), 0, buffer, 16, 16);
            return buffer;
        }

        /// <summary>
        /// 地址认证首发包.
        /// </summary>
        /// <returns></returns>
        private byte[] AddressPacket()
        {
            List<byte> items = new List<byte>(){ 
                0x01,0xA2,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x00,0x00,0x00,0x00,0x00,0x00
               ,0x00,0x00,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x00,0x00,0x00,0x00,0x00,0x00
               ,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
               ,0x00,0x00,0x30,0x30,0x30,0x33,0x66,0x66,0x64,0x62,0x65,0x62,0x65,0x62
            };
            byte[] IpAddress = Encoding.Default.GetBytes((this.LocalEP as IPEndPoint).Address.ToString());
            int ipLength = IpAddress.Length;
            items.AddRange(IpAddress);
            while (ipLength++ < 15)
                items.Add(0x20);
            items.AddRange(new byte[]{
                0x00,0x00,0x00
               ,0x00,0x33,0x31,0x30,0x00,0x30,0x30,0x30,0x30,0x30,0x30,0x00,0x00,0x00,0x00,0x00 
               ,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
               ,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
               ,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
               ,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
               ,0x00,0x00
               
            });
            return items.ToArray();
        }

        /// <summary>
        /// Sends the ini data.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="byLength">Length of the by.</param>
        /// <param name="nType">Type of the n.</param>
        private byte[] SendIniData(byte[] buffer, int byLength, byte nType)
        {
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

        #region 解码逻辑

        /// <summary>
        /// 解包入口数据包.
        /// </summary>
        /// <param name="Pakcet">The pakcet.</param>
        /// <returns></returns>
        protected override byte[] UnInPakcet(eTermApiPacket Pakcet)
        {
            return Unpacket(Pakcet.OriginalBytes, 2);
        }

        /// <summary>
        /// 解包出口数据包.
        /// </summary>
        /// <param name="Pakcet">The pakcet.</param>
        /// <returns></returns>
        protected override byte[] UnOutPakcet(eTermApiPacket Pakcet)
        {
            return Unpacket(Pakcet.OriginalBytes,4);
        }



        /// <summary>
        /// Unpackets the specified LPS buf.
        /// </summary>
        /// <param name="lpsBuf">The LPS buf.</param>
        /// <param name="unlessLength">Length of the unless.</param>
        /// <returns></returns>
        private byte[] Unpacket(byte[] lpsBuf, int unlessLength)
        {
            List<byte> UnPacketResult = new List<byte>();
            ushort nIndex = 18;
            uint ColumnNumber = 0;
            ushort maxLength = BitConverter.ToUInt16(new byte[] { lpsBuf[3], lpsBuf[2] }, 0);
            while (nIndex++ < maxLength - unlessLength)
            {
                if (nIndex >= lpsBuf.Length) break;
                switch (lpsBuf[nIndex])
                {
                    case 0x1C:                          //红色标记
                    case 0x1D:
                        UnPacketResult.Add(0x20);
                        ColumnNumber++;
                        break;
                    case 0x62:
                    case 0x03:
                    //case 0x1E:
                    case 0x1B:
                    case 0x00:
                        break;
                    case 0x1E:
                        UnPacketResult.Add(0x0E);
                        ColumnNumber++;
                        break;
                    case 0x0D:
                        while (++ColumnNumber % 80 != 0)
                        {
                            UnPacketResult.Add(0x20);
                            continue;
                        }
                        if (ColumnNumber % 80 == 0) { UnPacketResult.Add(0x0D); ColumnNumber = 0; }
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
                            ColumnNumber++;
                            UnPacketResult.AddRange(new byte[] { ch[0], ch[1] });
                            if (ColumnNumber % 80 == 0) { UnPacketResult.Add(0x0D); ColumnNumber = 0; }
                        }
                        break;
                    default:
                        ColumnNumber++;
                        UnPacketResult.Add(lpsBuf[nIndex]);
                        if (ColumnNumber % 80 == 0) { UnPacketResult.Add(0x0D); ColumnNumber = 0; }
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

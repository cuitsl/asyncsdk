using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using eTerm.ASynClientSDK.Exception;
using System.Text.RegularExpressions;
using eTerm.ASynClientSDK.Utils;
using SDKConfig = eTerm.ASynClientSDK.AsyncSDKConfig;
using System.Threading;
namespace eTerm.ASynClientSDK.Base {
    /// <summary>
    /// 连接基类
    /// </summary>
    public abstract class ASynCommand:ASyncBase {

        #region 属性定义
        /// <summary>
        /// Gets or sets a value indicating whether this instance is authorized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is authorized; otherwise, <c>false</c>.
        /// </value>
        protected bool IsAuthorized { get; private set; }

        /// <summary>
        /// Gets or sets the asn command sleep.
        /// </summary>
        /// <value>The asn command sleep.</value>
        public float? AsnCommandSleep { protected get; set; }
        #endregion

        #region 测试部份
        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Connect() {
            base.Connect(
                SDKConfig.Current.Address, 
                SDKConfig.Current.Port,
                SDKConfig.Current.IsSsl
                );
            LogIn(SDKConfig.Current.UserName, SDKConfig.Current.UserPass);
            //LogIn("o7340401", "wxcts");
            this.IsAuthorized = true;
        }

        /// <summary>
        /// 指令间隔时间.
        /// </summary>
        protected virtual void ThreadSleep() {
            Thread.Sleep(int.Parse((this.AsnCommandSleep ?? 0.5 * 1000).ToString()));
        }
        #endregion

        #region 重写
        /// <summary>
        /// Logs the in.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userPass">The user pass.</param>
        public virtual void LogIn(string userName,string userPass) {
            SendStream(
                SendIniData(GetAuthorCode(userName, userPass), 160, 0x01)
            );
            byte[] buffer=GetStream();

            SendStream(new byte[] { 
                                                               0x01,0xFE,0x00,0x11,0x14,0x10,0x00,0x02,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
                                                             });
            buffer = base.GetStream();
            if (buffer[1] != 0xFD) throw new AccessDenyException() ;
            IsAuthorized = true;
        }

        /// <summary>
        /// 接收数据包.
        /// </summary>
        /// <returns>接收到的数据包</returns>
        public override byte[] GetStream() {
            if (!IsAuthorized)
                return base.GetStream();
            else
                return Unpacket(base.GetStream());
        }

        /// <summary>
        /// Gets or sets the RID.
        /// </summary>
        /// <value>The RID.</value>
        protected virtual byte RID { get; private set; }

        /// <summary>
        /// Gets or sets the SID.
        /// </summary>
        /// <value>The SID.</value>
        protected virtual byte SID { get; private set; }

        /// <summary>
        /// 按连接发送指令.
        /// </summary>
        /// <param name="CmdStream">The CMD stream.</param>
        public virtual void SendStream(string CmdStream) {
            //TODO:Usas编码 2011-01-10
            byte[] buffer = EnCodeBuffer(GbToUsas(CmdStream));
            SendStream(buffer);
        }

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
            Command.AddRange(new byte[] { 0, 0, 0, 0x01, SID, RID, 0x70, 0x02, 0x1b, 0x0B, 0x2C, 0x20, 0, 0x0f, 0x1e });
            //len = Convert.ToByte((int)((0x13 + buffer.Length) + 2));
            Command.AddRange(buffer);
            Command.AddRange(new byte[] { 0x20, 0x03 });


            return Command.ToArray();
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

        #region 解码逻辑
        /// <summary>
        /// 数据解码(适用不同类型客户端).
        /// </summary>
        /// <param name="lpsBuf">The LPS buf.</param>
        /// <returns></returns>
        protected virtual byte[] Unpacket(byte[] lpsBuf) {
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

        #region 重写
        /// <summary>
        /// 指令结果解析适配器.
        /// </summary>
        /// <param name="Msg">指令结果集合.</param>
        /// <returns></returns>
        protected abstract ASyncResult ResultAdapter(string Msg);


        /// <summary>
        /// 结果错误检查及异常抛出.
        /// </summary>
        protected virtual void ThrowExeption(string Msg) { }


        /// <summary>
        /// 生成指令并发送分析(子类必须重写).
        /// </summary>
        /// <param name="SynCmd">eTerm实质指令.</param>
        /// <returns></returns>
        protected virtual ASyncResult GetSyncResult(string SynCmd) {
            if(!IsAuthorized)
                Connect();
            SendStream( SynCmd);
            ASyncResult Result = ResultAdapter(ConvertResult(GetStream()));
            Dispose();
            Result.ASynCmd = SynCmd;
            return Result;
        }

        /// <summary>
        /// Converts the result.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        protected virtual string ConvertResult(byte[] buffer) {
            return Encoding.GetEncoding("gb2312").GetString(buffer);
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 获取日期字符串辅助方法.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        protected string getDateString(DateTime date) {
            MatchCollection regx = new Regex(@"[A-Z]{3,3}").Matches(date.ToString("R").ToUpper());
            return regx[1].Value;
        }

        /// <summary>
        /// 中文编码到Usas转换.
        /// </summary>
        /// <param name="inputString">输入字符串.</param>
        /// <returns></returns>
        protected virtual byte[] GbToUsas(string inputString) {
            StringBuilder sb = new StringBuilder(inputString);
            foreach (Match m in Regex.Matches(inputString, @"[\u4e00-\u9fa5]+")) {
                sb.Replace(m.Value, string.Format(@"{0}{1}", Cn2PyUtil.FullConvert(m.Value), m.Value));
            }

            byte[] org = Encoding.GetEncoding("gb2312").GetBytes(sb.ToString());
            List<byte> result = new List<byte>();
            bool flag = false;
            foreach (byte b in org) {
                if (!flag && b > 128) {
                    result.AddRange(new byte[] { 0x1B, 0x0E });
                    flag = true;
                }
                else if (flag && b <= 128) {
                    result.AddRange(new byte[] { 0x1B, 0x0F });
                    flag = false;
                }
                if (flag) {
                    result.Add((byte)(b - 128));
                }
                else {
                    result.Add(b);
                }
            }
            if (flag)
                result.AddRange(new byte[] { 0x1B, 0x0F });
            return result.ToArray();
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using eTerm.SynClientSDK.Utils;

namespace eTerm.SynClientSDK.Base {
    public class MessageStream : IDisposable {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageStream"/> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <param name="streamBody">The stream body.</param>
        public MessageStream(byte responseCode, byte[] streamBody):this(0x00,0x00,streamBody,Encoding.GetEncoding("gb2312")) {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageStream"/> class.
        /// </summary>
        /// <param name="protocolVersion">The protocol version.</param>
        /// <param name="responseCode">The response code.</param>
        /// <param name="streamBody">The stream body.</param>
        public MessageStream(byte protocolVersion, byte responseCode, byte[] streamBody):this(protocolVersion,responseCode,streamBody,Encoding.GetEncoding("gb2312")) {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageStream"/> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <param name="streamBody">The stream body.</param>
        /// <param name="streamCoding">The stream coding.</param>
        public MessageStream(byte responseCode, byte[] streamBody, Encoding streamCoding):this(0x00,responseCode,streamBody,streamCoding) { 
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageStream"/> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <param name="CmdStream">The CMD stream.</param>
        /// <param name="StreamCoding">The stream coding.</param>
        public MessageStream(byte responseCode, string CmdStream, Encoding StreamCoding):this(0x00,responseCode,StreamCoding.GetBytes(CmdStream),StreamCoding) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageStream"/> class.
        /// </summary>
        /// <param name="protocolVersion">The protocol version.</param>
        /// <param name="responseCode">The response code.</param>
        /// <param name="streamBody">The stream body.</param>
        /// <param name="streamCoding">The stream coding.</param>
        public MessageStream(byte protocolVersion, byte responseCode, byte[] streamBody, Encoding streamCoding) {
            ProtocolVersion = protocolVersion;
            ResponseCode = responseCode;
            StreamBody = streamBody;
            EnCoding = streamCoding;
        }

        /// <summary>
        /// Gets or sets the en coding.
        /// </summary>
        /// <value>The en coding.</value>
        public Encoding EnCoding { set; private get; }

        /// <summary>
        /// Gets or sets the protocol version.
        /// </summary>
        /// <value>The protocol version.</value>
        public byte ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>The response code.</value>
        public byte ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the stream body.
        /// </summary>
        /// <value>The stream body.</value>
        public byte[] StreamBody { get; set; }

        /// <summary>
        /// Gets or sets the stream date.
        /// </summary>
        /// <value>The stream date.</value>
        public DateTime StreamDate { get; private set; }

        /// <summary>
        /// Gets or sets the length of the stream.
        /// </summary>
        /// <value>The length of the stream.</value>
        public int StreamLength { get; private set; }

        /// <summary>
        /// Gets or sets the stream string.
        /// </summary>
        /// <value>The stream string.</value>
        public string StreamString { get {
                return EnCoding.GetString(StreamBody);
        } }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { 
            
        }


        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public virtual void GetStream(byte[] buffer) {
            StreamBody = Unpacket(buffer);
            //StreamString = EnCoding.GetString(StreamBody);
        }



        /// <summary>
        /// Toes the stream.
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ToStream() {
            return EnCodeBuffer(GbToUsas(EnCoding.GetString(StreamBody)));
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return StreamString;
        }

        #region 编码逻辑
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
            Command.AddRange(new byte[] { 0, 0, 0, 0x01, 0x27, 0x51, 0x70, 0x02, 0x1b, 0x0B, 0x2C, 0x20, 0, 0x0f, 0x1e });
            //len = Convert.ToByte((int)((0x13 + buffer.Length) + 2));
            Command.AddRange(buffer);
            Command.AddRange(new byte[] { 0x20, 0x03 });


            return Command.ToArray();
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

    }
}

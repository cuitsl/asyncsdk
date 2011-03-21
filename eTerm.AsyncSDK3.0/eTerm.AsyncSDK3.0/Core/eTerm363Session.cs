using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Net;
using System.Threading;
using System.IO;

namespace eTerm.AsyncSDK.Core {

    /// <summary>
    /// 会话类型
    /// </summary>
    public enum TSessionType:int {
        /// <summary>
        /// eTerm客户端（返回原始数据）
        /// </summary>
        eTerm363=1,
        /// <summary>
        /// 接口客户端（数据被解码）
        /// </summary>
        Interface=2
    }

    /// <summary>
    /// 指令配置释放计时重置代理
    /// </summary>
    public delegate int ReleaseIntervalSetDelegate(string Packet,eTerm363Session TSession);


    /// <summary>
    /// eTerm3.63版本会话端实例
    /// </summary>
    public sealed class eTerm363Session : AsyncBase<eTerm363Session, eTerm363Packet> {

        private Timer __ReleaseAsync;

        /// <summary>
        /// Initializes a new instance of the <see cref="eTerm363Session"/> class.
        /// </summary>
        public eTerm363Session() {
            //SessionType = TSessionType.eTerm363;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="eTerm363Session"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userPass">The user pass.</param>
        public eTerm363Session(string userName, string userPass) {
            this.userName = userName;
            this.userPass = userPass;
        }

        /// <summary>
        /// 配置资源释放事件.
        /// </summary>
        public event EventHandler<AsyncEventArgs<eTerm363Session>> OnTSessionRelease;

        /// <summary>
        /// 计时重置代理.
        /// </summary>
        /// <value>The ig interval set.</value>
        public ReleaseIntervalSetDelegate ReleaseIntervalSet { get; set; }


        /// <summary>
        /// 开始连接远程主机.
        /// </summary>
        public override void Connect() {
            //base.Connect();
            FireOnAsynConnect();
            base.BuildStream();
        }

        /// <summary>
        /// 禁用的指令正规表达式.
        /// </summary>
        /// <value>The unallowable reg.</value>
        public string UnallowableReg { get; set; }

        /// <summary>
        /// 特殊指令时限设置，格式为:^AV|10,^SS|20.
        /// </summary>
        /// <value>The special interval list.</value>
        public string SpecialIntervalList { get; set; }

        /// <summary>
        /// 解包用户登录信息.
        /// </summary>
        /// <param name="Pakcet">The pakcet.</param>
        public void UnpakcetSession(eTerm363Packet Pakcet) {
            byte[] buffer = Pakcet.OriginalBytes;
            byte[] userBytes=new byte[16]; 
            byte[] passBytes=new byte[16];
            Buffer.BlockCopy(buffer, 2, userBytes, 0, 16);
            Buffer.BlockCopy(buffer, 18, passBytes, 0, 16);
            this.userName = Encoding.Default.GetString(userBytes).Replace("\0", "");
            this.userPass = Encoding.Default.GetString(passBytes).Replace("\0", "");
        }


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
        /// 激活断线事件通知.
        /// </summary>
        protected override void FireOnDisconnect() {
            if (__ReleaseAsync != null) {
                //FireTSessionRelease();
                __ReleaseAsync.Dispose();
                __ReleaseAsync = null;
            }
            base.FireOnDisconnect();
        }


        /// <summary>
        /// 激活数据收取事件通知.
        /// </summary>
        protected override void FireOnPacketReceive() {
            int ___TSessionInterval = __TSessionInterval;
            if (this.ReleaseIntervalSet != null)
                ___TSessionInterval = ReleaseIntervalSet(Encoding.GetEncoding("gb2312").GetString(UnInPakcet(InPacket)).Trim(), this) * 1000;
            base.FireOnPacketReceive();
            if (this.Async443 == null) {
                if (this.__ReleaseAsync != null)
                    this.__ReleaseAsync.Dispose();
                this.__ReleaseAsync = null;
                return;
            }

            if (__ReleaseAsync == null) {
                __ReleaseAsync = new Timer(
                new TimerCallback(
                        delegate(object sender)
                        {
                            FireTSessionRelease();
                            __ReleaseAsync.Dispose();
                            __ReleaseAsync = null;
                        }),
                    null, ___TSessionInterval, ___TSessionInterval);
            }
            else {
                __ReleaseAsync.Change(___TSessionInterval, 0);
            }
        }

        /// <summary>
        /// Fires the T session release.
        /// </summary>
        private void FireTSessionRelease() {
            if (this.OnTSessionRelease != null)
                this.OnTSessionRelease(this, new AsyncEventArgs<eTerm363Session>(this as eTerm363Session));
        }

        /// <summary>
        /// 配置RID.
        /// </summary>
        /// <value>The RID.</value>
        public byte RID { get; set; }

        /// <summary>
        /// 配置SID.
        /// </summary>
        /// <value>The SID.</value>
        public byte SID { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string userName { get;private set; }

        /// <summary>
        /// Gets or sets the user pass.
        /// </summary>
        /// <value>The user pass.</value>
        public string userPass { get;private set; }

        /// <summary>
        /// Gets or sets the user group.
        /// </summary>
        /// <value>The user group.</value>
        public string userGroup { get; set; }

        /// <summary>
        /// 会话类型.
        /// </summary>
        /// <value>The type of the sesstion.</value>
        //public TSessionType SessionType { get; set; }

        /// <summary>
        /// 通信是否完成.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is completed; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// 配置资源会话.
        /// </summary>
        /// <value>The async443.</value>
        public eTerm443Async Async443 { get; set; }

        private int __TSessionInterval = 1000 * 15;
        /// <summary>
        /// 会话资源使用时间（必须大于10时限）.
        /// <remarks>
        ///     因网络流读取超时时限为10秒，而避免数据丢失所以必须大于此超时时限
        /// </remarks>
        /// </summary>
        /// <value>The T session interval.</value>
        public int TSessionInterval { set { this.__TSessionInterval = value * 1000; } get { return this.__TSessionInterval / 1000; } }



        /// <summary>
        /// 发送字符串流(不可用).
        /// <remarks>
        /// 当需要时重写
        /// </remarks>
        /// </summary>
        /// <param name="Packet">The packet.</param>
        public override void SendPacket(string Packet) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 解包入口数据包.
        /// </summary>
        /// <param name="Pakcet">The pakcet.</param>
        /// <returns></returns>
        public override byte[] UnInPakcet(eTerm363Packet Pakcet) {
            if (Pakcet.OriginalBytes.Length - 2 - 19 <= 0) return new byte[] { };
            byte[] VPakcet=new byte[Pakcet.OriginalBytes.Length-2-19];
            Buffer.BlockCopy(Pakcet.OriginalBytes, 19, VPakcet, 0, VPakcet.Length);
            return VPakcet;
        }

        /// <summary>
        /// 解包出口数据包.
        /// </summary>
        /// <param name="Pakcet">The pakcet.</param>
        /// <returns></returns>
        public override byte[] UnOutPakcet(eTerm363Packet Pakcet) {
            return UnpackPakcet(Pakcet.OriginalBytes);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.Net;
using eTerm.AsyncSDK.Net;

namespace eTerm.AsyncSDK.Core {
    /// <summary>
    /// eTerm转发服务器
    /// </summary>
    public sealed class eTermAsyncServer : AsyncBaseServer<eTerm363Session, eTerm363Packet> {

        #region 基础方法
        /// <summary>
        /// Initializes a new instance of the <see cref="eTermAsyncServer"/> class.
        /// </summary>
        /// <param name="RemotePoint">The remote point.</param>
        /// <param name="Sid">The sid.</param>
        /// <param name="Rid">The rid.</param>
        public eTermAsyncServer(IPEndPoint RemotePoint, byte Sid, byte Rid):base() {
            base.AsyncEndPoint = RemotePoint;
            this.RID = Rid;
            this.SID = Sid;
            //eTerm443AsyncList = new List<eTerm443Async>();
        }

        /// <summary>
        /// 客户端默认RID.
        /// </summary>
        /// <value>The RID.</value>
        public byte RID { private set; get; }

        /// <summary>
        /// 客户端默认SID.
        /// </summary>
        /// <value>The SID.</value>
        public byte SID { private set; get; }

        /// <summary>
        /// 激活读取事件通知.
        /// </summary>
        /// <param name="e"></param>
        protected override void FireOnReadPacket(AsyncEventArgs<eTerm363Packet,eTerm363Packet, eTerm363Session> e) {
            //base.FireOnReadPacket(e);
            if (e.InPacket.Sequence == 1) {
                e.Session.RID = RID;
                e.Session.SID = SID;
                if (base.TSessionValidate != null && base.TSessionValidate(e.Session, e.InPacket)) {
                    e.Session.SendPacket(new byte[] { 
                            0x00,0x14,0x01,0x00,0x03,0x00,0x00,0x00,this.SID,this.RID,0x0C,0x00,0x00,0x8C,0x8C,0x29,
                            0x00,0x00,0xA9,0xA9 
                            });
                    FireTSessionValidated(e.Session);
                }
                else {
                    e.Session.SendPacket(new byte[] { 
                            0x00,0x37,0x00,0x31,0x30,0x30,0x30,0x31,0x3A,0x20,0xB5,0xC7,0xC2,0xBC,0xCA,0xA7
                            ,0xB0,0xDC,0xA3,0xBA,0xC7,0xEB,0xBC,0xEC,0xB2,0xE9,0xD3,0xC3,0xBB,0xA7,0xC3,0xFB 
                            ,0xBA,0xCD,0xBF,0xDA,0xC1,0xEE,0xA3,0xAC,0xBB,0xF2,0xD5,0xDF,0xC8,0xCF,0xD6,0xA4 
                            ,0xC0,0xE0,0xD0,0xCD,0xA3,0xA1,0x00 
                            });
                }
            }
            else if (e.InPacket.Sequence == 2) {
                e.Session.SendPacket(new byte[] { 
                                0x01,0xFD,0x00,0x06,0x00,0x00,0x01,0xFD,0x00,0x06,0x00,0x0C,0x01,0xFD,0x00,0x06,
                                0x00,0x29 
                            });
            }
            else if (e.InPacket.OriginalBytes[1] == 0x00) {
                base.FireOnReadPacket(e);
            }
            else {
#if DEBUG
                Console.WriteLine("No Avaliable Data Buffer,Lenght Is:{0} Packet Sequence Date:{1}", e.InPacket.OriginalBytes.Length, e.InPacket.PacketDateTime);
                foreach (byte b in e.InPacket.OriginalBytes) {
                    Console.Write(" {0} ", b);
                }
                Console.WriteLine();
#endif
            }
        }
        #endregion

        #region 重写
        /// <summary>
        /// 向指定会话面发送字符串流.
        /// </summary>
        /// <param name="TSession">指定会话端.</param>
        /// <param name="Packet">字符流.</param>
        public override void SendPacket(eTerm363Session TSession, string Packet) {
            throw new NotImplementedException();
        }
        #endregion

    }
}

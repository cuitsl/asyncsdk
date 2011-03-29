using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using eTerm.AsyncSDK.Util;

namespace eTerm.AsyncSDK.Net {
    /// <summary>
    /// 安全通信包内部结构
    /// </summary>
    public static class __eTerm443Packet {
        /// <summary>
        /// 无空闲资源提示.
        /// </summary>
        /// <param name="Sid">The sid.</param>
        /// <param name="Rid">The rid.</param>
        /// <returns></returns>
        public static byte[] NoAsyncSocketInfo(byte Sid, byte Rid) {
            return BuildSessionPacket(Sid, Rid, "无可用活动配置!");
        }

        /// <summary>
        /// 资源读取超时提示.
        /// </summary>
        /// <param name="Sid">The sid.</param>
        /// <param name="Rid">The rid.</param>
        /// <returns></returns>
        public static byte[] AsyncSocketTimeoutInfo(byte Sid, byte Rid) {
            return BuildSessionPacket(Sid, Rid, "数据读取超时!");
        }

        /// <summary>
        /// Asyncs the socket error.
        /// </summary>
        /// <param name="Sid">The sid.</param>
        /// <param name="Rid">The rid.</param>
        /// <returns></returns>
        public static byte[] AsyncSocketError(byte Sid, byte Rid) {
            return BuildSessionPacket(Sid,Rid, AUTHERROR_MES);
        }

        /// <summary>
        /// 通用提示生成.
        /// </summary>
        /// <param name="Sid">The sid.</param>
        /// <param name="Rid">The rid.</param>
        /// <param name="Message">提示内容.</param>
        /// <returns></returns>
        public static byte[] BuildSessionPacket(byte Sid, byte Rid, string Message) {
            List<byte> __b = new List<byte>();
            __b.AddRange(new byte[] { 0x01, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x01, Sid, Rid, 0x70, 0x02, 0x1B, 0x0B, 0x20, 0x20, 0x0F, 0x1B, 0x4D/*,0x1C*/ });
            foreach (byte b in GbToUsas(Message)) {
                if (b == 0x0A) continue;
                if (b == 0x0D) {
                    __b.AddRange(new byte[] { 0x1D, 0x1C,0x0D  });
                    continue;
                }
                __b.Add(b);
            }
            //__b.AddRange(GbToUsas(Message));
            __b.AddRange(new byte[] {/*0x1D,*/0x0D,0x1E,0x1B,0x62, 0x03 });
            __b[3] = (byte)__b.Count;
            return __b.ToArray();
        }

        /// <summary>
        /// 中文编码到Usas转换.
        /// </summary>
        /// <param name="inputString">输入字符串.</param>
        /// <returns></returns>
        private static byte[] GbToUsas(string inputString) {
            //StringBuilder sb = new StringBuilder(inputString);
            //foreach (Match m in Regex.Matches(inputString, @"[\u4e00-\u9fa5]+")) {
            //    sb.Replace(m.Value, string.Format(@"{0}",m.Value));
            //}

            byte[] org = Encoding.GetEncoding("gb2312").GetBytes(inputString);
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
        /// 授权错误提示
        /// </summary>
        public const string AUTHERROR_MES = @"授权服务不正确，请联系开发商获取授权KEY！！
电话　：13524008692  FORCEN
电邮　：FORCE0908@GMAIL.COM
ＱＱ　: 29742914
ＭＳＮ：VALON0908@HOTMAIL.COM";
    }
}

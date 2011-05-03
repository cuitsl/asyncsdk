using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Contexts;
using System.Transactions;
using eTerm.AsyncSDK;
using eTerm.AsyncSDK.Util;
using ASyncSDK.Office;
using System.Data;
using System.Data.Common;





namespace AsyncAPI3._0Tst {
    class Program {
        static void Main(string[] args) {
            SQLiteDatabase Db = new SQLiteDatabase(new FileInfo(@"D:\SouceCode\Personal\eTerm.AsyncSDK3.0\ASyncSDK.Office\bin\Release\SQLiteDb.s3db").FullName);
            DbCommand dbCommand = Db.GetSqlStringCommand(@"select * from SQLiteLog201105 order by TLogId DESC limit 0,5");
            IDataReader DR = dbCommand.ExecuteReader();
            while (DR.Read()) {
                Console.WriteLine(string.Format(@"InPacket:{0} Bytes OutPacket:{1} Bytes On {2}", Encoding.GetEncoding(@"gb2312").GetString(Program.UnpackPakcet(DR[4] as byte[])), Encoding.GetEncoding(@"gb2312").GetString(Program.UnpackPakcet(DR[5] as byte[])),DR[6] as DateTime?));
            }
            Console.ReadLine();

        }


        #region 解码逻辑
        /// <summary>
        /// 数据解码(适用不同类型客户端).
        /// </summary>
        /// <returns></returns>
        public static byte[] UnpackPakcet(byte[] OriginalBytes)
        {
            return Unpacket(OriginalBytes);
        }


        /// <summary>
        /// Unpackets the specified LPS buf.
        /// </summary>
        /// <param name="lpsBuf">The LPS buf.</param>
        /// <returns></returns>
        private static byte[] Unpacket(byte[] lpsBuf)
        {
            List<byte> UnPacketResult = new List<byte>();
            ushort nIndex = 18;
            uint ColumnNumber = 0;
            ushort maxLength = BitConverter.ToUInt16(new byte[] { lpsBuf[3], lpsBuf[2] }, 0);
            while (nIndex++ < maxLength)
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
                    case 0x1E:
                    case 0x1B:
                    case 0x00:
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
        private static void UsasToGb(ref byte c1, ref byte c2)
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
}

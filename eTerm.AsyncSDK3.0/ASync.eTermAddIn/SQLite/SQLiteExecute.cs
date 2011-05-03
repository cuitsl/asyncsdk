using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.Common;
using System.Data;

namespace ASync.eTermAddIn
{
    /// <summary>
    /// 日志执行器
    /// </summary>
    public sealed class SQLiteExecute
    {
        #region 变量定义
        private string __dbString = string.Empty;
        private static readonly SQLiteExecute __instance = new SQLiteExecute();
        private SQLiteDatabase __sqliteDb;
        private string __CurrentTable = string.Empty;
        private InvokeSQLiteDbCommand __Execute;
        #endregion

        /// <summary>
        /// 执行代理
        /// </summary>
        public delegate void InvokeSQLiteDbCommand(string TSession, string TASync, string TSessionIp, byte[] TInPacket, byte[] TOutPacket);


        #region 构造函数
        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteExecute"/> class.
        /// </summary>
        private SQLiteExecute() {
            __dbString = new FileInfo(@"SQLiteDb.s3db").FullName;
            __sqliteDb = new SQLiteDatabase(__dbString);
            BuildLogTable(DateTime.Now);
            __Execute = new InvokeSQLiteDbCommand(ExecuteLog);
        }
        #endregion

        #region 读取日志
        /// <summary>
        /// Packets the SQ lite log.
        /// </summary>
        /// <param name="TSession">The T session.</param>
        /// <param name="TASync">The TA sync.</param>
        /// <param name="Start">The start.</param>
        /// <param name="Expire">The expire.</param>
        /// <returns></returns>
        public List<SQLiteLog> PacketSQLiteLog(string TSession, string TASync, DateTime? Start, DateTime? Expire,int? PageId) {
            int PageSize = 20;
            List<SQLiteLog> Result = new List<SQLiteLog>();
            string Sql = string.Format(@"
    select * from {0} order by TLogId DESC limit {1},{2}
", this.__CurrentTable, ((PageId ?? 1)-1) * PageSize, PageSize);
            IDataReader dr = __sqliteDb.GetSqlStringCommand(Sql).ExecuteReader();
            while (dr.Read()) {
                Result.Add(new SQLiteLog() {
                    DbCommand = Encoding.GetEncoding(@"gb2312").GetString(UnpackPakcet(dr[4] as byte[])),
                    DbDate = (dr[6] as DateTime?)??DateTime.Now,
                    DbResult = Encoding.GetEncoding(@"gb2312").GetString(UnpackPakcet(dr[5] as byte[])),
                     TASync=dr[2].ToString(),
                      TSession=dr[1].ToString()
                });
            }
            return Result;
        }
        #endregion

        #region 解码逻辑
        /// <summary>
        /// 数据解码(适用不同类型客户端).
        /// </summary>
        /// <returns></returns>
        public byte[] UnpackPakcet(byte[] OriginalBytes)
        {
            return Unpacket(OriginalBytes);
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


        #region 写日志
        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="TSession">The T session.</param>
        /// <param name="TSessionIp">The T session ip.</param>
        /// <param name="TData">The T data.</param>
        /// <param name="TLogType">Type of the T log.</param>
        /// <returns></returns>
        public IAsyncResult BeginExecute(string TSession, string TASync, string TSessionIp, byte[] TInPacket, byte[] TOutPacket)
        {
            try
            {
                return __Execute.BeginInvoke(TSession,TASync, TSessionIp, TInPacket,TOutPacket, new AsyncCallback(delegate(IAsyncResult iar)
                                    {
                                        EndExecute(iar);
                                    }), null);
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Ends the execute.
        /// </summary>
        /// <param name="iar">The iar.</param>
        private void EndExecute(IAsyncResult iar) {
            if (iar == null) return;
            try
            {
                __Execute.EndInvoke(iar);
                iar.AsyncWaitHandle.Close();
            }
            catch
            {
                // Hide inside method invoking stack 
                //throw e;
            }
        }



        /// <summary>
        /// Executes the log.
        /// </summary>
        /// <param name="TSession">The T session.</param>
        /// <param name="TSessionIp">The T session ip.</param>
        /// <param name="TData">The T data.</param>
        /// <param name="TLogType">Type of the T log.</param>
        private void ExecuteLog(string TSession, string TASync, string TSessionIp, byte[] TInPacket, byte[] TOutPacket)
        {
            DbCommand sqliteCommand = __sqliteDb.GetSqlStringCommand(string.Format(@"
    INSERT INTO {0}([TSession],[TASync],[TargetIp],[TInPacket],[TOutPacket],[TLogDate]) 
                    VALUES(?,?,?,?,?,?)
", this.__CurrentTable));
            __sqliteDb.AddInParameter(sqliteCommand, System.Data.DbType.String, TSession);
            __sqliteDb.AddInParameter(sqliteCommand, System.Data.DbType.String, TASync);
            __sqliteDb.AddInParameter(sqliteCommand, System.Data.DbType.String, TSessionIp);
            __sqliteDb.AddInParameter(sqliteCommand, System.Data.DbType.Binary, TInPacket);
            __sqliteDb.AddInParameter(sqliteCommand, System.Data.DbType.Binary, TOutPacket);
            __sqliteDb.AddInParameter(sqliteCommand, System.Data.DbType.DateTime, DateTime.Now);
            sqliteCommand.ExecuteNonQuery();
        }
        #endregion

        #region 以月份为单位储存日志
        /// <summary>
        /// Exists the log table.
        /// </summary>
        /// <param name="Current">The current.</param>
        /// <returns></returns>
        private bool ExistLogTable(DateTime Current) {
            return ((long)__sqliteDb.GetSqlStringCommand(string.Format(@"SELECT COUNT(*) FROM sqlite_master where type='table' and name='SQLiteLog{0}';", Current.ToString(@"yyyyMM"))).ExecuteScalar())>0;
        }

        /// <summary>
        /// Builds the log table.
        /// </summary>
        /// <param name="Current">The current.</param>
        /// <returns></returns>
        private bool BuildLogTable(DateTime Current) {
            __CurrentTable = string.Format(@"SQLiteLog{0}", Current.ToString(@"yyyyMM"));
            if (ExistLogTable(Current)) return true;
            __sqliteDb.GetSqlStringCommand( string.Format(@"
CREATE TABLE [SQLiteLog{0}] (
    [TLogId] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
    [TSession] NVARCHAR(50)  NOT NULL,
    [TASync]    NVARCHAR(50)  NOT NULL,
    [TargetIp] NVARCHAR(50)  NOT NULL,
    [TInPacket]  BLOB   NULL,
    [TOutPacket]  BLOB   NULL,
    [TLogDate] DATE  NOT NULL
);

CREATE INDEX [IDX_SQLiteLog{0}_] ON [SQLiteLog{0}](
    [TSession]  ASC,
    [TASync]  ASC,
    [TLogDate]  ASC
);
", Current.ToString(@"yyyyMM"))).ExecuteNonQuery() ;
            return false;
        }
        #endregion

        #region 单例对象
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SQLiteExecute Instance { get { return __instance; } }
        #endregion
    }

    public struct SQLiteLog {
        /// <summary>
        /// Gets or sets the T session.
        /// </summary>
        /// <value>The T session.</value>
        public string TSession { get; set; }

        /// <summary>
        /// Gets or sets the TA sync.
        /// </summary>
        /// <value>The TA sync.</value>
        public string TASync { get; set; }

        /// <summary>
        /// Gets or sets the db command.
        /// </summary>
        /// <value>The db command.</value>
        public string DbCommand { get; set; }

        /// <summary>
        /// Gets or sets the db result.
        /// </summary>
        /// <value>The db result.</value>
        public string DbResult { get; set; }

        /// <summary>
        /// Gets or sets the db date.
        /// </summary>
        /// <value>The db date.</value>
        public DateTime DbDate { get; set; }
    }
}

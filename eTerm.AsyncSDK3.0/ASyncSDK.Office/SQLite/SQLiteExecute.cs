using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ASyncSDK.Office
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
        #endregion

        #region 构造函数
        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteExecute"/> class.
        /// </summary>
        private SQLiteExecute() {
            __dbString = new FileInfo(@"SQLiteDb.s3db").FullName;
            __sqliteDb = new SQLiteDatabase(__dbString);
        }
        #endregion

        #region 以月份为单位储存日志
        /// <summary>
        /// Exists the log table.
        /// </summary>
        /// <param name="Current">The current.</param>
        /// <returns></returns>
        private bool ExistLogTable(DateTime Current) {
            return ((int)__sqliteDb.GetSqlStringCommand(string.Format(@"SELECT COUNT(*) FROM sqlite_master where type='table' and name='SQLiteLog{0}';", Current.ToString(@"yyyyMM"))).ExecuteScalar())>0;
        }

        /// <summary>
        /// Builds the log table.
        /// </summary>
        /// <param name="Current">The current.</param>
        /// <returns></returns>
        private bool BuildLogTable(DateTime Current) {
            __CurrentTable = string.Format(@"SQLiteLog{0}", Current.ToString(@"yyyyMM"));
            if (ExistLogTable(Current)) return true;
            string.Format(@"
CREATE TABLE [SQLiteLog{0}] (
[TLogId] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
[TSession] NVARCHAR(50)  NOT NULL,
[TargetIp] NVARCHAR(50)  NOT NULL,
[TData]  NVARCHAR(2048)   NULL,
[TLogDate] DATE  NOT NULL,
[TLogType] NVARCHAR(25)  NULL
)
", Current.ToString(@"yyyyMM"));
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
}

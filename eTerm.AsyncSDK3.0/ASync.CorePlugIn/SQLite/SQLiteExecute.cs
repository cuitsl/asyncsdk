using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.Common;
using eTerm.AsyncSDK;

namespace ASync.CorePlugIn
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
            if (!ExistTable(@"TAuthorize"))
            {
                BuildTAuthorize();
            }
        }
        #endregion

        #region 创建授权表
        /// <summary>
        /// Exists the log table.
        /// </summary>
        /// <param name="Current">The current.</param>
        /// <returns></returns>
        private bool ExistTable(string TableName)
        {
            return ((long)__sqliteDb.GetSqlStringCommand(string.Format(@"SELECT COUNT(*) FROM sqlite_master where type='table' and name='{0}';", TableName)).ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Builds the T authorize.
        /// </summary>
        private void BuildTAuthorize() {
            __sqliteDb.GetSqlStringCommand(string.Format(@"
CREATE TABLE [TAuthorize] (
[Code] NVARCHAR(50)  UNIQUE NOT NULL PRIMARY KEY,
[CompanyName] NVARCHAR(255)  NULL,
[IpEndPoint] NVARCHAR(25)  NULL,
[ExpireDate] DATE  NULL,
[IsValid] BOOLEAN  NULL,
[LastLogIn] DATE  NULL
);

CREATE INDEX [IDX_TAUTHORIZE_CODE] ON [TAuthorize](
[Code]  DESC
);
", @"")).ExecuteNonQuery();
        }

        /// <summary>
        /// 是否已经过期.
        /// </summary>
        /// <param name="Code">The code.</param>
        /// <returns>是否已经过期</returns>
        public bool CheckTAuthorize(string Code,string IpAddress) {
            if (!IsExist(Code))
            {
                __sqliteDb.GetSqlStringCommand(string.Format(@"INSERT INTO [TAuthorize]([Code],[IpEndPoint],[IsValid],[LastLogIn]) VALUES('{0}','{1}',1,date('now'))", Code,IpAddress)).ExecuteNonQuery();
                return false;
            }
            return ((long)__sqliteDb.GetSqlStringCommand(string.Format(@"SELECT * FROM TAuthorize WHERE Code='{0}' AND IsValid=0;", Code)).ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Determines whether the specified code is exist.
        /// </summary>
        /// <param name="Code">The code.</param>
        /// <returns>
        /// 	<c>true</c> if the specified code is exist; otherwise, <c>false</c>.
        /// </returns>
        private bool IsExist(string Code) {
            return ((long)__sqliteDb.GetSqlStringCommand(string.Format(@"SELECT Count(1) FROM TAuthorize A  WHERE A.Code='{0}';", Code)).ExecuteScalar()) > 0;
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

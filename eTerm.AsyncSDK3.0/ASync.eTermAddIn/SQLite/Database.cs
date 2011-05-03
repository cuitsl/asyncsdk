using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace ASync.eTermAddIn
{
    public class SQLiteDatabase
    {
        SQLiteConnection __Conn;

        /// <summary>
        ///   构造函数
        /// </summary>
        /// <param name="dataSource">数据文件</param>
        public SQLiteDatabase(string dataSource)
            : this(dataSource, false, true)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource">数据文件</param>
        /// <param name="readOnly">是否只读</param>
        /// <param name="pooling">是否使用连接池</param>
        public SQLiteDatabase(string dataSource, bool readOnly, bool pooling)
        {
            if (!File.Exists(dataSource))
            {
                SQLiteConnection.CreateFile(dataSource);
            }
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder
            {
                DataSource = dataSource,
                Version = 3,
                ReadOnly = readOnly,
                Pooling = pooling
            };
            ConnectionString = sb.ToString();
            __Conn = new SQLiteConnection(ConnectionString);
            __Conn.Open();
        }

        /// <summary>
        ///   连接字符串
        /// </summary>
        public static string ConnectionString { get; private set; }

        /// <summary>
        ///   获取Sql语句命令
        /// </summary>
        /// <param name = "query"></param>
        /// <returns></returns>
        public DbCommand GetSqlStringCommand(string query)
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentException("查询字符串不能为空。");
            }

            DbCommand command = new SQLiteCommand(query) { CommandType = CommandType.Text, Connection = __Conn };
            return command;
        }

        public DbCommand GetSqlStringCommand(string query, DbTransaction transaction)
        {
            DbCommand command = GetSqlStringCommand(query);
            command.Transaction = transaction;
            return command;
        }

        public SQLiteTransaction GetTransaction()
        {
            SQLiteConnection conn = new SQLiteConnection(ConnectionString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return conn.BeginTransaction();
        }

        /// <summary>
        ///   添加参数
        /// </summary>
        /// <param name = "command"></param>
        /// <param name = "dbType"></param>
        public void AddInParameter(DbCommand command, DbType dbType)
        {
            command.Parameters.Add(new SQLiteParameter(dbType));
        }

        /// <summary>
        ///   添加参数
        /// </summary>
        /// <param name = "command"></param>
        /// <param name = "dbType"></param>
        /// <param name = "value"></param>
        public void AddInParameter(DbCommand command, DbType dbType, object value)
        {
            if (value == null)
            {
                value = "null";
            }
            command.Parameters.Add(new SQLiteParameter(dbType) { Value = value });
        }

        /// <summary>
        ///   执行
        /// </summary>
        /// <param name = "command"></param>
        /// <returns></returns>
        public long ExecuteNonQuery(DbCommand command)
        {
            //如果包含事务，就执行返回，不管连接
            if (command.Transaction != null)
            {
                return command.ExecuteNonQuery();
            }

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                command.Connection = conn;
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///   执行
        /// </summary>
        /// <param name = "command"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(DbCommand command)
        {
            SQLiteConnection conn = new SQLiteConnection(ConnectionString);
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                command.Connection = conn;
                return command.ExecuteReader();
            }
            catch
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                throw;
            }
        }

        public long ExecuteScalar(DbCommand command)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                command.Connection = conn;
                object obj = command.ExecuteScalar();
                long l = (long)obj;
                return l;
            }
        }
    }
}
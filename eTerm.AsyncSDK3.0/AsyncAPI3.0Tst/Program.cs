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
            DbCommand dbCommand = Db.GetSqlStringCommand(@"select * from SQLiteLog201105 order by TLogId DESC");
            IDataReader DR = dbCommand.ExecuteReader();
            while (DR.Read()) {
                Console.WriteLine(string.Format(@"InPacket:{0} Bytes OutPacket:{1} Bytes", (DR[4] as byte[]).Length, (DR[5] as byte[]).Length));
            }
            Console.ReadLine();

        }
    }
}

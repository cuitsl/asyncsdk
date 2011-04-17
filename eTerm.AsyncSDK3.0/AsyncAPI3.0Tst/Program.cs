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




namespace AsyncAPI3._0Tst {
    class Program {
        static void Main(string[] args) {
            using (FileStream fs = new FileStream(@"C:\Key.Bin", FileMode.OpenOrCreate)) {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(
                new AsyncLicenceKey() {
                    AllowAfterValidate = true,
                    AllowDatabase = true,
                    AllowIntercept = true,
                    Company = @"开发测试",
                    connectionString = string.Empty,
                    ExpireDate = DateTime.Now.AddYears(2),
                    Key = TEACrypter.MD5(Encoding.Default.GetBytes(@"BFEBFBFF000206550026C75B7340")),
                    MaxAsync = 10,
                    MaxCommandPerMonth = 90000,
                    MaxTSession = 10,
                    providerName = @"System.Data.SqlDataClient",
                    RemainingMinutes = 400000
                }.XmlSerialize(TEACrypter.MD5(Encoding.Default.GetBytes(@"BFEBFBFF000206550026C75B7340"))));
                bw.Flush();
                bw.Close();
            }
            Console.ReadLine();

        }
    }
}

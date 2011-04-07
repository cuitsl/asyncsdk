using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.ASynClientSDK;
using System.Net;
using System.IO;
using eTerm.ASynClientSDK.Base;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Contexts;
using System.Transactions;
using eTerm.AsyncSDK;
using eTerm.AsyncSDK.Net;
using eTerm.ASynClientSDK.Utils;



namespace AsyncAPI3._0Tst {
    class Program {
        static void Main(string[] args) {
            eTerm.AsyncSDK.LicenceManager.Instance.BeginValidate(new AsyncCallback(
                delegate(IAsyncResult iar)
                {
                    try {
                        if (!eTerm.AsyncSDK.LicenceManager.Instance.EndValidate(iar)) {

                        }
                        else {
                            //激活配置
                            eTerm443Async Async = new eTerm443Async(@"pek3.eterm.com.cn", 443, @"", @"", 0x00, 0x00);
                            Async.Connect();
                        }
                    }
                    catch (Exception ex) {

                    }
                }), new FileInfo(@"D:\SouceCode\Personal\eTerm.AsyncSDK3.0\ASyncSDK.Office\bin\Release\Key.Bin").FullName);
            //AsyncStackNet.Instance.ASyncSetupFile = @"D:\SouceCode\Personal\eTerm.AsyncSDK3.0\ASyncSDK.Office\bin\Release\Key.Bin";
            
            Console.ReadLine();

        }
    }
}

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
using eTerm.AsyncSDK.Util;



namespace AsyncAPI3._0Tst {
    class Program {
        static void Main(string[] args) {
            using (FileStream fs = new FileStream(@"C:\Setup.Bin", FileMode.Open)) {
                byte[] buffer = new byte[fs.Length];
                BinaryReader br = new BinaryReader(fs, Encoding.GetEncoding(@"utf-8"));
                br.Read(buffer, 0, buffer.Length);
                SystemSetup setup= new SystemSetup().DeXmlSerialize(TEACrypter.GetDefaultKey, buffer);
            }
            Console.ReadLine();

        }
    }
}

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
using eTerm.ASynClientSDK.Utils;



namespace AsyncAPI3._0Tst {
    class Program {
        static void Main(string[] args) {
            RTResult Result = new RTCommand().retrieve(@"JNV1R5") as RTResult;
            Console.ReadLine();

        }
    }
}

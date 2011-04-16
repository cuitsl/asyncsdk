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




namespace AsyncAPI3._0Tst {
    class Program {
        static void Main(string[] args) {
            RTCommand Rt = new RTCommand();
            Rt.retrieve(@"HYF126");

            Console.ReadLine();

        }
    }
}

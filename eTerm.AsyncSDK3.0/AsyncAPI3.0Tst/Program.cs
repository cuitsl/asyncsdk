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
using eTerm.ASynClientSDK;




namespace AsyncAPI3._0Tst {
    class Program {
        static void Main(string[] args) {
            AVResult result= new AVCommand().getAvailability(@"SHA", @"PEK", DateTime.Now.AddDays(10)) as AVResult;
            Console.ReadLine();

        }
    }
}

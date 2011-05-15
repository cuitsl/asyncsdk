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

using ASyncSDK.Office;
using System.Data;
using System.Data.Common;
using eTerm.AsyncSDK;






namespace AsyncAPI3._0Tst {
    class Program {



        static void Main(string[] args)
        {

            FtpClient myFtp = new FtpClient(@"127.0.0.1", @"", @"");
            myFtp.Login();
            string[] files = myFtp.GetFileList();
            myFtp.Download(files[0], string.Format(@"C:\{0}", files[0]));
            //myFtp.BeginGetFileList(new AsyncCallback(delegate(IAsyncResult iar) {
            //    foreach (string key in (iar.AsyncState as eTerm.AsyncSDK.FtpClient.GetFileListCallback).EndInvoke(iar)) {
            //        myFtp.BeginDownload(key, new AsyncCallback(delegate(IAsyncResult iar1) {
            //            Console.WriteLine(@"Download        {0}", key);
            //        }));
            //    }
            //    iar.AsyncWaitHandle.Close();
            //}));

            Console.ReadLine();

        }

        private delegate int MyMethod();
        private int method()
        {
            Thread.Sleep(10000);
            return 100;
        }
        private void MethodCompleted(IAsyncResult asyncResult)
        {
            if (asyncResult == null) return;
            //textBox1.Text = (asyncResult.AsyncState as MyMethod).EndInvoke(asyncResult).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MyMethod my = method;
            IAsyncResult asyncResult = my.BeginInvoke(MethodCompleted, my);
        }


    }
}

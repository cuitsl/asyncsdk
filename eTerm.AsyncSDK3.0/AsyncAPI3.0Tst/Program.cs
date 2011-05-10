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
using eTerm.ASynClientSDK;





namespace AsyncAPI3._0Tst {
    class Program {



        static WinSocket socket = new WinSocket(@"guzm", @"guzm", @"BAD494E84A25D8BCA163CD3FB8DC4511");
        //static WinSocket socket = new WinSocket(@"guzm", @"guzm", @"92666505CE75444EE14BE2EBC2F10A60");
        static void Main(string[] args)
        {
            EBTEtermService service = new EBTEtermService();
            service.GetRtResult("HTQF6M");
            /*
            socket.OnAsynConnect += new EventHandler<AsyncEventArgs<WinSocket>>(
                    delegate(object sender, AsyncEventArgs<WinSocket> e) {
                        Console.WriteLine(@"OnAsynConnect.......");
                    }
                );
            socket.OnBeginConnect += new EventHandler<AsyncEventArgs<WinSocket>>(
                    delegate(object sender, AsyncEventArgs<WinSocket> e) {
                        Console.WriteLine(@"OnBeginConnect.....");
                    }
                );
            socket.OnAsyncDisconnect+=new EventHandler<AsyncEventArgs<WinSocket>>(
                    delegate(object sender, AsyncEventArgs<WinSocket> e)
                    {
                        Console.WriteLine(@"OnAsyncDisconnect.......");
                    }
                );
            socket.OnPacketSent += new EventHandler<AsyncEventArgs<eTermApiPacket, eTermApiPacket, WinSocket>>(
                    delegate(object sender, AsyncEventArgs<eTermApiPacket, eTermApiPacket, WinSocket> e) {
                        Console.WriteLine(@"OnPacketSent......");
                    }
                );
            socket.OnReadPacket += new EventHandler<AsyncEventArgs<eTermApiPacket, eTermApiPacket, WinSocket>>(
                    delegate(object sender, AsyncEventArgs<eTermApiPacket, eTermApiPacket, WinSocket> e)
                    {

                        #region 判断做下一条指令
                        if (Regex.IsMatch(@"\+", Encoding.GetEncoding(@"gb2312").GetString(e.InPacket.OriginalBytes), RegexOptions.IgnoreCase| RegexOptions.Multiline))
                            e.Session.SendPacket(@"PN");
                        #endregion
                        Console.WriteLine(string.Format(@"OnReadPacket.......
{2}
{0}
[{1}]",Encoding.GetEncoding(@"gb2312").GetString( e.InPacket.OriginalBytes),e.InPacket.Sequence,Encoding.GetEncoding(@"gb2312").GetString(e.OutPacket.OriginalBytes)));
                    }
                );
            socket.OnValidated += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        Console.WriteLine(@"OnValidated.......");
                    }
                );
            socket.Connect(@"asyncsdk.gicp.net", 350);
            //socket.Connect(@"127.0.0.1", 350);
            while (true)
            {
                string cmd = Console.ReadLine();
                if (cmd.ToLower().Trim() == "exit") break;
                socket.SendPacket(cmd);
            }
            */
            Console.ReadLine();

        }


    }
}

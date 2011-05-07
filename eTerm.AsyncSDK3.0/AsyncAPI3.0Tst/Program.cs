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
        static WinSocket socket = new WinSocket(@"guzm", @"guzm", @"07A02091492B8FFFA74315A16CE7231B");
        static void Main(string[] args) {

            
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
                    delegate(object sender, AsyncEventArgs<eTermApiPacket, eTermApiPacket, WinSocket> e) {
                        Console.WriteLine(string.Format(@"OnReadPacket.......
{0}[{1}]",Encoding.GetEncoding(@"gb2312").GetString( e.InPacket.OriginalBytes),e.InPacket.Sequence));
                    }
                );
            socket.OnValidated += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        Console.WriteLine(@"OnValidated.......");
                    }
                );
            socket.Connect(@"127.0.0.1", 350);
            while (true) {
                string cmd = Console.ReadLine();
                socket.SendPacket(cmd);
            }
            Console.ReadLine();

        }


    }
}

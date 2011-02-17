using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.Net;
using System.Net.Sockets;
using System.IO;
using eTerm.AsyncSDK;
using eTerm.AsyncSDK.Net;
using eTerm.AsyncSDK.Core;
using eTerm.AsyncSDK.Util;
using System.Text.RegularExpressions;
using System.Threading;
using System.Management;
namespace AsyncSDK3._0Tst {



    class Program {

        /// <summary>
        /// Gets the cpu SN.
        /// </summary>
        /// <returns></returns>
        private static string GetCpuSN() {
            ManagementClass cimobject = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = cimobject.GetInstances();
            StringBuilder sb = new StringBuilder();
            foreach (ManagementObject mo in moc) {
                sb.Append(mo.Properties["ProcessorId"].Value.ToString().Replace(":", string.Empty).Replace(" ", string.Empty));
            }
            //sb.Append("1qaz@WSX3edc");
            return sb.ToString();
        }


        static void Main(string[] args) {
            
            /*
            SystemSetup setup = new SystemSetup().DeserializeObject(@"SETUP.BIN", TEACrypter.GetDefaultKey);
            setup.AsynCollection.RemoveAt(0);
            setup.AsynCollection[0].IsOpen = true;
            setup.SessionCollection[0].TSessionForbidCmd = new List<string>() { "TEST"};
            setup.XmlSerialize(TEACrypter.GetDefaultKey, @"C:\Setup.Bin");
            */
            /**/
            string Cpu = GetCpuSN();
            TEACrypter Crypter = new TEACrypter();
            byte[] keys = TEACrypter.MD5(Encoding.Default.GetBytes(Cpu));
            byte[] Result = Crypter.Encrypt(Encoding.Default.GetBytes(Cpu), keys);
            


            
            AsyncLicenceKey Key = new AsyncLicenceKey() { 
                Company = "开发机授权", 
                ExpireDate = DateTime.Now.AddMonths(1), 
                Key = Result, 
                MaxAsync = 100,
                MaxTSession=500, 
                AllowDatabase=true,
                MaxCommandPerMonth=20000,
                connectionString = @"Data Source=(local);Initial Catalog=Async;User ID=sa;Password=Password01!",
                providerName=@"System.Data.SqlClient",
                AllowAfterValidate=true,
                AllowIntercept=true,
                RemainingMinutes = ((TimeSpan)(DateTime.Now.AddMonths(1)-DateTime.Now)).TotalMinutes
            };
            
            byte[] Buffer= Key.XmlSerialize(keys,@"C:\Key.Bin");
            Key = Key.DeXmlSerialize(keys, Buffer);
            


            AsyncStackNet.Instance.ASyncSetupFile = @"SETUP.BIN";

            AsyncStackNet.Instance.AfterIntercept = new InterceptCallback(delegate(AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e) {
                e.Session.SendPacket(__eTerm443Packet.BuildSessionPacket(e.Session.SID,e.Session.RID,"指令被禁止"));
            });

            /*
            AsyncStackNet.Instance.AfterPacket =new AfterPacketCallback(delegate(AsyncEventArgs<eTerm443Packet,eTerm443Packet,eTerm443Async> Args,PlugInSetup PlugIn)
            {
                Console.WriteLine(@"开始后续插件执行{0}", PlugIn.PlugInName);
                PlugIn.PlugInstance.BeginExecute(new AsyncCallback(delegate(IAsyncResult iar)
                {
                    Console.WriteLine(@"后续插件{0}执行完成！", PlugIn.PlugInName);
                }), Args.Session, Args.InPacket, Args.OutPacket);
            });
            */
            AsyncStackNet.Instance.StackNetPoint = new IPEndPoint(IPAddress.Any, 3500);
            AsyncStackNet.Instance.RID = 0x51;
            AsyncStackNet.Instance.SID = 0x27;
            AsyncStackNet.Instance.OnAsyncDisconnect += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e) {
                        Console.WriteLine(string.Format(@"OnAsyncDisconnect {0} On {1}........", e.Session,DateTime.Now));
                    }
                );

            AsyncStackNet.Instance.OnTSessionPacketSent += new EventHandler<AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Packet, eTerm363Packet, eTerm363Session> e) {
                        Console.WriteLine("OnTSessionPacketSent {0} {1}",e.Session.AsyncSocket.RemoteEndPoint.ToString(),DateTime.Now);
                    }
                );

            AsyncStackNet.Instance.OnAsyncReadPacket += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Packet, eTerm443Async> e)
                    {
                        Console.WriteLine(
                            string.Format(@"OnAsyncReadPacket:{0}
InPacket:{1}
OutPacket:{2}"
                            , e.Session.AsyncSocket.RemoteEndPoint.ToString()
                            , Encoding.GetEncoding("gb2312").GetString(e.Session.UnInPakcet(e.OutPacket))
                            , Encoding.GetEncoding("gb2312").GetString(e.Session.UnOutPakcet(e.InPacket)).Replace("\0",string.Empty)
                            ));
                        if (e.Session.TSession == null) return;

                    }
                );
            AsyncStackNet.Instance.OnAsyncValidated += new EventHandler<AsyncEventArgs<eTerm443Packet, eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Packet, eTerm443Async> e) {
                        Console.WriteLine("{0} OnValidated With {1}", e.Session, e.Session.AsyncSocket.RemoteEndPoint);
                    }
                );
            AsyncStackNet.Instance.TSessionValidate = new AsyncBaseServer<eTerm363Session, eTerm363Packet>.ValidateCallback(delegate(eTerm363Session s, eTerm363Packet p)
            {
                s.UnpakcetSession(p);
                TSessionSetup TSession = AsyncStackNet.Instance.ASyncSetup.SessionCollection.Single<TSessionSetup>(Fun => Fun.SessionPass == s.userPass && Fun.SessionCode == s.userName);
                if (TSession == null) return false;
                s.TSessionInterval = TSession.SessionExpire;
                s.UnallowableReg = TSession.ForbidCmdReg;
                return true;
            });

            AsyncStackNet.Instance.TSessionReconnectValidate = new AsyncBase<eTerm443Async, eTerm443Packet>.ValidateTSessionCallback(
                delegate(eTerm443Packet Packet, eTerm443Async Async)
                {
                    return Async.ReconnectCount <= 5;
                });

            AsyncStackNet.Instance.OnTSessionAssign += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e) {
                        Console.WriteLine("Session {2} Assign To {0} On {1}", e.Session, DateTime.Now, e.Session.Async443);
                    }
                );
            AsyncStackNet.Instance.OnTSessionAccept += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e)
                    {
                        Console.WriteLine("OnTSessionAccept {0}", e.Session);
                    }
                );
            AsyncStackNet.Instance.OnTSessionClosed += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e)
                    {
                        Console.WriteLine("OnTSessionClosed {0}", e.Session);
                    }
                );
            AsyncStackNet.Instance.OnAsyncTimeout += new EventHandler<AsyncEventArgs<eTerm443Async>>(
                    delegate(object sender, AsyncEventArgs<eTerm443Async> e) {
                        Console.WriteLine("OnAsyncTimeout {0}", e.Session);
                    }
                );
            AsyncStackNet.Instance.OnTSessionRelease += new EventHandler<AsyncEventArgs<eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Session> e) {
                        Console.WriteLine("Session {0} Release {2} On {1}", e.Session, DateTime.Now, e.Session.Async443);
                    }
                );

            AsyncStackNet.Instance.OnTSessionReadPacket += new EventHandler<AsyncEventArgs<eTerm363Packet,eTerm363Packet, eTerm363Session>>(
                    delegate(object sender, AsyncEventArgs<eTerm363Packet,eTerm363Packet, eTerm363Session> e)
                    {
                        Console.WriteLine("OnReadPacket From {0} Packet Sequence:{1} Total Bytes:{2:f2} KBytes ", e.Session.AsyncSocket.RemoteEndPoint, e.InPacket.Sequence, e.Session.TotalBytes);
                    }
                );


            LicenceManager.Instance.BeginValidate(new AsyncCallback(
                delegate(IAsyncResult iar) {
                    try {
                        if (!LicenceManager.Instance.EndValidate(iar))
                            Console.WriteLine("Validate Error");
                        else {
                            //ContextInstance.Instance.providerName = LicenceManager.Instance.LicenceBody.providerName;
                            //ContextInstance.Instance.connectionString = LicenceManager.Instance.LicenceBody.connectionString;
                            //激活配置
                            AsyncStackNet.Instance.BeginAsync();

                            AsyncStackNet.Instance.BeginReflectorPlugIn(new AsyncCallback(delegate(IAsyncResult iar1)
                            {
                                AsyncStackNet.Instance.EndReflectorPlugIn(iar1);
                            }));
                        }
                    }
                    catch(Exception ex) {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine();
                        Console.WriteLine(ex.StackTrace);
                    }
                }), @"Key.bin");
            Console.ReadKey();
        }
    }
}

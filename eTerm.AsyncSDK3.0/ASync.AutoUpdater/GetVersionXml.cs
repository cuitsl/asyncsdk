using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.IO;
using eTerm.AsyncSDK.Core;
using eTerm.AsyncSDK.Net;

namespace ASync.AutoUpdater {
    [AfterASynCommand("!GetVersionXml")]
    public sealed class GetVersionXml : BaseASyncPlugIn {
        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            new FileSendSocket(@"Version.Bin", SESSION).BeginSend();
        }

        protected override bool ValidatePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            return Key.AllowIntercept && SESSION != null;
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public override string Description {
            get {
                return "获取更新版本文件";
            }
        }
    }


    internal class FileSendSocket {
        public string FileName { set; private get; }
        private byte[] FileNameBuffer { get; set; }
        public eTerm363Session TargetSession { set; private get; }
        private byte[] __myBuffer = new byte[512];
        public long FileSize { set; private get; }
        private FileStream fs;
        public FileSendSocket(string fileName,eTerm363Session Session) {
            this.FileName = new FileInfo(fileName).FullName;
            this.TargetSession = Session;
            FileSize=new FileInfo(fileName).Length;
            fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, __myBuffer.Length, true);
            FileNameBuffer =Encoding.GetEncoding(@"gb2312").GetBytes( fileName.PadLeft(25, ' '));
        }

        public void BeginSend() {
            //using (
                fs.BeginRead(__myBuffer, 0, __myBuffer.Length, new AsyncCallback(delegate(IAsyncResult iar) {
                    int ReadCount=(iar.AsyncState as FileStream).EndRead(iar);
                    SendToSession(__myBuffer, ReadCount);
                    if (ReadCount == __myBuffer.Length)
                        BeginSend();
                    else {
                        fs.Close();
                        fs.Dispose();
                        SendToSession(new byte[] {0x00,0x00 }, 2);
                    }
                }), fs);
        }

        private void SendToSession(byte[] buffer,int bufferCount) {
            TargetSession.SendPacket(__eTerm443Packet.BuildSessionPacket(TargetSession.SID, TargetSession.RID, BuildPacket(buffer,bufferCount)));
        }

        private byte[] BuildPacket(byte[] buffer, int bufferCount) {
            List<byte> bufferSend = new List<byte>();
            //Buffer.BlockCopy(buffer, 0, tmpBytes, 0, bufferCount);
            bufferSend.AddRange(BitConverter.GetBytes(this.FileNameBuffer.Length + bufferCount + sizeof(long)));
            bufferSend.AddRange(FileNameBuffer);
            bufferSend.AddRange(buffer);
            return bufferSend.ToArray();
        }
    }
}

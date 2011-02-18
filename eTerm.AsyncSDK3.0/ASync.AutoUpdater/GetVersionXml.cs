using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.IO;
using eTerm.AsyncSDK.Core;

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
        public eTerm363Session TargetSession { set; private get; }
        private byte[] __myBuffer = new byte[512];
        public long FileSize { set; private get; }
        public FileSendSocket(string fileName,eTerm363Session Session) {
            this.FileName = fileName;
            this.TargetSession = Session;
            FileSize=new FileInfo(@fileName).Length;
        }

        public void BeginSend() {
            using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, __myBuffer.Length, true)) {
                fs.BeginRead(__myBuffer, 0, __myBuffer.Length, new AsyncCallback(delegate(IAsyncResult iar) {
                    int ReadCount=(iar.AsyncState as FileStream).EndRead(iar);
                    SendToSession(__myBuffer, ReadCount);
                    if (ReadCount == __myBuffer.Length)
                        BeginSend();
                    else {
                        SendToSession(new byte[] {0x00,0x00 }, 2);
                    }
                }), fs);
            }
        }

        private void SendToSession(byte[] buffer,int bufferCount) {
            TargetSession.SendPacket(BuildPacket(buffer, bufferCount));
        }

        private byte[] BuildPacket(byte[] buffer, int bufferCount) {
            List<byte> bufferSend = new List<byte>();
            byte[] tmpBytes = new byte[bufferCount];
            Buffer.BlockCopy(buffer, 0, tmpBytes, 0, bufferCount);
            bufferSend.AddRange(BitConverter.GetBytes(Encoding.GetEncoding("gb2312").GetByteCount(FileName) + bufferCount+sizeof(long)));
            bufferSend.AddRange(BitConverter.GetBytes(Encoding.GetEncoding("gb2312").GetByteCount(FileName)));
            bufferSend.AddRange(BitConverter.GetBytes(FileSize));
            bufferSend.AddRange(Encoding.GetEncoding("gb2312").GetBytes(FileName));
            bufferSend.AddRange(tmpBytes);
            return bufferSend.ToArray();
        }
    }
}

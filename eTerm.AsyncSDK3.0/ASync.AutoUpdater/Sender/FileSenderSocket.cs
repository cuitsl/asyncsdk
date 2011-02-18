using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.IO;
using eTerm.AsyncSDK.Net;

namespace ASync.AutoUpdater {
    #region Version Packet
    internal class VersionPacket : _Packet<VersionSession> {
        public string FileName { get; private set; }
        public long FileSize { get; private set; }
        private int length = 0;
        private byte[] buffer;
        public override byte[] AfterBody {
            get { return new byte[]{0x00,0x00}; }
        }

        public override byte[] GetPacketBodyBytes() {
            return buffer;
        }

        public override void GetPacketCommand() {
            
        }

        public override int GetPakcetLength() {
            using (MemoryStream ms = new MemoryStream(this.OriginalBytes)) {
                BinaryReader br = new BinaryReader(ms);
                length = br.ReadInt32();
                int fileNameLength = br.ReadInt32();
                FileSize = br.ReadInt64();
                FileName=Encoding.GetEncoding("gb2312").GetString( br.ReadBytes(fileNameLength));
                buffer = br.ReadBytes(length - fileNameLength-sizeof(long));
                br.Close();
            }
            return length;
        }

        protected override byte TSessionVersion {
            get { return 0x00; }
        }

        public override bool ValidatePacket() {
            return true;
        }
    }
    
    #endregion

    #region Version Session
    public class VersionSession : eTerm443Async {

    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace ASync.AutoUpdater {
    #region Version Packet
    internal class VersionPacket : _Packet<VersionSession> {

        public override byte[] AfterBody {
            get { return new byte[]{0x00,0x00}; }
        }

        public override byte[] GetPacketBodyBytes() {
            throw new NotImplementedException();
        }

        public override void GetPacketCommand() {
            throw new NotImplementedException();
        }

        public override int GetPakcetLength() {
            throw new NotImplementedException();
        }

        protected override byte TSessionVersion {
            get { throw new NotImplementedException(); }
        }

        public override bool ValidatePacket() {
            throw new NotImplementedException();
        }
    }
    
    #endregion

    #region Version Session
    internal class VersionSession : AsyncBase<VersionSession, VersionPacket> {

        public override void SendPacket(string Packet) {
            throw new NotImplementedException();
        }
    }
    #endregion
}

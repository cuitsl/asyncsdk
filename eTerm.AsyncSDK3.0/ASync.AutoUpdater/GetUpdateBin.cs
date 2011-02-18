using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Net;
using eTerm.AsyncSDK;
using eTerm.AsyncSDK.Core;


namespace ASync.AutoUpdater {
    [AfterASynCommand("!GetVersionXml")]
    public class GetUpdateBin : BaseASyncPlugIn {

        private eTerm363Session mySession;

        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            VersionSession Session =new VersionSession();

            mySession = SESSION;
            Session.OnReadPacket+=new EventHandler<AsyncEventArgs<eTerm443Packet,eTerm443Packet,eTerm443Async>>(
                    delegate(object sender,AsyncEventArgs<eTerm443Packet,eTerm443Packet,eTerm443Async> e){
                        mySession.SendPacket(e.InPacket.OriginalBytes);   
                    }
                );
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
                return "获取更新文件";
            }
        }

    }
}

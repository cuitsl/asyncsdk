using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace ASync.AutoUpdater {
    [AfterASynCommand("!GetVersionXml")]
    public sealed class SendVersionXml : BaseASyncPlugIn {
        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            throw new NotImplementedException();
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
                return "用户PNR查询插件";
            }
        }
    }
}

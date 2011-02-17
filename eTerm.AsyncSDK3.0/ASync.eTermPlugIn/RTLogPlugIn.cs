using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.Text.RegularExpressions;

namespace ASync.eTermPlugIn {
    [AfterASynCommand("RT")]
    public sealed class RTLogPlugIn : BaseAfterCmd {
        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="SESSION">会话端.</param>
        /// <param name="InPacket">入口数据包.</param>
        /// <param name="OutPacket">出口数据包.</param>
        /// <param name="Key">The key.</param>
        protected override void ExecutePlugIn(eTerm.AsyncSDK.Net.eTerm443Async SESSION, eTerm.AsyncSDK.Net.eTerm443Packet InPacket, eTerm.AsyncSDK.Net.eTerm443Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            ContextInstance.Instance.providerName = Key.providerName;
            ContextInstance.Instance.connectionString = Key.connectionString;
            string Pnr=Regex.Match( Encoding.GetEncoding("gb2312").GetString(SESSION.UnInPakcet(OutPacket)).Trim(),@"^rt\s+([A-Z0-9]+)", RegexOptions.Multiline| RegexOptions.IgnoreCase).Groups[1].Value;
            if (Async_PNR.SingleOrDefault(PNR => PNR.PnrCode.ToLower() == Pnr.ToLower()) == null) {
                new Async_PNR() {
                    PnrCode = Pnr,
                    SourcePnr = string.Empty,
                    ClientSession = SESSION.TSession.userName,
                    UpdateDate = DateTime.Now
                }.Add();
            }
        }

        /// <summary>
        /// 验证可用性.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        protected override bool ValidatePlugIn(eTerm.AsyncSDK.Net.eTerm443Async SESSION, eTerm.AsyncSDK.Net.eTerm443Packet InPacket, eTerm.AsyncSDK.Net.eTerm443Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            return Key.AllowDatabase && SESSION != null && SESSION.TSession != null;
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public override string Description {
            get {
                return "PNR记录插件";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.Text.RegularExpressions;
using eTerm.AsyncSDK.Net;

namespace ASync.eTermPlugIn {
    [AfterASynCommand("!RT")]
    public sealed class RTLogPlugIn : BaseASyncPlugIn {

        /// <summary>
        /// 验证可用性.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        protected override bool ValidatePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            return Key.AllowDatabase && SESSION != null;
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

        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            ContextInstance.Instance.providerName = Key.providerName;
            ContextInstance.Instance.connectionString = Key.connectionString;
            //eTerm363Session ClientSession = SESSION;
            string Cmd = Encoding.GetEncoding("gb2312").GetString(SESSION.UnOutPakcet(InPacket)).Trim();
            Match PnrMatch = Regex.Match(Cmd, @"!RT\s{0,}([A-Z0-9]{5,6})", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            //string Pnr = Regex.Match(Encoding.GetEncoding("gb2312").GetString(SESSION.UnOutPakcet(InPacket)).Trim(), @"^rt\s+([A-Z0-9]+)", RegexOptions.Multiline | RegexOptions.IgnoreCase).Groups[1].Value;
            if (
                Regex.IsMatch(Cmd, @"!RT\s{1,}([A-Z0-9]{5,6})", RegexOptions.Multiline | RegexOptions.IgnoreCase)
                &&
                Async_PNR.SingleOrDefault(PNR => PNR.PnrCode.ToLower() == PnrMatch.Groups[1].Value.ToLower()) == null) {
                new Async_PNR() {
                    PnrCode = PnrMatch.Groups[1].Value,
                    SourcePnr = string.Empty,
                    ClientSession = SESSION.userName,
                    UpdateDate = DateTime.Now
                }.Add();
                SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, string.Format(@"PNR {0} 已入库",PnrMatch.Groups[1].Value)));
                return;
            }
            SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, @"入库失败,请检查PNR格式"));
        }
    }
}

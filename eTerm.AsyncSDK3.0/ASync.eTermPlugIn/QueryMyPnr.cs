using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.Text.RegularExpressions;
using eTerm.AsyncSDK.Core;
using eTerm.AsyncSDK.Net;

namespace ASync.eTermPlugIn {
    [AfterASynCommand("!mypnr")]
    public sealed class QueryMyPnr:BaseASyncPlugIn {
        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="SESSION">会话端.</param>
        /// <param name="InPacket">入口数据包.</param>
        /// <param name="OutPacket">出口数据包.</param>
        /// <param name="Key">The key.</param>
        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            ContextInstance.Instance.providerName = Key.providerName;
            ContextInstance.Instance.connectionString = Key.connectionString;
            //eTerm363Session ClientSession = SESSION;
            MatchCollection ExpireDates = Regex.Matches(Encoding.GetEncoding("gb2312").GetString(SESSION.UnOutPakcet(InPacket)).Trim(), @"(\d{4}\-\d{1,2}\-\d{1,2})", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            StringBuilder sb = new StringBuilder();
            IEnumerable<Async_PNR> PnrResult = null;
            if (ExpireDates.Count == 1) {
                PnrResult = Async_PNR.Find(PNR =>
                    PNR.UpdateDate >= DateTime.Parse(ExpireDates[0].Value)
                    &&
                    PNR.ClientSession == SESSION.userName
                    ).OrderByDescending<Async_PNR, DateTime?>(PNR => PNR.UpdateDate).Take<Async_PNR>(10);
            }
            else if (ExpireDates.Count == 2) {
                PnrResult = Async_PNR.Find(PNR =>
                    PNR.UpdateDate >= DateTime.Parse(ExpireDates[0].Value)
                    &&
                    PNR.UpdateDate <= DateTime.Parse(ExpireDates[1].Value)
                    &&
                    PNR.ClientSession == SESSION.userName
                    ).OrderByDescending<Async_PNR, DateTime?>(PNR => PNR.UpdateDate).Take<Async_PNR>(10);
            }
            else {
                sb.Append(@"查询指令格式不正确:!mypnr 2011-01-01  !mypnr 2011-01-01 2011-01-20");
            }
            if (PnrResult != null) {
                IEnumerator<Async_PNR> Enumerator = PnrResult.GetEnumerator();
                while (Enumerator.MoveNext()) {
                    sb.AppendFormat("{0}    ", Enumerator.Current.PnrCode, Enumerator.Current.UpdateDate.Value.ToString("yyyy-MM-dd"));
                }
            }
            SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, sb.ToString()));
        }

        /// <summary>
        /// 验证可用性.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
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

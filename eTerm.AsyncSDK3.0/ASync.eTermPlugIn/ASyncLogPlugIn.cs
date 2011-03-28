using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK;

namespace ASync.eTermPlugIn {
    [AfterASynCommand("AV")]
    [AfterASynCommand("FD")]
    [AfterASynCommand("PN")]
    [AfterASynCommand("IG")]
    [AfterASynCommand("SK")]
    [AfterASynCommand("RT")]
    [AfterASynCommand("@")]
    [AfterASynCommand(@"\")]
    [AfterASynCommand(@"XEPNR@")]
    [AfterASynCommand(@"SS")]
    [AfterASynCommand(@"SD")]
    [AfterASynCommand(@"NM")]
    [AfterASynCommand(@"RMK")]
    [AfterASynCommand(@"NFD")]
    [AfterASynCommand(@"EC")]
    [AfterASynCommand(@"ETDZ")]
    [AfterASynCommand(@"XC")]
    public class ASyncLogPlugIn : BaseAfterCmd {
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
            string ClientSession=SESSION.TSession.userName;
            string eTermSession=SESSION.userName;
            new Async_Log() {
                ASynCommand = Encoding.GetEncoding("gb2312").GetString(SESSION.UnInPakcet(OutPacket)),
                ASyncResult = Encoding.GetEncoding("gb2312").GetString(SESSION.UnOutPakcet(InPacket)),
                 ClientSession=ClientSession,
                eTermSession = eTermSession,
                   LogDate=DateTime.Now
            }.Add();
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
            return (AsyncStackNet.Instance.ASyncSetup.AllowLog??false)&& Key.AllowDatabase && SESSION != null && SESSION.TSession!=null;
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public override string Description {
            get {
                return "日志记录插件";
            }
        }

    }
}

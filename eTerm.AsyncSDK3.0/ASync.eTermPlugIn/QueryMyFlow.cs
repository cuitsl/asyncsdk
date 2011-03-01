using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Net;

namespace ASync.eTermPlugIn {
    /// <summary>
    /// 流量查询插件
    /// </summary>
    [AfterASynCommand("!myflow")]
    public sealed class QueryMyFlow : BaseASyncPlugIn {
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
            DateTime? start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime? end = start.Value.AddMonths(1);//.AddDays(-1);
            SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, string.Format(@"{0}  至  {1}   流量为:{2} 条有效指令", start, end, Async_Log.Find(Log => Log.LogDate >= start && Log.LogDate <= end && Log.ClientSession == SESSION.userName).Count)));
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
            return Key.AllowIntercept &&Key.AllowDatabase&& SESSION != null;
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public override string Description {
            get {
                return "用户当月流量查询插件";
            }
        }
    }
}

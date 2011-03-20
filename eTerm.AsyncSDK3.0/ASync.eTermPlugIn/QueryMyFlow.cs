using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Net;
using eTerm.AsyncSDK;

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
            SocketTraffic Traffic=new SocketTraffic(DateTime.Now.ToString("yyyyMM")){ Traffic=0};
            if (
                AsyncStackNet.Instance.ASyncSetup.SessionCollection.Contains(new TSessionSetup(SESSION.userName))
                &&
                AsyncStackNet.Instance.ASyncSetup.SessionCollection[
                    AsyncStackNet.Instance.ASyncSetup.SessionCollection.IndexOf(new TSessionSetup(SESSION.userName))
                    ].Traffics.Contains(new SocketTraffic(DateTime.Now.ToString(@"yyyyMM")))
                ) {
               
                Traffic=AsyncStackNet.Instance.ASyncSetup.SessionCollection[
                    AsyncStackNet.Instance.ASyncSetup.SessionCollection.IndexOf(new TSessionSetup(SESSION.userName))
                    ].Traffics[
                        AsyncStackNet.Instance.ASyncSetup.SessionCollection[
                    AsyncStackNet.Instance.ASyncSetup.SessionCollection.IndexOf(new TSessionSetup(SESSION.userName))
                    ].Traffics.IndexOf(new SocketTraffic(DateTime.Now.ToString(@"yyyyMM")))];
            }
            SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, string.Format(@"{0}  总流量为:{1} 条 最后更新 {2}",Traffic.MonthString,Traffic.Traffic,Traffic.UpdateDate )));
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

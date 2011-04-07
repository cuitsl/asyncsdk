using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Net;

namespace ASync.CorePlugIn {
    [AfterASynCommand("!UpdateDate", IsSystem = true)]
    public sealed class SDKDate : BaseASyncPlugIn {
        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="SESSION">会话端.</param>
        /// <param name="InPacket">入口数据包.</param>
        /// <param name="OutPacket">出口数据包.</param>
        /// <param name="Key">The key.</param>
        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            string Cmd = Encoding.GetEncoding("gb2312").GetString(SESSION.UnOutPakcet(InPacket)).Trim();
            SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss")));
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
            return SESSION != null;
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public override string Description {
            get {
                return "主机日期获取";
            }
        }
    }
}

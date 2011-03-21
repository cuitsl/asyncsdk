using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Net;
using System.Text.RegularExpressions;
using eTerm.AsyncSDK;

namespace ASync.eTermPlugIn {
    [AfterASynCommand("!HelpExpire", IsSystem = true)]
    public sealed class SDKExpire : BaseASyncPlugIn {
        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="SESSION">会话端.</param>
        /// <param name="InPacket">入口数据包.</param>
        /// <param name="OutPacket">出口数据包.</param>
        /// <param name="Key">The key.</param>
        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            StringBuilder sb = new StringBuilder("指令发送成功");
            //eTerm363Session ClientSession = SESSION;
            MatchCollection regResult = Regex.Matches(Encoding.GetEncoding("gb2312").GetString(SESSION.UnOutPakcet(InPacket)).Trim(), @"(\d{4}\-\d{1,2}\-\d{1,2})", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            if (regResult.Count == 1) {
                LicenceManager.Instance.LicenceBody.ExpireDate = DateTime.Parse(regResult[0].Groups[0].Value);
                LicenceManager.Instance.LicenceBody.RemainingMinutes = ((TimeSpan)(LicenceManager.Instance.LicenceBody.ExpireDate - DateTime.Now)).TotalMinutes;
                AsyncStackNet.Instance.BeginRateUpdate(new AsyncCallback(delegate(IAsyncResult iar)
                {
                    AsyncStackNet.Instance.EndRateUpdate(iar);
                }));
            }else
                sb = new StringBuilder("指令格式错误");
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
            return SESSION != null;
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public override string Description {
            get {
                return "主机时长获取";
            }
        }
    }
}

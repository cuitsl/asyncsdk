using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.ASynClientSDK.Base;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// PNR短信通知
    /// </summary>
    public sealed class SmsCommand : ASynCommand {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmsCommand"/> class.
        /// </summary>
        public SmsCommand() : base() { }

        /// <summary>
        /// 提交短信请求.
        /// </summary>
        /// <param name="PnrCode">PNR编号.</param>
        /// <param name="isCn">是否中文短信内容.</param>
        /// <returns></returns>
        public ASyncResult Commit(string PnrCode,bool isCn) {
            base.Connect();
            ASyncResult result = GetSyncResult(string.Format(@"SMS:I/{0}{1}", PnrCode, isCn ? "" : "/I"));
            return result;
        }

        /// <summary>
        /// 提交短信请求(默认使用中文内容).
        /// </summary>
        /// <param name="PnrCode">PNR编号.</param>
        /// <returns></returns>
        public ASyncResult Commit(string PnrCode) {
            return Commit(PnrCode, true);
        }

        /// <summary>
        /// 指令结果解析适配器.
        /// </summary>
        /// <param name="Msg">指令结果集合.</param>
        /// <returns></returns>
        protected override ASyncResult ResultAdapter(string Msg) {
            throw new NotImplementedException();
        }
    }
}

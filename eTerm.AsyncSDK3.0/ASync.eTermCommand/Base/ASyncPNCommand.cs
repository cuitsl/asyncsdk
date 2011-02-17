using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ASync.eTermCommand.Base {
    /// <summary>
    /// 分页指令基类
    /// </summary>
    public abstract class ASyncPNCommand:ASynCommand {

        /// <summary>
        /// 下页指令.
        /// </summary>
        /// <value>The pn command.</value>
        protected virtual string PnCommand { get { return "PN"; } }

        /// <summary>
        /// 是否还有下页数据（将自动执行“PnCommand”）.
        /// </summary>
        /// <param name="msgBody">当前指令结果.</param>
        /// <returns></returns>
        /// <value><c>true</c> if [exist next page]; otherwise, <c>false</c>.</value>
        protected virtual bool ExistNextPage(string msgBody) {
            return false;
        }

        /// <summary>
        /// 最大分页次数(避免无限分页).
        /// </summary>
        /// <value>The max pn count.</value>
        protected virtual int? MaxPnCount { get; private set; }

        /// <summary>
        /// 生成指令并发送分析(子类必须重写).
        /// </summary>
        /// <param name="SynCmd">eTerm实质指令.</param>
        /// <returns></returns>
        protected override ASyncResult GetSyncResult(string SynCmd) {
            //Connect();
            StringBuilder pnResult = new StringBuilder();
            SendStream(SynCmd);
            string PnResult = ConvertResult(GetStream());
            pnResult.Append(PnResult);
            int maxPnCount = this.MaxPnCount??10;
            while (ExistNextPage(PnResult) && --maxPnCount>0) {
                Thread.Sleep(int.Parse((this.AsnCommandSleep??0.5 * 1000).ToString()));
                SendStream(PnCommand);
                PnResult = ConvertResult(GetStream());
                pnResult.AppendFormat("\0{0}", PnResult);
            }
            Dispose();
            ASyncResult Restult = ResultAdapter(pnResult.ToString());
            Restult.ASynCmd = SynCmd;
            return Restult;
        }
    }
}

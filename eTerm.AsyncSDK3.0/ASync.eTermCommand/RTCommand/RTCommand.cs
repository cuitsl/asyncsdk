using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ASync.eTermCommand.Base;
using ASync.eTermCommand;

namespace ASync.eTermCommand {
    /// <summary>
    /// 获取 pnr编号 对应的pnr信息
    /// <code>
    /// RTCommand Rt = new RTCommand();
    /// SyncResult T = Rt.retrieve("QDT6X");
    /// </code>
    /// <example>
    /// RTCommand Rt = new RTCommand();
    /// SyncResult T = Rt.retrieve("QDT6X");
    /// </example>
    /// </summary>
    internal class RTCommand : ASyncPNCommand {
        #region 定义变量
        private string __pnr = string.Empty;
        #endregion

        #region 构造函数
        /// <summary>
        /// 使用定义配置项构造连接.
        /// </summary>
        /// <param name="address">服务器地址.</param>
        /// <param name="port">服务器端口.</param>
        /// <param name="userName">授权用户名.</param>
        /// <param name="userPass">授权用户密码.</param>
        /// <param name="groupCode">授权用户分组.</param>
        public RTCommand(string address, int port, string userName, string userPass, string groupCode) {

        }

        /// <summary>
        /// 使用配置文件配置项构造连接.
        /// </summary>
        public RTCommand() : base() { }
        #endregion

        #region 重写
        /// <summary>
        /// 指令结果解析适配器.
        /// </summary>
        /// <param name="Msg">指令结果集合.</param>
        /// <returns></returns>
        protected override ASyncResult ResultAdapter(string Msg) {
            Msg = string.Join("\r", Msg.Split(new char[]{'\0'}, StringSplitOptions.RemoveEmptyEntries));
            RTResult Rt = new RTResult();
            ParseContract(Rt, Msg);
            ParseGroup(Rt, Msg);
            ParsePassager(Rt, Msg);
            ParseSeg(Rt, Msg);
            return Rt;
        }

        /// <summary>
        /// 是否还有下页数据（将自动执行“PnCommand”）.
        /// </summary>
        /// <param name="msgBody">当前指令结果.</param>
        /// <returns></returns>
        /// <value><c>true</c> if [exist next page]; otherwise, <c>false</c>.</value>
        protected override bool ExistNextPage(string msgBody) {
            return Regex.IsMatch(msgBody.Trim(), @"\+$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        /// <summary>
        /// 异常抛出.
        /// </summary>
        /// <param name="Msg"></param>
        protected override void ThrowExeption(string Msg) {
            //if (!base.Cmd.EndsWith("ig", StringComparison.OrdinalIgnoreCase)&& Regex.IsMatch(Msg, @"NO\s*PNR", RegexOptions.Multiline | RegexOptions.IgnoreCase))
            //    throw new RTNoPnrExeption();
            //if (Regex.IsMatch(Msg, @"THIS\s+PNR\s+WAS\s+ENTIRELY\s+CANCELLED", RegexOptions.Multiline | RegexOptions.IgnoreCase))
            //    throw new RTPNRCancelledException();
        }
        #endregion

        #region 指令主体
        /// <summary>
        /// 获取 pnr编号 对应的pnr信息.
        /// </summary>
        /// <param name="pnrno">pnr编号.</param>
        /// <returns>pnr信息解析存储,若对应pnr编号无相应信息或已经删除则返回null</returns>
        public ASyncResult retrieve(string pnrno) {
            __pnr = pnrno;
            return base.GetSyncResult(string.Format(@"RT:{0}", pnrno));
        }
        #endregion

        #region 分析旅客组
        /// <summary>
        /// Parses the passager.
        /// </summary>
        /// <param name="Rt">The rt.</param>
        /// <param name="Msg">The MSG.</param>
        private void ParsePassager(RTResult Rt, string Msg) {
            Rt.getPassengers =new List<PNRPassengerResult>();
            foreach (Match m in Regex.Matches(Msg.Replace("\0", "\r"), @"(\d+\.(([A-Z]+\/[A-Z]+)|([\u4e00-\u9fa5]+))\s*(NI)?\s*)+" + this.__pnr, RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                foreach (Capture c in m.Groups[2].Captures) {
                    Rt.getPassengers.Add(new PNRPassengerResult(c.Value));
                }
            }
        }
        #endregion

        #region 分析航段组
        /// <summary>
        /// Parses the seg.
        /// </summary>
        /// <param name="Rt">The rt.</param>
        /// <param name="Msg">The MSG.</param>
        private void ParseSeg(RTResult Rt, string Msg) {
            Rt.getAirSegs = new List<PNRAirSegResult>();
            foreach (Match m in Regex.Matches(Msg.Replace("\0", "\r"), @"\d+\.\s+(\w{2}\d+)\s+([A-Z])\s+([A-Z]{2}\d{2}[A-Z]{3})\s+([A-Z]{3})([A-Z]{3})\s+(\w{3})\s+(\d{4})\s+(\d{4})\s+", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                PNRAirSegResult seg = new PNRAirSegResult();
                seg.getActionCode = m.Groups[6].Value;
                seg.getAirNo = m.Groups[1].Value;
                seg.getArrivalTime = m.Groups[8].Value;
                seg.getDepartureTime = m.Groups[7].Value;
                seg.getDesCity = m.Groups[5].Value;
                seg.getFltClass = m.Groups[2].Value;
                seg.getOrgCity = m.Groups[4].Value;
                Rt.getAirSegs.Add(seg);
            }
        }
        #endregion

        #region 联系组
        /// <summary>
        /// Parses the contract.
        /// </summary>
        /// <param name="Rt">The rt.</param>
        /// <param name="Msg">The MSG.</param>
        private void ParseContract(RTResult Rt, string Msg) {

        }
        #endregion

        #region 分析团队标识组
        /// <summary>
        /// Parses the group.
        /// </summary>
        /// <param name="Rt">The rt.</param>
        /// <param name="Msg">The MSG.</param>
        private void ParseGroup(RTResult Rt, string Msg) {

        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using eTerm.ASynClientSDK.Base;
using eTerm.ASynClientSDK;
using eTerm.ASynClientSDK.Utils;

namespace eTerm.ASynClientSDK {
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
    public class RTCommand : ASyncPNCommand {
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

            PNRInfo PnrInfoResult= new AnalysisPRN(__pnr, Msg).ParsePNR();
            RTResult Rt = new RTResult() { getAirSegs=new List<PNRAirSegResult>(), getPassengers=new List<PNRPassengerResult>() };
            foreach (Passenger p in PnrInfoResult.PassengerList) {
                Rt.getPassengers.Add(new PNRPassengerResult() { 
                 getName=p.FullName,
                  IdentityNo=p.DocumentNo
                });
            }
            foreach (Segment seg in PnrInfoResult.SegmentList) {
                Rt.getAirSegs.Add(new PNRAirSegResult() { 
                 getAirNo=string.Format(@"{0}{1}",seg.Airline,seg.FltNo),
                  getDepartureTime=ConvertUtil.InitialsDateCast( string.Format(@"{1} {0}",seg.DepartureTime.Insert(2,":"),seg.Date)),
                 getArrivalTime = ConvertUtil.InitialsDateCast(string.Format(@"{1} {0}", seg.ArrivalTime.Insert(2,":"), seg.Date)),
                     getOrgCity=seg.DepartureAirport,
                      getDesCity=seg.ArrivalAirport,
                       getActionCode=seg.Ticket,
                        getFltClass=seg.Carbin,
                });
            }
            Rt.IsCancel = PnrInfoResult.Cancel;
            Rt.TKTL = PnrInfoResult.TKTL;
            //PnrInfoResult.
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
            RTResult Result=base.GetSyncResult(string.Format(@"RT:{0}", pnrno)) as RTResult;
            Result.PnrCode = pnrno;
            return Result;
        }
        #endregion
    }
}

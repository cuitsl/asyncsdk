using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Net;
using System.Text.RegularExpressions;
using System.Net;

namespace ASync.eTermCommand {
    [AfterASynCommand(@"!SMSPNR")]
    public sealed class SMSPnr : BaseASyncPlugIn {
        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="SESSION">会话端.</param>
        /// <param name="InPacket">入口数据包.</param>
        /// <param name="OutPacket">出口数据包.</param>
        /// <param name="Key">The key.</param>
        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            Match PnrMatch = Regex.Match(Encoding.GetEncoding("gb2312").GetString(SESSION.UnInPakcet(InPacket)).Trim(), @"^!SMSPNR\s+([A-Z0-9]{6,6})\s+(\d+)", RegexOptions.Multiline| RegexOptions.IgnoreCase);
            string Pnr = string.Empty;
            string Mobile = string.Empty;
            if (PnrMatch == null) {
                SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, "指令格式错误!"));
                return;
            }
            Pnr = PnrMatch.Groups[1].Value;
            Mobile = PnrMatch.Groups[2].Value;
            RTCommand Rt = new RTCommand();
            IPEndPoint ServerEP = SESSION.AsyncSocket.LocalEndPoint as IPEndPoint;
            Rt.Connect(ServerEP.Address.ToString(), ServerEP.Port, SESSION.userName, SESSION.userPass);
            if (SESSION.Async443 != null)
                SESSION.Async443.TSession = null;
            RTResult Result=Rt.retrieve(Pnr) as RTResult;
            StringBuilder sb = new StringBuilder();
            foreach (PNRAirSegResult Seg in Result.getAirSegs)
                sb.AppendFormat("航段:{0} {1}-{2} {3} {4}\r", Seg.getAirNo, Seg.getOrgCity, Seg.getDesCity, Seg.getDepartureTime, Seg.getArrivalTime);
            foreach (PNRPassengerResult Inf in Result.getPassengers)
                sb.AppendFormat("旅客:{0}\r", Inf.getName);            
            sb.AppendFormat("{0}  Pnr信息已经发送至:{1}",Pnr, Mobile);
            SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID,
                    sb.ToString()
                ));
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
            return Key.AllowIntercept;
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public override string Description {
            get {
                return "PNR信息短信发送";
            }
        }
    }
}

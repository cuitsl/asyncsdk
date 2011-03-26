using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.ASynClientSDK.Base;
using eTerm.ASynClientSDK;
using System.Text.RegularExpressions;
using eTerm.ASynClientSDK.AVException;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// AV指令对象
    /// <remarks>
    /// 提供简单有效的航班信息实时查询通道 查询指定日期及航线上的可用航班信息支持多种查询参数 结果数据以类AvResult的形式表示
    /// </remarks>
    /// <code>
    /// AVCommand Av = new AVCommand();
    /// SyncResult r = Av.getAvailability("SHA", "CTU", DateTime.Now.AddMonths(1),string.Empty,true);
    /// </code>
    /// <example>
    /// AVCommand Av = new AVCommand();
    /// SyncResult r = Av.getAvailability("SHA", "CTU", DateTime.Now.AddMonths(1),string.Empty,true);
    /// </example>
    /// </summary>
    public sealed class AVCommand : ASyncPNCommand {

        #region 变量定义
        private string queryDate = string.Empty;
        #endregion


        #region 重写
        /// <summary>
        /// 是否还有下页数据（将自动执行“PnCommand”）.
        /// </summary>
        /// <param name="msgBody">当前指令结果.</param>
        /// <returns></returns>
        /// <value><c>true</c> if [exist next page]; otherwise, <c>false</c>.</value>
        protected override bool ExistNextPage(string msgBody) {
            return Regex.IsMatch(msgBody, @"7\+");
        }

        /// <summary>
        /// 异常抛出.
        /// </summary>
        /// <param name="Msg"></param>
        protected override void ThrowExeption(string Msg) {
            if (Regex.IsMatch(Msg, @"\*NO\sROUTING\*", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new AVNoRoutingException();
            if (Regex.IsMatch(Msg, @"CITY\sPAIR", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new AVCityPairException();
            if (Regex.IsMatch(Msg, @"\*CITY\*", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new AVCityPairException();
        }

        /// <summary>
        /// 生成指令并发送分析(子类必须重写).
        /// </summary>
        /// <param name="SynCmd">eTerm实质指令.</param>
        /// <returns></returns>
        protected override ASyncResult GetSyncResult(string SynCmd) {
            return base.GetSyncResult(SynCmd);
        }

        /// <summary>
        /// 指令结果解析适配器.
        /// </summary>
        /// <param name="Msg">指令结果集合.</param>
        /// <returns></returns>
        protected override ASyncResult ResultAdapter(string Msg) {
            AVResult result = SyncParseAVH(Msg) as AVResult;
            foreach (AvItem item in result) {
                item.getCabins.Reverse();
            }
            return result;
        }
        #endregion

        #region 结果分析
        /// <summary>
        /// Syncs the parse AVH.
        /// </summary>
        /// <param name="szResult">The sz result.</param>
        /// <returns></returns>
        private ASyncResult SyncParseAVH(string szResult) {
            AVResult avResult = new AVResult();
            //有效的分页数据
            foreach (string s in szResult.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries)) {
                if (!Regex.IsMatch(s, queryDate + @"(\d{2})?\("))
                    continue;
                foreach (string avItemStr in getAvItemStringArray(s)) {
                    AvItem Item = new AvItem();
                    try {
                        Item = getAvItem(avItemStr);
                    }
                    catch { continue; }
                    if (avResult.AvSegment.Contains(Item) || string.IsNullOrEmpty(Item.getAirline))
                        continue;
                    if (avResult.AvSegment.Contains(Item))
                        continue;
                    avResult.AvSegment.Add(Item);
                }
            }
            return avResult;
        }

        /// <summary>
        /// Gets the av item.
        /// </summary>
        /// <param name="itemString">The item string.</param>
        /// <returns></returns>
        private List<string> getAvItemStringArray(string itemString) {
            List<string> items = new List<string>();
            MatchCollection matchs = new Regex(@"(^\d(.{1,}))|(^>(.{1,}))", RegexOptions.Multiline | RegexOptions.IgnoreCase).Matches(itemString.Replace("\r", "\r\n")); //Regex.Matches(itemString, @"(^\d(.{1,}))|(^>(.{1,}))");
            for (int i = 0; i < matchs.Count; i++) {
                try {
                    items.Add(string.Format("{0}{1}", matchs[i].Value.PadRight(80, ' '), matchs[++i].Value.PadRight(80, ' ')));
                }
                catch { }
            }
            return items;
        }

        /// <summary>
        /// Gets the av item.
        /// </summary>
        /// <param name="itemString">The item string.</param>
        /// <returns></returns>
        private AvItem getAvItem(string itemString) {
            AvItem avItem = new AvItem();
            avItem.getAirline = Regex.Match(itemString, @"(\s|\*)([A-Z]|\d)[A-Z]\d{1,}").Value;
            avItem.isCodeShare = avItem.getAirline.StartsWith("*");
            avItem.getAirline = avItem.getAirline.Substring(1);
            avItem.getCarrier = avItem.getAirline.Substring(0, 2);
            avItem.getDeptime = Regex.Matches(itemString, @"\s\d{4}\+?\d?\s")[0].Value.Trim();
            avItem.getArritime = Regex.Matches(itemString, @"\s\d{4}\+?\d?\s")[1].Value.Trim().Substring(0, 4);
            avItem.getOrgcity = Regex.Match(itemString, @"\s[A-Z]{6}\s").Value.Trim().Substring(0, 3);
            avItem.getDstcity = Regex.Match(itemString, @"\s[A-Z]{6}\s").Value.Trim().Substring(3);
            avItem.getLink = Regex.Match(itemString, @"\s[A-Z]{2}\#").Value.Trim();
            avItem.getDepdate = queryDate;
            avItem.getPlanestyle = Regex.Match(itemString, @"\s\d{3}\s").Value.Trim();
            if (Regex.Matches(itemString, @"\s\d{4}\+?\d?\s")[1].Value.Trim().IndexOf("+") > 0) {
                avItem.getArridate = (int.Parse(Regex.Match(queryDate, @"\d{1,}").Value) + 1).ToString("D2") + Regex.Match(queryDate, "[A-Z]{1,}").Value;
            }
            else
                avItem.getArridate = queryDate;
            avItem.getCabins = new List<AvItemCabinChar>();
            foreach (Match m in Regex.Matches(itemString.Substring(
                Regex.Match(itemString, @"\s[A-Z]{2}\#").Index,
                Regex.Matches(itemString, @"\s{5,}")[1].Index - Regex.Match(itemString, @"\s[A-Z]{2}\#").Index), @"[A-Z](\d|A)\s", RegexOptions.Multiline | RegexOptions.IgnoreCase)) {
                avItem.getCabins.Add(new AvItemCabinChar(m.Value[0], m.Value[1]));
            }
            return avItem;
        }

        #endregion


        #region 查询方法
        /// <summary>
        ///  查询指定日期和城市间的实时航班信息,包含转飞航班.
        /// </summary>
        /// <param name="org">起飞城市三字代码.</param>
        /// <param name="dst">到达城市三字代码.</param>
        /// <param name="date">查询日期.</param>
        /// <param name="airline">航空公司.</param>
        /// <returns></returns>
        public ASyncResult getAvailability(string org, string dst, DateTime date, string airline) {
            return this.getAvailability(org, dst, string.Format(@"{0}{1}", date.Day.ToString("D2"), getDateString(date)), airline, true, true);
        }

        /// <summary>
        /// 查询指定日期和城市间的实时航班信息,包含转飞航班.
        /// </summary>
        /// <param name="org">起飞城市三字代码.</param>
        /// <param name="dst">到达城市三字代码.</param>
        /// <param name="date">查询日期.</param>
        /// <returns></returns>
        public ASyncResult getAvailability(string org, string dst, DateTime date) {
            return this.getAvailability(org, dst, string.Format(@"{0}{1}", date.Day.ToString("D2"), getDateString(date)), string.Empty, true, true);
        }

        /// <summary>
        /// 查询指定日期和城市间的实时航班信息.
        /// </summary>
        /// <param name="org">起飞城市三字代码.</param>
        /// <param name="dst">到达城市三字代码.</param>
        /// <param name="date">查询日期.</param>
        /// <param name="airline">航空公司.</param>
        /// <param name="direct">是否只查询直达航班.</param>
        /// <returns></returns>
        public ASyncResult getAvailability(string org, string dst, DateTime date, string airline, bool direct) {
            return this.getAvailability(org, dst, string.Format(@"{0}{1}", date.Day.ToString("D2"), getDateString(date)), airline, direct, true);
        }

        /// <summary>
        ///   查询指定日期和城市间的实时航班信息.
        /// </summary>
        /// <param name="org">起飞城市三字代码.</param>
        /// <param name="dst">到达城市三字代码.</param>
        /// <param name="date">查询日期.</param>
        /// <param name="airline">航空公司.</param>
        /// <param name="direct">是否只查询直达航班.</param>
        /// <param name="e_ticket">是否只查询支持电子客户票航班.</param>
        /// <returns></returns>
        public ASyncResult getAvailability(string org, string dst, DateTime date, string airline, bool direct, bool e_ticket) {
            return this.getAvailability(org, dst, string.Format(@"{0}{1}", date.Day.ToString("D2"), getDateString(date)), airline, direct, e_ticket);
        }


        /// <summary>
        /// 查询指定航班号和日期的航班信息.
        /// </summary>
        /// <param name="airline">航空公司.</param>
        /// <param name="date">查询日期.</param>
        /// <returns></returns>
        //public SyncResult getAvailability(string airline, string date) {
        //    return this.getAvailability(string.Empty,string.Empty, date, airline, false, true);
        //}

        /// <summary>
        ///  查询指定日期和城市间的实时航班信息,包含转飞航班.
        /// </summary>
        /// <param name="org">起飞城市三字代码.</param>
        /// <param name="dst">到达城市三字代码.</param>
        /// <param name="date">查询日期(10JUL).</param>
        /// <param name="airline">航空公司.</param>
        /// <returns></returns>
        public ASyncResult getAvailability(string org, string dst, string date, string airline) {
            return this.getAvailability(org, dst, date, airline, true, true);
        }

        /// <summary>
        /// 查询指定日期和城市间的实时航班信息.
        /// </summary>
        /// <param name="org">起飞城市三字代码.</param>
        /// <param name="dst">到达城市三字代码.</param>
        /// <param name="date">查询日期(10JUL).</param>
        /// <param name="airline">航空公司.</param>
        /// <param name="direct">是否只查询直达航班.</param>
        /// <returns></returns>
        public ASyncResult getAvailability(string org, string dst, string date, string airline, bool direct) {
            return this.getAvailability(org, dst, date, airline, direct, true);
        }

        /// <summary>
        ///   查询指定日期和城市间的实时航班信息.
        /// </summary>
        /// <param name="org">起飞城市三字代码.</param>
        /// <param name="dst">到达城市三字代码.</param>
        /// <param name="date">查询日期(10JUL).</param>
        /// <param name="airline">航空公司.</param>
        /// <param name="direct">是否只查询直达航班.</param>
        /// <param name="e_ticket">是否只查询支持电子客户票航班.</param>
        /// <returns></returns>
        public ASyncResult getAvailability(string org, string dst, string date, string airline, bool direct, bool e_ticket) {
            string avCommand = string.Format(@"AV:H/{0}{1}/{2}{3}{4}{5}"
                , org
                , dst
                , date
                , string.IsNullOrEmpty(airline) ? string.Empty : "/" + airline
                , direct ? "/D" : string.Empty
                , e_ticket ? "" : ""
                );
            this.queryDate = Regex.Match(avCommand, @"\d{2,2}[A-Z]{3,3}", RegexOptions.Singleline).Value;
            return base.GetSyncResult(avCommand);
        }
        #endregion

    }
}

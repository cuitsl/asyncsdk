using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using eTerm.ASynClientSDK.Base;
using eTerm.ASynClientSDK;
using eTerm.ASynClientSDK.FDException;
namespace eTerm.ASynClientSDK {
    /// <summary>
    /// FD指令
    /// </summary>
    public class FDCommand:ASyncPNCommand {

        #region 构造函数
        /// <summary>
        /// 使用定义配置项构造连接.
        /// </summary>
        public FDCommand() {

        }
        #endregion

        #region 重写
        /// <summary>
        /// 指令结果解析适配器.
        /// </summary>
        /// <param name="Msg">指令结果集合.</param>
        /// <returns></returns>
        protected override ASyncResult ResultAdapter(string Msg) {
            FDResult Fd = new FDResult();
            ParseFd(Fd, Msg);
            return Fd;
        }

        /// <summary>
        /// 结果错误检查及异常抛出.
        /// </summary>
        /// <param name="Msg"></param>
        protected override void ThrowExeption(string Msg) {
            if (Regex.IsMatch(Msg, @"[\u4E00-\u9FA5]{1,}", RegexOptions.IgnoreCase))
                throw new FDCityPairException();
        }

        /// <summary>
        /// 是否还有下页数据（将自动执行“PnCommand”）.
        /// </summary>
        /// <param name="msgBody">当前指令结果.</param>
        /// <returns></returns>
        /// <value><c>true</c> if [exist next page]; otherwise, <c>false</c>.</value>
        protected override bool ExistNextPage(string msgBody) {
            Match m = Regex.Match(msgBody, @"PAGE\s+(\d+)\/(\d+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (string.IsNullOrEmpty(m.Groups[1].Value.Trim()) || string.IsNullOrEmpty(m.Groups[2].Value.Trim()))
                return true;
            return int.Parse(m.Groups[1].Value) != int.Parse(m.Groups[2].Value);
        }
        #endregion

        #region 指令主体
        /// <summary>
        /// Finds the price.
        /// </summary>
        /// <param name="org">The org.</param>
        /// <param name="dst">The DST.</param>
        /// <param name="date">The date.</param>
        /// <param name="airline">The airline.</param>
        /// <param name="planeModel">The plane model.</param>
        /// <param name="passType">Type of the pass.</param>
        /// <returns></returns>
        public ASyncResult findPrice(string org, string dst,DateTime? date, string airline, string planeModel, string passType) {
            return base.GetSyncResult(
                string.Format(@"FD:{0}{1}{2}{3}",
                org,
                dst,
                date==null?string.Empty:string.Format("/{0}{1}{2}" ,date.Value.Day.ToString("D2"),getDateString(date.Value),date.Value.ToString("yy")),
                string.IsNullOrEmpty(airline)?"":"/"+airline,
                planeModel, passType));
        }
        #endregion

        #region 结查分析
        /// <summary>
        /// Parses the fd.
        /// </summary>
        /// <param name="Fd">The fd.</param>
        /// <param name="Msg">The MSG.</param>
        private void ParseFd(FDResult Fd, string Msg) {
            Fd.getElements = new List<FDItem>();
            float CabinY = Regex.IsMatch(Msg, @"\s\w{2}\/[Y]\s*\/\s*(\d*\.?\d{0,2})\=\s*(\d*\.?\d{0,2})", RegexOptions.IgnoreCase) ? float.Parse(Regex.Match(Msg, @"\s\w{2}\/[Y]\s*\/\s*(\d*\.?\d{0,2})\=\s*(\d*\.?\d{0,2})", RegexOptions.IgnoreCase).Groups[1].Value) : 0.0f;
            Fd.getMoneyType = Regex.Match(Msg, @"\/\w{2}\s*\/([A-Z]{3})", RegexOptions.IgnoreCase).Groups[1].Value;
            Fd.getOrg = Regex.Match(Msg, @"FD\:([A-Z]{3})([A-Z]{3})\/", RegexOptions.IgnoreCase).Groups[1].Value;
            Fd.getDst = Regex.Match(Msg, @"FD\:([A-Z]{3})([A-Z]{3})\/", RegexOptions.IgnoreCase).Groups[2].Value;
            Fd.getDistance = Regex.Match(Msg, @"\/TPM\s*(\d{1,})\s*\/", RegexOptions.IgnoreCase).Groups[1].Value;
            Fd.getBaseFare = CabinY;
            foreach (string s in Msg.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries)) {
                if (Regex.IsMatch(s, @"^\d{1,}", RegexOptions.IgnoreCase | RegexOptions.Multiline)){
                    try {
                        FDItem Item = ParseItem(s,CabinY,Msg);
                        if (Fd.getElements.Contains(Item))
                            continue;
                        Fd.getElements.Add(Item);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Parses the item.
        /// </summary>
        /// <param name="MsgItem">The MSG item.</param>
        /// <returns></returns>
        public FDItem ParseItem(string MsgItem,double CabinY,string Msg) {
            FDItem Item = new FDItem();
            Item.getAirline = Regex.Match(MsgItem, @"^\d+\s(\w{2})", RegexOptions.IgnoreCase).Groups[1].Value;
            Item.getCabinType = Regex.Match(MsgItem, @"\d{2,2}\s+([A-Z0-9]{2,2})\/([A-Z]\d?)\s+\/[\s\d\=\.]*\/([A-Z])\/([A-Z])\/", RegexOptions.IgnoreCase).Groups[2].Value;
            //查找Y航价格{CA/Y}
            //Match CabinMatch = Regex.Match(MsgItem, @"\s*\/([A-Z])\/([A-Z])\/", RegexOptions.IgnoreCase);
            //Item.getDiscountRate = (double.Parse(CabinMatch.Groups[1].Value) / CabinY).ToString("f2");
            Item.getBasicCabinType = Regex.Match(MsgItem, @"\d{2,2}\s+([A-Z0-9]{2,2})\/([A-Z]\d?)\s+\/[\s\d\=\.]*\/([A-Z])\/([A-Z])\/", RegexOptions.IgnoreCase).Groups[4].Value;
            if (!Regex.IsMatch(MsgItem, @"(\d*\.?\d{0,2})\=\s*(\d*\.?\d{0,2})", RegexOptions.IgnoreCase)) {
                //Match m = Regex.Match(MsgItem, @"^\d{1,}\s*("+Item.getAirline+@")\s*\/(" + Item.getCabinType + @")OW\s*\/\s*(\d*\.?\d{0,2})\s*\/([A-Z])\/([A-Z])\/");
                Item.getSinglePrice = Regex.Match(Msg, @"\d{1,}\s*(" + Item.getAirline + @")\s*\/(" + Item.getCabinType + @")OW\s*\/\s*(\d*\.?\d{0,2})\s*\/([A-Z])\/([A-Z])\/").Groups[3].Value;
                Item.getRoundPrice = Regex.Match(Msg, @"\d{1,}\s*(" + Item.getAirline + @")\s*\/(" + Item.getCabinType + @")RT\s*\/\s*(\d*\.?\d{0,2})\s*\/([A-Z])\/([A-Z])\/").Groups[3].Value;
            }
            else {
                Match m = Regex.Match(MsgItem, @"(\d*\.?\d{0,2})\=\s*(\d*\.?\d{0,2})");
                Item.getSinglePrice = m.Groups[1].Value;
                Item.getRoundPrice = m.Groups[2].Value;
            }
            /*
            if(Regex.IsMatch(MsgItem, @"(\d*\.?\d{0,2})\=\s*(\d*\.?\d{0,2})", RegexOptions.IgnoreCase))
                Item.getSinglePrice = Regex.Match(MsgItem, @"(\d*\.?\d{0,2})\=\s*(\d*\.?\d{0,2})", RegexOptions.IgnoreCase).Groups[1].Value;
            else
                Item.getSinglePrice = Regex.Match(MsgItem, @"(\d{1,}\.\d{1,2})(\s{1,})\/", RegexOptions.IgnoreCase).Groups[1].Value;
            if (Regex.IsMatch(MsgItem, @"(\d*\.?\d{0,2})\=\s*(\d*\.?\d{0,2})", RegexOptions.IgnoreCase))
                Item.getRoundPrice = Regex.Match(MsgItem, @"(\d*\.?\d{0,2})\=\s*(\d*\.?\d{0,2})", RegexOptions.IgnoreCase).Groups[2].Value;
            else
                Item.getRoundPrice = Regex.Match(MsgItem, @"(\d*\.?\d{0,2})\=\s*(\d*\.?\d{0,2})", RegexOptions.IgnoreCase).Groups[2].Value;
            */
            Item.getStartDate = Regex.Match(MsgItem, @"\.\s*\/(\d{2}[A-Z]{3}\d{2})\s*", RegexOptions.IgnoreCase).Groups[1].Value;
            return Item;
        }
        #endregion
    }
}

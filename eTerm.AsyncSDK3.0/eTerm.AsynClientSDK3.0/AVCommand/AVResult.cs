using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// AV指令结果实体
    /// </summary>
    public class AVResult:ASyncResult {
        #region 属性定义
        private List<AvItem> __items = new List<AvItem>();
        /// <summary>
        /// 航班集合.
        /// </summary>
        /// <value>The av segment.</value>
        public List<AvItem> AvSegment { get { return this.__items; } set { this.__items = value; } }

        /// <summary>
        /// 得到Date类型的日期.
        /// </summary>
        /// <value>The depart date.</value>
        public string getDate { get; set; }

        /// <summary>
        /// 得到日期信息中的星期, 返回的值为星期的缩写形式.
        /// </summary>
        /// <value>The get day.</value>
        //public string getDay { get { return this.getDate.DayOfWeek.ToString().ToUpper().Substring(0, 3); } }

        /// <summary>
        /// 得到日期信息中的月, 返回的值为月份的英文缩写形式, 从一月到十二月为: JAN,FEB,MAR,APR,MAY,JUN,JUL,AUG,SEP,OCT,NOV,DEC.
        /// </summary>
        /// <value>The get month.</value>
        //public string getMonth { get {
        //        MatchCollection regx = new Regex(@"[A-Z]{3,3}").Matches(this.getDate.ToString("R").ToUpper());
        //        return regx[1].Value;
        //} }

        /// <summary>
        /// 得到目的城市三字代码.
        /// </summary>
        /// <value>The get DST.</value>
        public string getDst { get; set; }

        /// <summary>
        /// 得到起始城市三字代码.
        /// </summary>
        /// <value>The get org.</value>
        public string getOrg { get; set; }


        /// <summary>
        /// 得到查询日期的年, 如"01","02".......
        /// </summary>
        /// <value>The get year.</value>
        //public string getYear { get { return this.getDate.Year.ToString().Substring(2); } }

        /// <summary>
        /// 到日期信息中的日, 返回的值为从"01"到"31"的字符串.
        /// </summary>
        /// <value>The get dt.</value>
        //public string getDt { get { return this.getDate.Day.ToString("D2"); } }

        /// <summary>
        /// 读取指定位置的AvItem对象.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public AvItem getItem(int index) {
            if (index >= this.AvSegment.Count)
                throw new NotFiniteNumberException("指令序号不存在，请检查！");
            return this.AvSegment[index];
        }

        /// <summary>
        /// 从AvItem列表中删除指定位置的元素.
        /// </summary>
        /// <param name="index">The index.</param>
        public void removeItemAt(int index) {
            if (index >= this.AvSegment.Count)
                throw new NotFiniteNumberException("指令序号不存在，请检查！");
            this.AvSegment.RemoveAt(index);
        }

        /// <summary>
        /// 得到航班信息记录的数量.
        /// </summary>
        /// <value>The get itemamount.</value>
        [XmlIgnore]
        public int getItemamount { get { return this.AvSegment.Count; } }
        #endregion
    }
}

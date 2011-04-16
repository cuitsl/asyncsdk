using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using eTerm.ASynClientSDK;
namespace eTerm.ASynClientSDK {
    /// <summary>
    /// RT指令结果结构
    /// </summary>
    public class RTResult:ASyncResult {
        /// <summary>
        /// PNR航段集合.
        /// </summary>
        /// <value>The get air seg at.</value>
        public List<PNRAirSegResult> getAirSegs { get; set; }

        /// <summary>
        /// 记录编号.
        /// </summary>
        /// <value>The PNR code.</value>
        public string PnrCode { get; set; }

        /// <summary>
        /// 获取所有联系组 .
        /// </summary>
        /// <value>The get contacts.</value>
        public List<PNRContactResult> getContacts { get; set; }

        /// <summary>
        /// 获取联系组数目.
        /// </summary>
        /// <value>The get contacts count.</value>
        [XmlIgnore]
        public int getContactsCount { get { return this.getContacts.Count; } }

        /// <summary>
        /// 判断PNR是否为团体票
        /// </summary>
        /// <value><c>true</c> if this instance is group; otherwise, <c>false</c>.</value>
        public bool isGroup { get; set; }

        /// <summary>
        /// 获取团体票名称.
        /// </summary>
        /// <value>The get groupname.</value>
        public string getGroupname { get; set; }

        /// <summary>
        /// 获取团体票总人数.
        /// </summary>
        /// <value>The get group number.</value>
        public int getGroupNumber { get; set; }

        /// <summary>
        /// 出票时限.
        /// </summary>
        /// <value>The tk tl.</value>
        public string TKTL { get; set; }

        /// <summary>
        /// 出票时限.
        /// </summary>
        /// <value>The office code.</value>
        public string OfficeCode { get; set; }

        /// <summary>
        /// 获取所有婴儿组.
        /// </summary>
        /// <value>The get infants.</value>
        public List<PNRInfantResult> getInfants { get; set; }

        /// <summary>
        /// 获取婴儿组数目.
        /// </summary>
        /// <value>The get infants count.</value>
        [XmlIgnore]
        public int getInfantsCount { get { return this.getInfants.Count; } }

        /// <summary>
        /// 获取所有姓名组 .
        /// </summary>
        /// <value>The get passengers.</value>
        public List<PNRPassengerResult> getPassengers { get; set; }

        /// <summary>
        /// 获取PNR中的姓名组数量.
        /// </summary>
        /// <value>The get passenger number.</value>
        [XmlIgnore]
        public int getPassengerNumber { get { return this.getPassengers.Count; } }

        /// <summary>
        /// 获取第n乘客信息.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public PNRPassengerResult getPassengerAt(int n) {
            if (n >= this.getPassengers.Count)
                throw new SdkSequenceException();
            return this.getPassengers[n];
        }

        /// <summary>
        /// 获取第n婴儿组.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public PNRInfantResult getInfantAt(int n) {
            if (n >= this.getInfants.Count)
                throw new SdkSequenceException();
            return this.getInfants[n];
        }

        /// <summary>
        /// 获取第n航段.
        /// </summary>
        /// <param name="n">The index.</param>
        /// <returns></returns>
        public PNRAirSegResult getAirSegAt(int n) {
            if (n >= this.getAirSegs.Count)
                throw new SdkSequenceException();
            return this.getAirSegs[n];
        }

        /// <summary>
        /// 获取航段数目.
        /// </summary>
        /// <value>The get air segs count.</value>
        public int getAirSegsCount { get { return this.getAirSegs.Count; } }
    }
}

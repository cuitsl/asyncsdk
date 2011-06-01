using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;
using System.Runtime.Serialization;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 本类包含一个PNR航段的所有信息。航段有普通、开放、信息三种类别。 对于不同类别的航段，该类中的有效字段个数会有所区别。普通航段的 所有字段都是有定义的；而对于开放航段，起飞到达时间就没有明确的 定义，这是相应的提取函数返回空值或者0值。 
    /// </summary>
    [DataContract]
    public class PNRAirSegResult:ASyncResult {
        /// <summary>
        /// 获取该航段的行动代码.
        /// </summary>
        /// <value>The get action code.</value>
        [DataMember]
        public string getActionCode { get; set; }

        /// <summary>
        /// 获取该航段的航班号.
        /// </summary>
        /// <value>The get air no.</value>
        [DataMember]
        public string getAirNo { get; set; }

        /// <summary>
        /// 获取该航段的到达时间.
        /// </summary>
        /// <value>The get arrival time.</value>
        [DataMember]
        public string getArrivalTime { get; set; }

        /// <summary>
        /// 获取该航段的起飞时间.
        /// </summary>
        /// <value>The get departure time.</value>
        [DataMember]
        public string getDepartureTime { get; set; }

        /// <summary>
        /// 获取该航段的到达城市.
        /// </summary>
        /// <value>The get DES city.</value>
        [DataMember]
        public string getDesCity { get; set; }

        /// <summary>
        /// 获取该航段的舱位等级.
        /// </summary>
        /// <value>The get FLT class.</value>
        [DataMember]
        public string getFltClass { get; set; }

        /// <summary>
        /// 获取该航段的起飞城市.
        /// </summary>
        /// <value>The get org city.</value>
        [DataMember]
        public string getOrgCity { get; set; }

        /// <summary>
        /// pnr中航段原文。 .
        /// </summary>
        /// <value>The get original air seg.</value>
        [DataMember]
        public string getOriginalAirSeg { get; set; }

        /// <summary>
        /// 获取该航段的航程变更标志
        /// </summary>
        /// <value><c>true</c> if [get sk changed]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool getSkChanged { get; set; }

        /// <summary>
        /// 获取该航段的类型.
        /// </summary>
        /// <value><c>true</c> if [get type]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool getType { get; set; }
    }
}

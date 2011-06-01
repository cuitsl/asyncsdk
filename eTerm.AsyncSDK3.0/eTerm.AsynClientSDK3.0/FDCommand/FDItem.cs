using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;
using System.Runtime.Serialization;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 运价条目
    /// </summary>
    [DataContract]
    public class FDItem:ASyncResult {
        /// <summary>
        /// 获取对应纪录的航空公司代码.
        /// </summary>
        /// <value>The get airline.</value>
        [DataMember]
        public string getAirline { get; set; }

        /// <summary>
        ///  获取基础舱位类型 .
        /// </summary>
        /// <value>The type of the get basic cabin.</value>
        [DataMember]
        public string getBasicCabinType { get; set; }

        /// <summary>
        /// 获取舱位描述.
        /// </summary>
        /// <value>The get cabin desc.</value>
        [DataMember]
        public string getCabinDesc { get; set; }

        /// <summary>
        /// 获取舱位类型.
        /// </summary>
        /// <value>The type of the get cabin.</value>
        [DataMember]
        public string getCabinType { get; set; }

        /// <summary>
        /// 获取舱位折扣率.
        /// </summary>
        /// <value>The get discount rate.</value>
        [DataMember]
        public string getDiscountRate { get; set; }

        /// <summary>
        /// 获取生效日期.
        /// </summary>
        /// <value>The get start date.</value>
        [DataMember]
        public string getStartDate { get; set; }

        /// <summary>
        /// 获取截止日期.
        /// </summary>
        /// <value>The get end date.</value>
        [DataMember]
        public string getEndDate { get; set; }

        /// <summary>
        /// 获取单程票价.
        /// </summary>
        /// <value>The get single price.</value>
        [DataMember]
        public string getSinglePrice { get; set; }

        /// <summary>
        /// 获取往返票价.
        /// </summary>
        /// <value>The get round price.</value>
        [DataMember]
        public string getRoundPrice { get; set; }

        /// <summary>
        /// 获取对应纪录的运价规则代码.
        /// </summary>
        /// <value>The get rule.</value>
        [DataMember]
        public string getRule { get; set; }

        #region 重写
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return string.Format(@"{0}-{1}", this.getAirline, this.getCabinType);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }
        #endregion

    }
}

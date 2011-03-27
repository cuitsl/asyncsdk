using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK {

    /// <summary>
    /// 舱位信息
    /// </summary>
    public class AvItemCabinChar:ASyncResult {

        /// <summary>
        /// Initializes a new instance of the <see cref="AvItemCabinChar"/> class.
        /// </summary>
        public AvItemCabinChar() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvItemCabinChar"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="avalibly">The avalibly.</param>
        public AvItemCabinChar(char code, char avalibly) {
            this.getCode = code;
            this.getAvalibly = avalibly;
        }

        /// <summary>
        /// 舱位等级.
        /// </summary>
        /// <value>The get code.</value>
        public char getCode { get; set; }

        /// <summary>
        /// 有效数.
        /// </summary>
        /// <value>The get avalibly.</value>
        public char getAvalibly { get; set; }

        /// <summary>
        /// 舱位价格.
        /// </summary>
        /// <value>The single price.</value>
        public string getSinglePrice { get; set; }

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
            return this.getCode == (obj as AvItemCabinChar).getCode;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return string.Format(@"{0}{1}", this.getCode, this.getAvalibly);
        }
        
    }

    /// <summary>
    /// 航班结构
    /// </summary>
    public class AvItem:ASyncResult {
        
        /// <summary>
        /// 得到指定AvSegment对象内包含的航班号.
        /// </summary>
        /// <value>The get airline.</value>
        public string getAirline { get; set; }

        /// <summary>
        /// 如果是代码共享,获取该航班号的实际承运航班 否则无意义 使用前先使用 isCodeShare()判断是否由其他航班代码共享 .
        /// </summary>
        /// <value>The get carrier.</value>
        public string getCarrier { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的到达时刻的DATE型表示.
        /// </summary>
        /// <value>The get arridate.</value>
        public string getArridate { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的到达时间.
        /// </summary>
        /// <value>The get arritime.</value>
        public string getArritime { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的到达日期修正.
        /// </summary>
        /// <value>The get arritimemodify.</value>
        public string getArritimemodify { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的ASR支持标志.
        /// </summary>
        /// <value><c>true</c> if [get asr]; otherwise, <c>false</c>.</value>
        public bool getAsr { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的起飞时刻DATE表示.
        /// </summary>
        /// <value>The get depdate.</value>
        public string getDepdate { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的起飞时刻.
        /// </summary>
        /// <value>The get deptime.</value>
        public string getDeptime { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的起飞日期修正.
        /// </summary>
        /// <value>The get deptimemodify.</value>
        public string getDeptimemodify { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的到达城市三字码.
        /// </summary>
        /// <value>The get dstcity.</value>
        public string getDstcity { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的连接级别.
        /// </summary>
        /// <value>The get link.</value>
        public string getLink { get; set; }

        /// <summary>
        /// 是否有餐食.
        /// </summary>
        /// <value><c>true</c> if [get meal]; otherwise, <c>false</c>.</value>
        public bool getMeal { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的起始城市三字码.
        /// </summary>
        /// <value>The get orgcity.</value>
        public string getOrgcity { get; set; }

        /// <summary>
        /// 得到指定AvSegment对象内包含的机型.
        /// </summary>
        /// <value>The get planestyle.</value>
        public string getPlanestyle { get; set; }

        /// <summary>
        ///  得到本航段的经停次数.
        /// </summary>
        /// <value>The get stopnumber.</value>
        public int getStopnumber { get; set; }

        /// <summary>
        /// 获取此航班是否由其他航班执行(代码共享) .
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is code share; otherwise, <c>false</c>.
        /// </value>
        public bool isCodeShare { get; set; }

        /// <summary>
        /// 有效舱位列表.
        /// </summary>
        /// <value>The get cabins.</value>
        public List<AvItemCabinChar> getCabins { get; set; }


        #region 重写
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return string.Format(@"{0}", this.getAirline);
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
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj) {
            return obj.GetHashCode() == this.GetHashCode();
        }
        #endregion
    }

}

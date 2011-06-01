using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;
using System.Runtime.Serialization;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 本类包含一个PNR航段的所有信息。航段有普通、开放、信息三种类别。对于不同类别的航段，该类中的有效字段个数会有所区别。普通航段的所有字段都是有定义的；而对于开放航段，起飞到达时间就没有明确的定义
    /// <remarks>
    /// 1.建立普通航段时
    /// (1)对于中国民航的航空公司的航班,代理人只能订取系统中实际存在的航班；
    /// (2)对于外国航空公司的航班,代理人可以任意订取,即使该航班实际并不存在,也可以建立。
    /// (3)建立普通航段组时,一次输入最多可订取5个航班。
    /// 2.建立信息航段，这样的航段不占用座位，只是作为信息通知营业员，为旅客预留联程航班的座位；
    /// 或者为了保证PNR中的航段的连续性，便于打票，而建立此航段。
    /// 3.建立开放航段，必须确认的内容是航段和舱位，其他内容可以置为不确定信息，如航空公司、旅行日期。
    /// </remarks>
    /// </summary>
    [DataContract]
    public class BookAirSeg:ASyncResult {

        #region 类别
        /// <summary>
        /// 类别，即设置为普通、开放、信息三种类别
        /// </summary>
        public enum AIRSEGTYPE : int {
            /// <summary>
            /// 表示普通航段
            /// </summary>
            AIRSEG_NORMAL = 1,
            /// <summary>
            /// 表示信息航段
            /// </summary>
            AIRSEG_ARNK = 2,
            /// <summary>
            /// 表示开放航段
            /// </summary>
            AIRSEG_OPEN = 3
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数（航班号、舱位等级、始发城市、到达城市、行动代码、订座数、起飞时间、类别）
        /// </summary>
        /// <param name="airNo">航班号.</param>
        /// <param name="fltClass">舱位等级.</param>
        /// <param name="tktNum">订座数.</param>
        /// <param name="orgCity">始发城市.</param>
        /// <param name="dstCity">到达城市.</param>
        /// <param name="actionCode">行动代码.</param>
        /// <param name="departureTime">起飞时间（格式为YYYY-MM-DD）.</param>
        /// <param name="type">类别.</param>
        public BookAirSeg(string airNo, string fltClass,string orgCity, string dstCity, string actionCode, DateTime departureTime,AIRSEGTYPE type) {
            this.getairNo = airNo;
            this.departureTime = departureTime;
            this.getactionCode = actionCode;
            this.getdesCity = dstCity;
            this.getfltClass = fltClass;
            this.getorgCity = orgCity;
            //this.gettktNum = tktNum;
            this.getType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookAirSeg"/> class.
        /// </summary>
        public BookAirSeg() { }
        #endregion

        #region 重写
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return string.Format("{0}-{1}", this.getairNo, this.getfltClass.ToString()) ;
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
            return this.GetHashCode() == obj.GetHashCode();
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
        #endregion

        /// <summary>
        ///  航班号.
        /// </summary>
        /// <value>The getair no.</value>
        [DataMember]
        public string getairNo { get;  set; }

        /// <summary>
        /// 舱位等级.
        /// </summary>
        /// <value>The getflt class.</value>
        [DataMember]
        public string getfltClass { get;  set; }

        /// <summary>
        /// 类别，即设置为普通、开放、信息三种类别.
        /// </summary>
        /// <value>The type of the get.</value>
        [DataMember]
        public AIRSEGTYPE getType { get;  set; }

        /// <summary>
        /// 行动代码.
        /// </summary>
        /// <value>The getaction code.</value>
        [DataMember]
        public string getactionCode { get;  set; }

        /// <summary>
        /// 始发城市.
        /// </summary>
        /// <value>The getorg city.</value>
        [DataMember]
        public string getorgCity { get;  set; }

        /// <summary>
        /// 到达城市.
        /// </summary>
        /// <value>The getdes city.</value>
        [DataMember]
        public string getdesCity { get;  set; }

        /// <summary>
        /// 起飞时间.
        /// </summary>
        /// <value>The departure time.</value>
        [DataMember]
        public DateTime departureTime { get;  set; }
    }
}

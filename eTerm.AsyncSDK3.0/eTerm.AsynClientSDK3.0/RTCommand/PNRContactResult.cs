using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;
using System.Runtime.Serialization;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 本类代表一个PNR联系组信息
    /// </summary>
    [DataContract]
    public class PNRContactResult:ASyncResult {
        /// <summary>
        /// 获取该联系信息所在城市.
        /// </summary>
        /// <value>The get city.</value>
        [DataMember]
        public string getCity { get; set; }

        /// <summary>
        /// 获取该联系信息.
        /// </summary>
        /// <value>The get contact.</value>
        [DataMember]
        public string getContact { get; set; }

        /// <summary>
        /// 获取该项对应旅客编号.
        /// </summary>
        /// <value>The get PSGR ID.</value>
        [DataMember]
        public string getPsgrID { get; set; }
    }
}

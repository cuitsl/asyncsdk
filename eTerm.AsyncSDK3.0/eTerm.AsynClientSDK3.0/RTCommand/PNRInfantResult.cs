using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;
using System.Runtime.Serialization;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 本类包含一个PNR婴儿组信息
    /// </summary>
    [DataContract]
    public class PNRInfantResult:ASyncResult {
        /// <summary>
        ///  获取该婴儿的生日.
        /// </summary>
        /// <value>The get brith.</value>
        [DataMember]
        public string getBrith { get; set; }

        /// <summary>
        /// 获取携带该婴儿的旅客在旅客组中的序号.
        /// </summary>
        /// <value>The get carrier.</value>
        [DataMember]
        public string getCarrier { get; set; }

        /// <summary>
        /// 获取该婴儿的姓名.
        /// </summary>
        /// <value>The name of the get.</value>
        [DataMember]
        public string getName { get; set; }
    }
}

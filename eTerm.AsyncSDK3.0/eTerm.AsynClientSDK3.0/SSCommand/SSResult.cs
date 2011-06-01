using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;
using System.Runtime.Serialization;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// SS结果结构
    /// </summary>
    [DataContract]
    public class SSResult:ASyncResult {
        /// <summary>
        /// 订票生成的PNR.
        /// </summary>
        /// <value>The get PNR.</value>
        [DataMember]
        public string getPnr { get; set; }
    }
}

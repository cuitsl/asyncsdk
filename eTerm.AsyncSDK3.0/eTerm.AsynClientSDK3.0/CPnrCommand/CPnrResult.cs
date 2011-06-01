using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;
using System.Runtime.Serialization;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// PNR取消结果
    /// </summary>
    [DataContract]
    public class CPnrResult:ASyncResult {
        /// <summary>
        /// 是否成功取消.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is canceld; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool isCanceld { get; set; }
    }
}

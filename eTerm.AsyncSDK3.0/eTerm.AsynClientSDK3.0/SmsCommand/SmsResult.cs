using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// SMS结果
    /// </summary>
    [DataContract]
    public sealed class SmsResult : ASyncResult {
        /// <summary>
        /// 是否成功发送.
        /// </summary>
        /// <value>The success.</value>
        [DataMember]
        public bool? Success { get; set; }
    }
}

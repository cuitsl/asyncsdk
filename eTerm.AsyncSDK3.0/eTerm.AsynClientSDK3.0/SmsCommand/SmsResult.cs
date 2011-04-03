using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// SMS结果
    /// </summary>
    public sealed class SmsResult : ASyncResult {
        /// <summary>
        /// 是否成功发送.
        /// </summary>
        /// <value>The success.</value>
        public bool? Success { get; set; }
    }
}

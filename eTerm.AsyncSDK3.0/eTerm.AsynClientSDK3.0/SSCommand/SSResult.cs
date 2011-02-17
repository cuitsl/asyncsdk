using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// SS结果结构
    /// </summary>
    public class SSResult:ASyncResult {
        /// <summary>
        /// 订票生成的PNR.
        /// </summary>
        /// <value>The get PNR.</value>
        public string getPnr { get; set; }
    }
}

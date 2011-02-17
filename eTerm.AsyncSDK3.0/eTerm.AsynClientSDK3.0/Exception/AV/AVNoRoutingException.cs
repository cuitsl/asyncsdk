using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK.AVException {
    /// <summary>
    /// 没有航线，见于AV和SK指令
    /// </summary>
    public class AVNoRoutingException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="AVNoRoutingException"/> class.
        /// </summary>
        public AVNoRoutingException() : base("GDS中目前不存在这样的航线！") { }
    }
}

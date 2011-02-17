using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK.SSException {
    /// <summary>
    /// 座位数与旅客数不一致
    /// </summary>
    public class SSSeatsNumberException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSSeatsNumberException"/> class.
        /// </summary>
        public SSSeatsNumberException() : base("座位数与旅客数不一致") { }
    }
}

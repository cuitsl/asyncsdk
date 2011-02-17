using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK.SSException {
    /// <summary>
    /// 缺少出票组
    /// </summary>
    public class SSTicketMissingException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSTicketMissingException"/> class.
        /// </summary>
        public SSTicketMissingException() : base("缺少出票组") { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK.RTException {
    /// <summary>
    /// Pnr已经删除
    /// </summary>
    public class RTPNRCancelledException : SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="RTPNRCancelledException"/> class.
        /// </summary>
        public RTPNRCancelledException() : base("Pnr已经删除") { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK.SSException {
    /// <summary>
    /// 缺少联系组
    /// </summary>
    public class SSContactMissingException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSContactMissingException"/> class.
        /// </summary>
        public SSContactMissingException() : base("缺少联系组") { }
    }
}

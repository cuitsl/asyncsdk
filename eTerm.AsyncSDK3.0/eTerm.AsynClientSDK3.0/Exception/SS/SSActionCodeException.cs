using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK.SSException {
    /// <summary>
    /// 行动代码不正确
    /// </summary>
    public class SSActionCodeException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSActionCodeException"/> class.
        /// </summary>
        public SSActionCodeException() : base("行动代码不正确。订取航段或者SSR项一般都需要输入") { }
    }
}

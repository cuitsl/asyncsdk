using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK.SSException {
    /// <summary>
    /// 姓名中或者自由格式文本中有非法字符
    /// </summary>
    public class SSInvalidCharException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSInvalidCharException"/> class.
        /// </summary>
        public SSInvalidCharException() : base("姓名中或者自由格式文本中有非法字符") { }
    }
}

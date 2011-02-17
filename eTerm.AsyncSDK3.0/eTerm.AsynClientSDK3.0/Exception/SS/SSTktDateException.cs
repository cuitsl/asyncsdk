using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK.SSException {
    /// <summary>
    /// 出票组日期错误，常见于出票时限晚于第一航段起飞时间
    /// </summary>
    public class SSTktDateException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSTktDateException"/> class.
        /// </summary>
        public SSTktDateException() : base("出票组日期错误，常见于出票时限晚于第一航段起飞时间") { }
    }
}

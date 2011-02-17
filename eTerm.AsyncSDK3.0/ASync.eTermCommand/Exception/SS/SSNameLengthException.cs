using System;
using System.Collections.Generic;
using System.Text;
using ASync.eTermCommand;

namespace ASync.eTermCommand.SSException {
    /// <summary>
    /// 姓名超长或者姓氏少于两个字符
    /// </summary>
    public class SSNameLengthException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSNameLengthException"/> class.
        /// </summary>
        public SSNameLengthException() : base("姓名超长或者姓氏少于两个字符") { }
    }
}

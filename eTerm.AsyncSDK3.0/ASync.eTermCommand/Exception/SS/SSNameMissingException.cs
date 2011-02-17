using System;
using System.Collections.Generic;
using System.Text;
using ASync.eTermCommand;

namespace ASync.eTermCommand.SSException {
    /// <summary>
    /// 缺少姓名组
    /// </summary>
    public class SSNameMissingException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSNameMissingException"/> class.
        /// </summary>
        public SSNameMissingException() : base("缺少姓名组") { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using ASync.eTermCommand;

namespace ASync.eTermCommand.SSException {
    /// <summary>
    /// 航班号不正确
    /// </summary>
    public class SSFltNumberException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSFltNumberException"/> class.
        /// </summary>
        public SSFltNumberException() : base("航班号不正确") { }
    }
}

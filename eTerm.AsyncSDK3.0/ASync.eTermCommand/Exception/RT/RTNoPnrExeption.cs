using System;
using System.Collections.Generic;
using System.Text;
using ASync.eTermCommand;

namespace ASync.eTermCommand.RTException {
    /// <summary>
    /// PNR号不正确
    /// </summary>
    public class RTNoPnrExeption:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="RTNoPnrExeption"/> class.
        /// </summary>
        public RTNoPnrExeption() : base("输入的PNR号在GDS中不存在，请检查！") { }
    }
}

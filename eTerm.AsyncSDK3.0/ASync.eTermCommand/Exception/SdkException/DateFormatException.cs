using System;
using System.Collections.Generic;
using System.Text;
using ASync.eTermCommand;

namespace ASync.eTermCommand.Exception {
    /// <summary>
    /// 输入的日期字符串格式不正确
    /// </summary>
    public class DateFormatException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateFormatException"/> class.
        /// </summary>
        public DateFormatException() : base("输入的日期字符串格式不正确") { }
    }
}

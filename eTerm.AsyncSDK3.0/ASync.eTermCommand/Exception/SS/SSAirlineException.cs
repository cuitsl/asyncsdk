using System;
using System.Collections.Generic;
using System.Text;
using ASync.eTermCommand;

namespace ASync.eTermCommand.SSException {
    /// <summary>
    /// 订座条目中有需要输入航空公司代码的项目
    /// </summary>
    public class SSAirlineException:SdkException {
        /// <summary>
        /// 订座条目中有需要输入航空公司代码的项目.
        /// <code>
        /// 订座条目中有需要输入航空公司代码的项目。 如：ssr foid cz hk/ni12345/p1 如果没有输入"cz"则会抛此异常。osi中某些组也需要输入航空公司代码
        /// </code>
        /// </summary>
        public SSAirlineException() : base("订座条目中有需要输入航空公司代码的项目。 如：ssr foid cz hk/ni12345/p1 如果没有输入\"cz\"则会抛此异常。osi中某些组也需要输入航空公司代码") { }
    }
}

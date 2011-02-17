using System;
using System.Collections.Generic;
using System.Text;
using ASync.eTermCommand;

namespace ASync.eTermCommand.SKException {
    /// <summary>
    /// SK查询时返回NO ROUTING
    /// </summary>
    public class SKNoRoutingException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SKNoRoutingException"/> class.
        /// </summary>
        public SKNoRoutingException() : base("SK查询时返回NO ROUTING") { }
    }
}

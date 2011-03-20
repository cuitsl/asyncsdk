using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace eTerm.AsyncSDK {
    /// <summary>
    /// 流量记数
    /// </summary>
    public sealed class SocketTraffic : BaseBinary<SocketTraffic> {
        /// <summary>
        /// 月份，格式为：201101.
        /// </summary>
        /// <value>The month string.</value>
        public string MonthString { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>The update date.</value>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the traffic.
        /// </summary>
        /// <value>The traffic.</value>
        public long? Traffic { get; set; }
    }
}

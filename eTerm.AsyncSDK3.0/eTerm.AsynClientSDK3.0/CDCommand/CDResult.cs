using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 机场信息
    /// </summary>
    public sealed class CDResult:ASyncResult {
        /// <summary>
        /// 国家代码.
        /// </summary>
        /// <value>The nationality code.</value>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the city code.
        /// </summary>
        /// <value>The city code.</value>
        public string CityCode { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        /// <value>The short name.</value>
        public string ShortName { get; set; }
    }
}

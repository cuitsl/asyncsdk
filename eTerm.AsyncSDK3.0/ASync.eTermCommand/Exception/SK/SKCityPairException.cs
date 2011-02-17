using System;
using System.Collections.Generic;
using System.Text;
using ASync.eTermCommand;

namespace ASync.eTermCommand.SKException {
    /// <summary>
    /// sk查询时选择的城市对不正确，主机返回"CITY PAIR"。 
    /// </summary>
    public class SKCityPairException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SKCityPairException"/> class.
        /// </summary>
        public SKCityPairException() : base("sk查询时选择的城市对不正确，主机返回\"CITY PAIR\"。 ") { }
    }
}

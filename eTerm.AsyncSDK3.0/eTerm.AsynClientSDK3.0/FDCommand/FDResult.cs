using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 国内运信息结构
    /// </summary>
    public class FDResult:ASyncResult {
        /// <summary>
        /// 运价集合.
        /// </summary>
        /// <value>The get elements.</value>
        public List<FDItem> getElements { get; set; }

        /// <summary>
        /// 获取查询结果条目数量.
        /// </summary>
        /// <value>The get element num.</value>
        public int getElementNum { get { return this.getElements.Count; } }

        /// <summary>
        /// 获取货币类型.
        /// </summary>
        /// <value>The type of the get money.</value>
        public string getMoneyType { get; set; }

        /// <summary>
        /// 基准价格.
        /// </summary>
        /// <value>The get base fare.</value>
        public float getBaseFare { get; set; }

        /// <summary>
        /// 得到始发城市.
        /// </summary>
        /// <value>The get org.</value>
        public string getOrg { get; set; }

        /// <summary>
        /// 得到终到城市.
        /// </summary>
        /// <value>The get DST.</value>
        public string getDst { get; set; }

        /// <summary>
        /// 城市间的距离.
        /// </summary>
        /// <value>The get distance.</value>
        public string getDistance { get; set; }

        /// <summary>
        /// 获取对应纪录的机场税.
        /// </summary>
        /// <value>The get airport tax.</value>
        public string getAirportTax { get; set; }

    }
}

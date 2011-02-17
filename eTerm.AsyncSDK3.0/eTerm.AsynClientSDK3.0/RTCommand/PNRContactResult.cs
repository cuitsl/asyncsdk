using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 本类代表一个PNR联系组信息
    /// </summary>
    public class PNRContactResult:ASyncResult {
        /// <summary>
        /// 获取该联系信息所在城市.
        /// </summary>
        /// <value>The get city.</value>
        public string getCity { get; set; }

        /// <summary>
        /// 获取该联系信息.
        /// </summary>
        /// <value>The get contact.</value>
        public string getContact { get; set; }

        /// <summary>
        /// 获取该项对应旅客编号.
        /// </summary>
        /// <value>The get PSGR ID.</value>
        public string getPsgrID { get; set; }
    }
}

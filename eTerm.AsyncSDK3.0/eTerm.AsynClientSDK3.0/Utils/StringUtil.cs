using System;
using System.Collections.Generic;

using System.Text;

namespace eTerm.ASynClientSDK.Utils {
    public static class StringUtil {
        private static string __lastKey = string.Empty;
        /// <summary>
        /// 产生一个新的36位主键唯一编号.
        /// </summary>
        /// <value>The new36 primary key.</value>
        public static string New36PrimaryKey {
            get {
                __lastKey = Guid.NewGuid().ToString().Replace("-", string.Empty);
                return __lastKey;
            }
        }


        /// <summary>
        /// 上一次产生的主键值.
        /// </summary>
        /// <value>The last primary key.</value>
        public static string LastPrimaryKey { get { return __lastKey; } }


        /// <summary>
        /// 生成唯一的字符串
        /// </summary>
        /// <returns>唯一的字符串</returns>
        public static string GenUniqueString() {
            string readyStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string rtn = "";
            string guid = Guid.NewGuid().ToString();
            guid = guid.Replace("-", "");
            for (int i = 0; i < 8; i++) {
                int t = Convert.ToInt32(guid[i]) + Convert.ToInt32(guid[i + 8]) + Convert.ToInt32(guid[i + 16]) + Convert.ToInt32(guid[i + 24]);
                rtn += readyStr[t % 35];
            }

            return rtn;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.ASynClientSDK.Base;
using System.Text.RegularExpressions;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 城市机场指令
    /// <code>
    /// CDCommand Cp = new CDCommand();
    /// CDResult Cr = Cp.Commit("SHA") as CDResult;
    /// </code>
    /// </summary>
    public sealed class CDCommand : ASynCommand {

        #region 构造函数
        /// <summary>
        /// 使用定义配置项构造连接.
        /// </summary>
        /// <param name="address">服务器地址.</param>
        /// <param name="port">服务器端口.</param>
        /// <param name="userName">授权用户名.</param>
        /// <param name="userPass">授权用户密码.</param>
        /// <param name="groupCode">授权用户分组.</param>
        public CDCommand(string address, int port, string userName, string userPass, string groupCode) {

        }

        /// <summary>
        /// 使用配置文件配置项构造连接.
        /// </summary>
        public CDCommand() : base() { }
        #endregion

        /// <summary>
        /// 指令结果解析适配器.
        /// </summary>
        /// <param name="Msg">指令结果集合.</param>
        /// <returns></returns>
        protected override ASyncResult ResultAdapter(string Msg) {
            CDResult CdResult = new CDResult();
            MatchCollection m = Regex.Matches(Msg, @"([A-Z\s\/]+\,)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (m.Count == 0) return CdResult;
            CdResult.ShortName = m[1].Groups[1].Value;
            CdResult.FullName = m[3].Groups[1].Value;
            CdResult.CityCode = m[4].Groups[1].Value;
            CdResult.CountryCode = m[5].Groups[1].Value;
            return CdResult;
        }
    }
}

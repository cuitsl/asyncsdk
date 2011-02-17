using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using eTerm.ASynClientSDK.Base;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 取消PNR
    /// <code>
    /// CPnrCommand Cp = new CPnrCommand();
    /// CPnrResult Cr = Cp.Commit("UYE9Y") as CPnrResult;
    /// </code>
    /// <example>
    /// CPnrCommand Cp = new CPnrCommand();
    /// CPnrResult Cr = Cp.Commit("UYE9Y") as CPnrResult;
    /// </example>
    /// </summary>
    public class CPnrCommand:ASynCommand {

        #region 构造函数
        /// <summary>
        /// 使用定义配置项构造连接.
        /// </summary>
        /// <param name="address">服务器地址.</param>
        /// <param name="port">服务器端口.</param>
        /// <param name="userName">授权用户名.</param>
        /// <param name="userPass">授权用户密码.</param>
        /// <param name="groupCode">授权用户分组.</param>
        public CPnrCommand(string address, int port, string userName, string userPass, string groupCode) {

        }

        /// <summary>
        /// 使用配置文件配置项构造连接.
        /// </summary>
        public CPnrCommand() : base() { }
        #endregion

        #region 重写
        /// <summary>
        /// 指令结果解析适配器.
        /// </summary>
        /// <param name="Msg">指令结果集合.</param>
        /// <returns></returns>
        protected override ASyncResult ResultAdapter(string Msg) {
            CPnrResult Cr = new CPnrResult();
            Cr.isCanceld = Regex.IsMatch(Msg, @"PNR\s+CANCELLED", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return Cr;
        }
        #endregion

        #region 外部方法
        /// <summary>
        /// 提交请求.
        /// </summary>
        /// <param name="pnrNo">The PNR no.</param>
        /// <returns></returns>
        public ASyncResult Commit(string pnrNo) {
            Connect();
            SendStream(string.Format(@"RT:{0}", pnrNo));
            GetStream();
            return GetSyncResult("XEPNR@");
        }
        #endregion
    }
}

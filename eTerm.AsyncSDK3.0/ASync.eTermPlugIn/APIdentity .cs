using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Net;
using System.Text.RegularExpressions;
using eTerm.AsyncSDK.Util;

namespace ASync.eTermPlugIn
{
    /// <summary>
    /// API认证插件
    /// </summary>
    [AfterASynCommand(@"!API", IsSystem=true)]
    public sealed class APIdentity : BaseASyncPlugIn
    {
        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="SESSION">会话端.</param>
        /// <param name="InPacket">入口数据包.</param>
        /// <param name="OutPacket">出口数据包.</param>
        /// <param name="Key">The key.</param>
        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key)
        {
            string ExpressValue = Regex.Match(Encoding.GetEncoding("gb2312").GetString(SESSION.UnInPakcet(InPacket)).Trim(), @"^!api\s+([A-Z0-9]+)", RegexOptions.IgnoreCase| RegexOptions.Multiline).Groups[1].Value;
            try
            {
                StringBuilder ApiKey = new StringBuilder();
                foreach (byte c in TEACrypter.MD5(Encoding.Default.GetBytes(Key.Company)))
                {
                    ApiKey.Append( String.Format("{0:X}", c).PadLeft(2, '0'));
                }
                if (!ApiKey.ToString().Equals(ExpressValue)) { SESSION.Close(); return; }
                ApiKey = new StringBuilder();
                foreach (byte c in TEACrypter.MD5(Encoding.Default.GetBytes(ExpressValue)))
                {
                    ApiKey.Append(String.Format("{0:X}", c).PadLeft(2, '0'));
                }
                SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID,ApiKey.ToString()));
            }
            catch (Exception ex)
            {
                SESSION.Close();
                //SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, ex.Message));
            }
        }

        /// <summary>
        /// 验证可用性.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        protected override bool ValidatePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key)
        {
            return Key.AllowIntercept;
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get
            {
                return "表达式计算插件";
            }
        }

    }
}

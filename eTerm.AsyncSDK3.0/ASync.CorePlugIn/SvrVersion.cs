using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Net;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace ASync.CorePlugIn
{
    [AfterASynCommand("!SvrVersion", IsSystem = true)]
    public sealed class SvrVersion : BaseASyncPlugIn
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
            FileInfo fileInfo=new FileInfo(@"Version.Xml");
            if (!fileInfo.Exists) { __eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, @"<Version Id=""0""></Version>"); return; }
            string VersionContent = string.Empty;
            using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open)) {
                StreamReader sr = new StreamReader(fs);
                VersionContent=sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
            }
            SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, VersionContent));
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
            return SESSION != null;
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get
            {
                return "主机版本获取";
            }
        }

    }
}

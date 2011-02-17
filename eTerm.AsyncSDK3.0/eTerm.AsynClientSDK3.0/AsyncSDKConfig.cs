using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.ASynClientSDK.Config.DotNetConfig;
using System.Configuration;

namespace eTerm.ASynClientSDK {
    public sealed class AsyncSDKConfig : SectionBaseHandler<AsyncSDKConfig> {
        /// <summary>
        /// 默认服务器地址.
        /// </summary>
        /// <value>The address.</value>
        [ConfigurationProperty("Address", IsRequired = false)]
        public string Address {
            get { return (string)this["Address"]; }
        }

        /// <summary>
        /// 默认服务器端口.
        /// </summary>
        /// <value>The port.</value>
        [ConfigurationProperty("Port", IsRequired = false)]
        public int Port {
            get { return (int)this["Port"]; }
        }

        /// <summary>
        /// 是否为启用安全链接.
        /// </summary>
        /// <value><c>true</c> if this instance is SSL; otherwise, <c>false</c>.</value>
        [ConfigurationProperty("IsSsl", IsRequired = false)]
        public bool IsSsl {
            get { return (bool)this["IsSsl"]; }
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        [ConfigurationProperty("UserName", IsRequired = false)]
        public string UserName {
            get { return (string)this["UserName"]; }
        }

        /// <summary>
        /// Gets the user pass.
        /// </summary>
        /// <value>The user pass.</value>
        [ConfigurationProperty("UserPass", IsRequired = false)]
        public string UserPass {
            get { return (string)this["UserPass"]; }
        }

        /// <summary>
        /// 自动指令间隔(以秒为单位).
        /// </summary>
        /// <value>The syn CMD thread.</value>
        [ConfigurationProperty("ASynCmdThread", IsRequired = false)]
        public double ASynCmdThread {
            get { return (double)this["ASynCmdThread"]; }
        }
    }
}

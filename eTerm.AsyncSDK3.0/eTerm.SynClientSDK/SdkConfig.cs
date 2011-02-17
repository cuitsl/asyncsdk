using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.SynClientSDK.Config.DotNetConfig;
using System.Configuration;

namespace eTerm.SynClientSDK {
    public sealed class SdkConfig : SectionBaseHandler<SdkConfig> {
        /// <summary>
        /// 默认服务器地址.
        /// </summary>
        /// <value>The address.</value>
        [ConfigurationProperty("Address", IsRequired = false)]
        public string Address {
            get { return (string)this["Address"]; }
        }

        /// <summary>
        /// Gets the group code.
        /// </summary>
        /// <value>The group code.</value>
        [ConfigurationProperty("GroupCode", IsRequired = false)]
        public string GroupCode {
            get { return (string)this["GroupCode"]; }
        }

        /// <summary>
        /// Gets the syn CMD thread.
        /// </summary>
        /// <value>The syn CMD thread.</value>
        [ConfigurationProperty("SynCmdThread", IsRequired = false)]
        public float SynCmdThread {
            get { return (float)this["SynCmdThread"]; }
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

    }
}

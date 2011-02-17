using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Net;
using eTerm.AsyncSDK.Core;

namespace eTerm.AsyncSDK {
    /// <summary>
    /// 插件类配置
    /// </summary>
    [Serializable]
    public sealed class PlugInSetup:BaseBinary<PlugInSetup> {
        /// <summary>
        /// Gets or sets the name of the plug in.
        /// </summary>
        /// <value>The name of the plug in.</value>
        public string PlugInName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the type.
        /// </summary>
        /// <value>The full name of the type.</value>
        public string TypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the assembly path.
        /// </summary>
        /// <value>The assembly path.</value>
        public string AssemblyPath { get; set; }

        /// <summary>
        /// 构造实体.
        /// </summary>
        /// <value>The plug instance.</value>
        /// <returns></returns>
        public IAfterCommand<eTerm443Async, eTerm443Packet> ASyncInstance { get; set; }

        /// <summary>
        /// 构造实体.
        /// </summary>
        /// <value>The plug instance.</value>
        /// <returns></returns>
        public IAfterCommand<eTerm363Session, eTerm363Packet> ClientSessionInstance { get; set; }
    }
}

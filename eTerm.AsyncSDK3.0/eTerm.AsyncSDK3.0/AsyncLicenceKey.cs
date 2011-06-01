using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace eTerm.AsyncSDK {
    /// <summary>
    /// 授权实体
    /// </summary>
    [Serializable]
    public sealed class AsyncLicenceKey:BaseBinary<AsyncLicenceKey> {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLicenceKey"/> class.
        /// </summary>
        public AsyncLicenceKey() { }

        /// <summary>
        /// 使用期限.
        /// </summary>
        /// <value>The expire date.</value>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 被授予的公司名称.
        /// </summary>
        /// <value>The company.</value>
        public string Company { get; set; }

        /// <summary>
        /// 剩余分钟数.
        /// </summary>
        /// <value>The remaining hour.</value>
        public double RemainingMinutes { get; set; }

        /// <summary>
        /// 最大可承受资源数.
        /// </summary>
        /// <value>The max async.</value>
        public int MaxAsync { get; set; }

        /// <summary>
        /// 最大终端数.
        /// </summary>
        /// <value>The max T session.</value>
        public int MaxTSession { get; set; }

        /// <summary>
        /// 是否允许使用数据库.
        /// </summary>
        /// <value><c>true</c> if [allow database]; otherwise, <c>false</c>.</value>
        public bool AllowDatabase { get; set; }

        /// <summary>
        /// 是否允许日志.
        /// </summary>
        /// <value>The allow db log.</value>
        public bool? AllowDbLog { get; set; }

        /// <summary>
        /// 是否启用指令后续插件处理(启用将损耗一定的处理性能).
        /// </summary>
        /// <value><c>true</c> if [allow after validate]; otherwise, <c>false</c>.</value>
        public bool AllowAfterValidate { get; set; }

        /// <summary>
        /// 是否启用指令拦截服务(启用将损耗一定的处理性能).
        /// </summary>
        /// <value><c>true</c> if [allow intercept]; otherwise, <c>false</c>.</value>
        public bool AllowIntercept { get; set; }

        /// <summary>
        /// 数据库连接串.
        /// </summary>
        /// <value>The SQL conn string.</value>
        public string connectionString { get; set; }

        /// <summary>
        /// 月流量限制.
        /// </summary>
        /// <value>The max command per month.</value>
        public int MaxCommandPerMonth { get; set; }

        /// <summary>
        /// 数据库提供程序.
        /// </summary>
        /// <value>The name of the provider.</value>
        public string providerName { get; set; }

        /// <summary>
        /// 授权码.
        /// </summary>
        /// <value>The key.</value>
        public byte[] Key { get; set; }

        /// <summary>
        /// 是否允许eTerm终端.
        /// </summary>
        /// <value>The allowe term client.</value>
        public bool? AlloweTermClient { get; set; }
    }
}

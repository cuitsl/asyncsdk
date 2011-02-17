using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace eTerm.SynClientSDK.Config.DotNetConfig {
    /// <summary>
    /// 配置集合（泛型）
    /// </summary>
    /// <typeparam name="T">集合元素类型</typeparam>
    public class ConfigCollection<T> : BaseConfigurationElementCollection where T : ConfigurationElement, new() {
        /// <summary>
        /// 按索引方式获取元素
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>元素</returns>
        public virtual T this[int index] {
            get { return (T)this.BaseGet(index); }
        }

        /// <summary>
        /// 按键值方式获取元素
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>元素</returns>
        public virtual T this[object key] {
            get { return (T)this.BaseGet(key); }
        }

        /// <summary>
        /// 集合转换成数组
        /// </summary>
        /// <returns>元素数组</returns>
        public virtual T[] ToArray() {
            T[] values = new T[this.Count];
            this.CopyTo(values, 0);
            return values;
        }

        /// <summary>
        /// 创建新元素
        /// </summary>
        /// <returns>新元素</returns>
        protected override ConfigurationElement CreateNewElement() {
            return new T();
        }
    }
}

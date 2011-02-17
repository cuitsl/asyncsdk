﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace eTerm.ASynClientSDK.Config.DotNetConfig {
    /// <summary>
    /// 基础的配置元素集合（抽象），继承自 <seealso cref="ConfigurationElementCollection"/>
    /// </summary>
    public abstract class BaseConfigurationElementCollection : ConfigurationElementCollection {
        /// <summary>
        /// 获得元素的键
        /// </summary>
        /// <param name="element">配置元素</param>
        /// <returns>键</returns>
        protected override object GetElementKey(ConfigurationElement element) {
            return element.GetHashCode();
        }
    }
}

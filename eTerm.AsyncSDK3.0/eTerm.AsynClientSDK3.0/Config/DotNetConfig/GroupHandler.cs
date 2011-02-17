using System;
using System.Collections.Generic;

using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using eTerm.ASynClientSDK.Utils;

namespace eTerm.ASynClientSDK.Config.DotNetConfig {
    /// <summary>
    /// 配置组（依赖 .NET的配置架构）
    /// </summary>
    /// <remarks>
    ///		<code>
    ///			&lt;configSections&gt;
    ///				&lt;sectionGroup name="eTerm.ASynClientSDK" type="eTerm.ASynClientSDK.Config.DotNetConfig.GroupHandler, eTerm.ASynClientSDK"&gt;
    ///					&lt;section name="mail" type="eTerm.ASynClientSDK.Utils.Mail.Config.SectionHandler, eTerm.ASynClientSDK" /&gt;
    ///					......
    ///				&lt;/sectionGroup&gt;
    ///			&lt;/configSections&gt;
    /// 
    ///			......
    /// 
    ///			&lt;eTerm.ASynClientSDK&gt;
    ///				&lt;mail&gt;
    ///					&lt;smtpSetting server="" port="" userName="" password="" /&gt;
    ///				&lt;/mail&gt;
    ///			&lt;/eTerm.ASynClientSDK&gt;
    ///			......
    /// </code>
    /// </remarks>
    public class GroupHandler : ConfigurationSectionGroup {
        private static GroupHandler instance;
        private static bool isInit;
        private static bool isWebApp;
        private static Dictionary<Type, ConfigurationSection> sectionCache;
        private static readonly object lockObject = new object();

        private static void Init(bool throwOnError) {
            if (isInit) {
                return;
            }
            Configuration config;
            if (HttpContext.Current != null) {
                isWebApp = true;
                config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            }
            else {
                isWebApp = false;
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            foreach (string key in config.SectionGroups.Keys) {
                ConfigurationSectionGroup csg = config.SectionGroups[key];
                if (csg == null || string.IsNullOrEmpty(csg.Type)) {
                    continue;
                }
                Type csgType = TypeUtil.CreateType(csg.Type, false);
                if (csgType == typeof(GroupHandler)) {
                    break;
                }
            }

            sectionCache = new Dictionary<Type, ConfigurationSection>();

            if (instance == null) {
                isInit = true;
                if (throwOnError) {
                    throw new System.Exception("配置组未正确配置");
                }
                return;
            }

            foreach (ConfigurationSection section in instance.Sections) {
                string sectionTypeName = section.SectionInformation.Type;
                string sectionName = section.SectionInformation.SectionName;
                Type sectionType = TypeUtil.CreateType(sectionTypeName, false);
                object objectSection;
                if (isWebApp) {
                    objectSection = WebConfigurationManager.GetSection(sectionName);
                }
                else {
                    objectSection = ConfigurationManager.GetSection(sectionName);
                }
                if (objectSection != null) {
                    sectionCache.Add(sectionType, (ConfigurationSection)objectSection);
                }
            }

            isInit = true;
        }

        /// <summary>
        /// 当前配置组的实例（单件）
        /// </summary>
        public static GroupHandler Instance {
            get { return instance; }
        }

        /// <summary>
        /// 获取配置节（泛型）
        /// </summary>
        /// <typeparam name="T">配置节类型</typeparam>
        /// <returns>配置节</returns>
        public static T GetSection<T>() where T : ConfigurationSection {
            return GetSection<T>(true);
        }

        /// <summary>
        /// 获取配置节（泛型）
        /// </summary>
        /// <typeparam name="T">配置节类型</typeparam>
        /// <param name="throwOnError">如果配置组未配置是否抛出异常</param>
        /// <returns>配置节</returns>
        public static T GetSection<T>(bool throwOnError) where T : ConfigurationSection {
            if (!isInit) {
                lock (lockObject) {
                    if (!isInit) {
                        Init(throwOnError);
                    }
                }
            }

            Type type = typeof(T);
            ConfigurationSection sectionObject;
            sectionCache.TryGetValue(type, out sectionObject);
            return sectionObject as T;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        protected GroupHandler() {
            instance = this;
        }
    }
}

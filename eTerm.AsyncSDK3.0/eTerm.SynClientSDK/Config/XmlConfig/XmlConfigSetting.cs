
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;


namespace eTerm.SynClientSDK.Config.XmlConfig {
    /// <summary>
    /// XML方式实现配置节 <see cref="ConfigSetting"/>
    /// </summary>
    public class XmlConfigSetting : ConfigSetting {
        #region constructor

        /// <summary>
        /// 构造方法
        /// </summary>
        protected XmlConfigSetting() { }

        #endregion

        /// <summary>
        /// 创建配置节实例
        /// </summary>
        /// <returns></returns>
        protected override ConfigSetting CreateConfigSetting() {
            return new XmlConfigSetting();
        }

        /// <summary>
        /// 创建配置值
        /// </summary>
        /// <param name="name">配置值名</param>
        /// <param name="value">配置值</param>
        /// <param name="readonly">是否只读</param>
        /// <returns>SettingValue</returns>
        protected override SettingValue CreateSettingValue(string name, string value, bool @readonly) {
            return new XmlSettingValue(name, value, @readonly);
        }

        /// <summary>
        /// 创建配置属性实例
        /// </summary>
        /// <param name="readonly">是否只读</param>
        /// <returns>SettingProperty</returns>
        protected override SettingProperty CreateSettingProperty(bool @readonly) {
            return new XmlSettingProperty(@readonly);
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="sb"><see cref="StringBuilder"/></param>
        /// <param name="layerIndex">所处层次</param>
        protected override void ToString(StringBuilder sb, int layerIndex) {
            string layerString = new string('\t', layerIndex);
            sb.AppendFormat("{0}<{1}", layerString, this.SettingName);
            if (this.Property.Count > 0) {
                sb.AppendFormat(" {0}", this.Property);
            }
            if (this.childSettings.Count > 0) {
                sb.AppendFormat(">{0}\r\n", HttpUtility.HtmlAttributeEncode(this.Value.Value));
                foreach (XmlConfigSetting setting in this.childSettings.Values) {
                    setting.ToString(sb, layerIndex + 1);
                }
                sb.AppendFormat("{0}</{1}>\r\n", layerString, this.SettingName);
            }
            else {
                if (this.Value.Value != null) {
                    sb.AppendFormat(">{0}</{1}>\r\n", HttpUtility.HtmlAttributeEncode(this.Value.Value), this.SettingName);
                }
                else {
                    sb.AppendLine(" />");
                }
            }
        }

        /// <summary>
        /// 创建配置节
        /// </summary>
        /// <param name="parent">父配置节</param>
        /// <param name="xmlNode">XML节</param>
        /// <param name="readonly">是否只读</param>
        /// <param name="searchPath">XML搜索目录列表</param>
        /// <param name="configFiles">如果有子配置文件，则添加到此列表</param>
        internal static XmlConfigSetting Create(XmlConfigSetting parent, XmlNode xmlNode, bool @readonly, string[] searchPath, List<string> configFiles) {
            if (xmlNode.NodeType != XmlNodeType.Element) {
                throw new ConfigException("解析到非法元素");
            }

            if (searchPath == null || searchPath.Length <= 0) {
                searchPath = ConfigHelper.ConfigFileDefaultSearchPath;
            }
            XmlConfigSetting setting = new XmlConfigSetting();
            setting.parent = parent;
            setting.ReadOnly = @readonly;
            setting.property = new XmlSettingProperty(xmlNode, @readonly);
            setting.childSettings = new ConfigSettingCollection(true);
            setting.operatorSettings = new ConfigSettingCollection(false);
            setting.value = new XmlSettingValue(xmlNode, @readonly);
            setting.SettingName = setting.Name;
            ConfigSettingOperator settingOperator = setting.SettingOperator;
            if (settingOperator != 0 && settingOperator != ConfigSettingOperator.Clear) {
                string newName = setting.Property.TryGetPropertyValue(ConfigNamePropertyName);
                if (string.IsNullOrEmpty(newName)) {
                    throw new ConfigException("配置命令未定义属性：" + ConfigNamePropertyName);
                }
                setting.value.SetName(newName);
            }

            foreach (XmlNode node in xmlNode.ChildNodes) {
                if (node.NodeType != XmlNodeType.Element) {
                    continue;
                }
                XmlConfigSetting childSetting = Create(setting, node, @readonly, searchPath, configFiles);
                setting.operatorSettings.Add(childSetting);
            }
            Compile(setting, setting.operatorSettings);

            string configFile = setting.ConfigFile;
            string configNode = setting.ConfigNode;
            if (!string.IsNullOrEmpty(configFile)) {
                configFile = ConfigHelper.SearchConfigFile(configFile, searchPath);
                if (!string.IsNullOrEmpty(configFile)) {
                    if (string.IsNullOrEmpty(configNode)) {
                        configNode = "/";
                    }
                    XmlNode newNode = ConfigHelper.LoadXmlNodeFromFile(configFile, configNode, false);
                    if (newNode != null) {
                        if (configFiles != null) {
                            configFiles.Add(configFile);
                        }
                        setting.Merge(Create(parent, newNode, @readonly, searchPath, configFiles));
                    }
                }
            }
            return setting;
        }

        /// <summary>
        /// 创建配置节
        /// </summary>
        /// <param name="fileName">XML文件</param>
        /// <param name="readonly">是否只读</param>
        /// <param name="searchPath">XML搜索目录列表</param>
        /// <param name="configFiles">如果有子配置文件，则添加到此列表</param>
        /// <returns>配置节</returns>
        internal static XmlConfigSetting Create(string fileName, bool @readonly, string[] searchPath, List<string> configFiles) {
            fileName = ConfigHelper.SearchConfigFile(fileName, searchPath);
            if (string.IsNullOrEmpty(fileName)) {
                return null;
            }
            XmlNode xmlNode = ConfigHelper.LoadXmlNodeFromFile(fileName, "/", false);
            if (xmlNode == null) {
                return null;
            }
            if (searchPath == null || searchPath.Length <= 0) {
                searchPath = ConfigHelper.ConfigFileDefaultSearchPath;
            }
            string[] newSearchPath = new string[searchPath.Length + 1];
            newSearchPath[0] = Path.GetDirectoryName(fileName);
            searchPath.CopyTo(newSearchPath, 1);
            return Create(null, xmlNode, @readonly, newSearchPath, configFiles);
        }

        /// <summary>
        /// 创建配置节
        /// </summary>
        /// <param name="fileName">XML文件</param>
        /// <param name="readonly">是否只读</param>
        /// <returns>配置节</returns>
        internal static XmlConfigSetting Create(string fileName, bool @readonly) {
            return Create(fileName, @readonly, ConfigHelper.ConfigFileDefaultSearchPath, null);
        }

        /// <summary>
        /// 创建配置节
        /// </summary>
        /// <param name="fileName">XML文件</param>
        /// <returns>配置节</returns>
        internal static XmlConfigSetting Create(string fileName) {
            return Create(fileName, true);
        }
    }
}

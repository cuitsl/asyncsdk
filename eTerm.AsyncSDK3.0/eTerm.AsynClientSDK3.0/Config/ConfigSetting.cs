/******************************************************************************
	Copyright 2005-2007 R2@DevFx.NET 
	DevFx.NET is free software; you can redistribute it and/or modify
	it under the terms of the Lesser GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	DevFx.NET is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	Lesser GNU General Public License for more details.

	You should have received a copy of the Lesser GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
/*******************************************************************************/

using System;
using System.Text;
using eTerm.ASynClientSDK.Utils;

namespace eTerm.ASynClientSDK.Config
{
	/// <summary>
	/// 配置节实现
	/// </summary>
	public abstract class ConfigSetting : IConfigSetting
	{
		#region static fields

		/// <summary>
		/// 配置节另附的文件属性名
		/// </summary>
		public const string ConfigFilePropertyName = "configFile";
		/// <summary>
		/// 配置节在另附文件的节点名
		/// </summary>
		public const string ConfigNodePropertyName = "configNode";
		/// <summary>
		/// 配置节实际名称的属性名
		/// </summary>
		public const string ConfigNamePropertyName = "name";

		#endregion static fields

		#region constructor

		/// <summary>
		/// 保护构造方法
		/// </summary>
		protected ConfigSetting() {
		}

		#endregion constructor

		#region fields

		/// <summary>
		/// 是否只读
		/// </summary>
		private bool @readonly;
		/// <summary>
		/// 此配置节实际名称
		/// </summary>
		private string settingName;
		/// <summary>
		/// 配置值
		/// </summary>
		protected SettingValue value;
		/// <summary>
		/// 父配置节
		/// </summary>
		protected ConfigSetting parent;
		/// <summary>
		/// 配置属性
		/// </summary>
		protected SettingProperty property;
		/// <summary>
		/// 子配置节
		/// </summary>
		protected ConfigSettingCollection childSettings;
		/// <summary>
		/// 配置节命令集合
		/// </summary>
		protected ConfigSettingCollection operatorSettings;

		#endregion fields

		#region abstract methods

		/// <summary>
		/// 创建配置节实例
		/// </summary>
		/// <returns></returns>
		protected abstract ConfigSetting CreateConfigSetting();

		/// <summary>
		/// 创建配置值
		/// </summary>
		/// <param name="name">配置值名</param>
		/// <param name="value">配置值</param>
		/// <param name="readonly">是否只读</param>
		/// <returns>SettingValue</returns>
		protected abstract SettingValue CreateSettingValue(string name, string value, bool @readonly);

		/// <summary>
		/// 创建配置属性实例
		/// </summary>
		/// <param name="readonly">是否只读</param>
		/// <returns>SettingProperty</returns>
		protected abstract SettingProperty CreateSettingProperty(bool @readonly);

		/// <summary>
		/// 转换成字符串
		/// </summary>
		/// <param name="sb"><see cref="StringBuilder"/></param>
		/// <param name="layerIndex">所处层次</param>
		protected abstract void ToString(StringBuilder sb, int layerIndex);

		#endregion abstract methods

		#region members

		/// <summary>
		/// 创建配置节实例
		/// </summary>
		/// <param name="setting">被复制的配置节</param>
		/// <param name="deep">是否深度复制</param>
		/// <returns>配置节</returns>
		protected virtual ConfigSetting CreateConfigSetting(ConfigSetting setting, bool deep) {
			ConfigSetting newSetting = this.CreateConfigSetting();
			newSetting.@readonly = setting.ReadOnly;
			newSetting.settingName = setting.settingName;
			if (deep) {
				newSetting.value = setting.Value.Clone();
				newSetting.property = setting.Property.Clone(this.@readonly, deep);
				newSetting.childSettings = setting.childSettings.Clone();
				newSetting.operatorSettings = setting.operatorSettings.Clone();
			} else {
				newSetting.value = setting.Value;
				newSetting.property = setting.Property;
				newSetting.childSettings = setting.childSettings;
				newSetting.operatorSettings = setting.operatorSettings;
			}
			return newSetting;
		}

		/// <summary>
		/// 当前配置节是否只读
		/// </summary>
		public virtual bool ReadOnly {
			get { return this.@readonly; }
			protected set { this.@readonly = value; }
		}

		/// <summary>
		/// 此配置节的名
		/// </summary>
		public virtual string Name {
			get { return this.Value.Name; }
		}

		/// <summary>
		/// 此配置节实际名称
		/// </summary>
		public virtual string SettingName {
			get {
				if (!string.IsNullOrEmpty(this.settingName)) {
					return this.settingName;
				} else {
					return this.Name;
				}
			}
			protected set { this.settingName = value; }
		}

		/// <summary>
		/// 此配置节的名/值
		/// </summary>
		public virtual SettingValue Value {
			get { return this.value; }
			set {
				if(this.ReadOnly) {
					throw new ConfigException("配置节只读");
				}
				this.value = value;
			}
		}

		/// <summary>
		/// 包含此配置节的父配置节
		/// </summary>
		public virtual ConfigSetting Parent {
			get { return this.parent; }
			set { this.parent = value; }
		}

		/// <summary>
		/// 此配置节包含的子配置节数目
		/// </summary>
		public virtual int Children {
			get { return this.childSettings.Count; }
		}

		/// <summary>
		/// 配置节属性
		/// </summary>
		public virtual SettingProperty Property {
			get { return this.property; }
			set {
				if(this.ReadOnly) {
					throw new ConfigException("配置节只读");
				}
				this.property = value;
			}
		}

		/// <summary>
		/// 获取此配置节的另附文件
		/// </summary>
		public virtual string ConfigFile {
			get { return this.Property.TryGetPropertyValue(ConfigFilePropertyName); }
		}

		/// <summary>
		/// 获取此配置节另附文件中的节点
		/// </summary>
		public virtual string ConfigNode {
			get { return this.Property.TryGetPropertyValue(ConfigNodePropertyName); }			
		}

		/// <summary>
		/// 此节是否为配置节命令
		/// </summary>
		protected virtual ConfigSettingOperator SettingOperator {
			get { return ConvertUtil.StringToEnum<ConfigSettingOperator>(this.SettingName); }
		}

		/// <summary>
		/// 获取/设置子配置节
		/// </summary>
		/// <param name="childSettingName">子配置节名</param>
		/// <remarks>
		/// 如果不存在，将返回<c>null</c><br />
		/// 如果设置时存在相同的节，则替换
		/// </remarks>
		public virtual ConfigSetting this[string childSettingName] {
			get { return this.childSettings[childSettingName]; }
		}

		/// <summary>
		/// 获取子配置节
		/// </summary>
		/// <param name="childSettingIndex">子配置节顺序</param>
		/// <remarks>
		/// 如果不存在，将返回null
		/// </remarks>
		public virtual ConfigSetting this[int childSettingIndex] {
			get { return this.childSettings[childSettingIndex]; }
		}

		/// <summary>
		/// 获取所有子配置节
		/// </summary>
		/// <returns>配置节数组</returns>
		public virtual ConfigSetting[] GetChildSettings() {
			return this.childSettings.CopyToArray();
		}

		/// <summary>
		/// 按XPath方式获取配置节
		/// </summary>
		/// <param name="xpath">XPath</param>
		/// <returns>配置节</returns>
		/// <remarks>
		/// XPath为类似XML的XPath，形如<c>framework/modules/module"</c><br />
		/// 如果有相同的配置节，则返回第一个配置节
		/// </remarks>
		public virtual ConfigSetting GetChildSetting(string xpath) {
			string[] settingName = null;
			if(xpath != null) {
				if(xpath.StartsWith("/")) {
					xpath = xpath.Substring(1);
				}
				settingName = xpath.Split('/');
			}
			return this.GetChildSetting(settingName);
		}

		/// <summary>
		/// 按多级方式获取配置节
		/// </summary>
		/// <param name="settingName">多级的配置节名</param>
		/// <returns>配置节</returns>
		/// <remarks>
		/// 多级的配置节名，形如有如下配置：
		///		<code>
		///			&lt;app1&gt;
		///				&lt;app2&gt;
		///					&lt;app3&gt;&lt;/app3&gt;
		///				&lt;/app2&gt;
		///			&lt;/app1&gt;
		///		</code>
		///	则按顺序传入，比如<c>GetChildSetting("app1", "app2", "app3")</c>，此时返回名为<c>app3</c>的配置节<br />
		/// "."表示当前配置节，".."表示上级配置节
		/// </remarks>
		public virtual ConfigSetting GetChildSetting(params string[] settingName) {
			ConfigSetting setting = this;
			if(settingName != null && settingName.Length > 0) {
				for(int i = 0; setting != null && i < settingName.Length; i++) {
					string name = settingName[i];
					switch (name) {
						case ".":
						case null:
							break;
						case "..":
							setting = setting.parent;
							break;
						default:
							setting = setting[name];
							break;
					}
				}
			}
			return setting;
		}

		/// <summary>
		/// 克隆此配置节
		/// </summary>
		/// <param name="readonly">是否只读</param>
		/// <param name="deep">是否深层次的克隆</param>
		/// <returns>配置节</returns>
		public virtual ConfigSetting Clone(bool @readonly, bool deep) {
			ConfigSetting setting = this.CreateConfigSetting(this, deep);
			setting.@readonly = @readonly;
			return setting;
		}

		/// <summary>
		/// 克隆此配置节
		/// </summary>
		/// <returns>配置节</returns>
		public virtual ConfigSetting Clone() {
			return this.Clone(this.ReadOnly, true);
		}

		/// <summary>
		/// 合并配置节
		/// </summary>
		/// <param name="setting">需被合并的配置节</param>
		/// <returns>合并后的配置节</returns>
		public virtual ConfigSetting Merge(ConfigSetting setting) {
			if(setting == null || string.Compare(this.Name, setting.Name, true) != 0) {
				return this;
			}
			this.Property.Merge(setting.Property);
			this.value = setting.Value.Clone(this.ReadOnly);

			foreach(ConfigSetting configSetting in setting.operatorSettings.Values) {
				this.operatorSettings.Add(configSetting).Parent = this;
			}

			Compile(this, setting.operatorSettings);
			return this;
		}

		/// <summary>
		/// 编译本配置节，将执行一些配置命令，具有配置命令的配置节需执行本方法后才可以使用
		/// </summary>
		/// <param name="current">当前配置节</param>
		/// <param name="settings">配置命令集合</param>
		/// <returns>编译后的配置节</returns>
		protected static ConfigSetting Compile(ConfigSetting current, ConfigSettingCollection settings) {
			ConfigSettingCollection currentSettings = current.childSettings;
			if (settings.Count > 0) {
				for (int i = 0; i < settings.Count; i++) {
					ConfigSetting setting = settings[i];
					switch(setting.SettingOperator) {
						case ConfigSettingOperator.Add:
							if (currentSettings.Contains(setting.Name)) {
								throw new ConfigException(string.Format("已存在子配置节 {0}", setting.Name));
							} else {
								currentSettings.Add(setting).Parent = current;
							}
							break;
						case ConfigSettingOperator.Remove:
							currentSettings.Remove(setting.Name);
							break;
						case ConfigSettingOperator.Move:
							ConfigSetting moveSetting = currentSettings[setting.Name];
							if(moveSetting != null) {
								currentSettings.Remove(moveSetting.Name);
								currentSettings.Add(moveSetting);
							}
							break;
						case ConfigSettingOperator.Clear:
							currentSettings.Clear();
							break;
						case ConfigSettingOperator.Update:
							if (currentSettings.Contains(setting.Name)) {
								currentSettings[setting.Name].Merge(setting);
							}
							break;
						case ConfigSettingOperator.Set:
							if (currentSettings.Contains(setting.Name)) {
								currentSettings[setting.Name].Merge(setting);
							} else {
								currentSettings.Add(setting).Parent = current;
							}
							break;
						default:
							if (currentSettings.Contains(setting.Name)) {
								currentSettings[setting.Name].Merge(setting);
							} else {
								currentSettings.Add(setting).Parent = current;
							}
							break;
					}
				}
			}
			return current;
		}

		/// <summary>
		/// 获取根配置节
		/// </summary>
		/// <returns>配置节</returns>
		public virtual ConfigSetting GetRootSetting() {
			ConfigSetting root = this;
			while(root.parent != null) {
				root = root.parent;
			}
			return root;
		}

		/// <summary>
		/// 转换成字符串格式
		/// </summary>
		/// <returns>字符串</returns>
		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			this.ToString(sb, 0);
			return sb.ToString();
		}

		#endregion members

		#region ICloneable Members

		object ICloneable.Clone() {
			return this.Clone(this.ReadOnly, true);
		}

		#endregion

		#region IConfigSetting Members

		bool IConfigSetting.ReadOnly {
			get { return this.ReadOnly; }
		}

		string IConfigSetting.Name {
			get { return this.Name; }
		}

		string IConfigSetting.SettingName {
			get { return this.SettingName; }
		}

		ISettingValue IConfigSetting.Value {
			get { return this.Value; }
		}

		IConfigSetting IConfigSetting.Parent {
			get { return this.Parent; }
		}

		int IConfigSetting.Children {
			get { return this.Children; }
		}

		ISettingProperty IConfigSetting.Property {
			get { return this.Property; }
		}

		IConfigSetting IConfigSetting.this[string childSettingName] {
			get { return this[childSettingName]; }
		}

		IConfigSetting IConfigSetting.this[int childSettingIndex] {
			get { return this[childSettingIndex]; }
		}

		IConfigSetting[] IConfigSetting.GetChildSettings() {
			return this.GetChildSettings();
		}

		IConfigSetting IConfigSetting.GetChildSetting(string xpath) {
			return this.GetChildSetting(xpath);
		}

		IConfigSetting IConfigSetting.GetChildSetting(params string[] settingName) {
			return this.GetChildSetting(settingName);
		}

		IConfigSetting IConfigSetting.Clone(bool @readonly, bool deep) {
			return this.Clone(@readonly, deep);
		}

		IConfigSetting IConfigSetting.GetRootSetting() {
			return this.GetRootSetting();
		}

		void IConfigSetting.Merge(IConfigSetting setting) {
			this.Merge((ConfigSetting)setting);
		}

		#endregion
	}
}
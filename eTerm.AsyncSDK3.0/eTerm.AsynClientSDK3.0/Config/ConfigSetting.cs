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
	/// ���ý�ʵ��
	/// </summary>
	public abstract class ConfigSetting : IConfigSetting
	{
		#region static fields

		/// <summary>
		/// ���ý������ļ�������
		/// </summary>
		public const string ConfigFilePropertyName = "configFile";
		/// <summary>
		/// ���ý������ļ��Ľڵ���
		/// </summary>
		public const string ConfigNodePropertyName = "configNode";
		/// <summary>
		/// ���ý�ʵ�����Ƶ�������
		/// </summary>
		public const string ConfigNamePropertyName = "name";

		#endregion static fields

		#region constructor

		/// <summary>
		/// �������췽��
		/// </summary>
		protected ConfigSetting() {
		}

		#endregion constructor

		#region fields

		/// <summary>
		/// �Ƿ�ֻ��
		/// </summary>
		private bool @readonly;
		/// <summary>
		/// �����ý�ʵ������
		/// </summary>
		private string settingName;
		/// <summary>
		/// ����ֵ
		/// </summary>
		protected SettingValue value;
		/// <summary>
		/// �����ý�
		/// </summary>
		protected ConfigSetting parent;
		/// <summary>
		/// ��������
		/// </summary>
		protected SettingProperty property;
		/// <summary>
		/// �����ý�
		/// </summary>
		protected ConfigSettingCollection childSettings;
		/// <summary>
		/// ���ý������
		/// </summary>
		protected ConfigSettingCollection operatorSettings;

		#endregion fields

		#region abstract methods

		/// <summary>
		/// �������ý�ʵ��
		/// </summary>
		/// <returns></returns>
		protected abstract ConfigSetting CreateConfigSetting();

		/// <summary>
		/// ��������ֵ
		/// </summary>
		/// <param name="name">����ֵ��</param>
		/// <param name="value">����ֵ</param>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <returns>SettingValue</returns>
		protected abstract SettingValue CreateSettingValue(string name, string value, bool @readonly);

		/// <summary>
		/// ������������ʵ��
		/// </summary>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <returns>SettingProperty</returns>
		protected abstract SettingProperty CreateSettingProperty(bool @readonly);

		/// <summary>
		/// ת�����ַ���
		/// </summary>
		/// <param name="sb"><see cref="StringBuilder"/></param>
		/// <param name="layerIndex">�������</param>
		protected abstract void ToString(StringBuilder sb, int layerIndex);

		#endregion abstract methods

		#region members

		/// <summary>
		/// �������ý�ʵ��
		/// </summary>
		/// <param name="setting">�����Ƶ����ý�</param>
		/// <param name="deep">�Ƿ���ȸ���</param>
		/// <returns>���ý�</returns>
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
		/// ��ǰ���ý��Ƿ�ֻ��
		/// </summary>
		public virtual bool ReadOnly {
			get { return this.@readonly; }
			protected set { this.@readonly = value; }
		}

		/// <summary>
		/// �����ýڵ���
		/// </summary>
		public virtual string Name {
			get { return this.Value.Name; }
		}

		/// <summary>
		/// �����ý�ʵ������
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
		/// �����ýڵ���/ֵ
		/// </summary>
		public virtual SettingValue Value {
			get { return this.value; }
			set {
				if(this.ReadOnly) {
					throw new ConfigException("���ý�ֻ��");
				}
				this.value = value;
			}
		}

		/// <summary>
		/// ���������ýڵĸ����ý�
		/// </summary>
		public virtual ConfigSetting Parent {
			get { return this.parent; }
			set { this.parent = value; }
		}

		/// <summary>
		/// �����ýڰ����������ý���Ŀ
		/// </summary>
		public virtual int Children {
			get { return this.childSettings.Count; }
		}

		/// <summary>
		/// ���ý�����
		/// </summary>
		public virtual SettingProperty Property {
			get { return this.property; }
			set {
				if(this.ReadOnly) {
					throw new ConfigException("���ý�ֻ��");
				}
				this.property = value;
			}
		}

		/// <summary>
		/// ��ȡ�����ýڵ����ļ�
		/// </summary>
		public virtual string ConfigFile {
			get { return this.Property.TryGetPropertyValue(ConfigFilePropertyName); }
		}

		/// <summary>
		/// ��ȡ�����ý����ļ��еĽڵ�
		/// </summary>
		public virtual string ConfigNode {
			get { return this.Property.TryGetPropertyValue(ConfigNodePropertyName); }			
		}

		/// <summary>
		/// �˽��Ƿ�Ϊ���ý�����
		/// </summary>
		protected virtual ConfigSettingOperator SettingOperator {
			get { return ConvertUtil.StringToEnum<ConfigSettingOperator>(this.SettingName); }
		}

		/// <summary>
		/// ��ȡ/���������ý�
		/// </summary>
		/// <param name="childSettingName">�����ý���</param>
		/// <remarks>
		/// ��������ڣ�������<c>null</c><br />
		/// �������ʱ������ͬ�Ľڣ����滻
		/// </remarks>
		public virtual ConfigSetting this[string childSettingName] {
			get { return this.childSettings[childSettingName]; }
		}

		/// <summary>
		/// ��ȡ�����ý�
		/// </summary>
		/// <param name="childSettingIndex">�����ý�˳��</param>
		/// <remarks>
		/// ��������ڣ�������null
		/// </remarks>
		public virtual ConfigSetting this[int childSettingIndex] {
			get { return this.childSettings[childSettingIndex]; }
		}

		/// <summary>
		/// ��ȡ���������ý�
		/// </summary>
		/// <returns>���ý�����</returns>
		public virtual ConfigSetting[] GetChildSettings() {
			return this.childSettings.CopyToArray();
		}

		/// <summary>
		/// ��XPath��ʽ��ȡ���ý�
		/// </summary>
		/// <param name="xpath">XPath</param>
		/// <returns>���ý�</returns>
		/// <remarks>
		/// XPathΪ����XML��XPath������<c>framework/modules/module"</c><br />
		/// �������ͬ�����ýڣ��򷵻ص�һ�����ý�
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
		/// ���༶��ʽ��ȡ���ý�
		/// </summary>
		/// <param name="settingName">�༶�����ý���</param>
		/// <returns>���ý�</returns>
		/// <remarks>
		/// �༶�����ý������������������ã�
		///		<code>
		///			&lt;app1&gt;
		///				&lt;app2&gt;
		///					&lt;app3&gt;&lt;/app3&gt;
		///				&lt;/app2&gt;
		///			&lt;/app1&gt;
		///		</code>
		///	��˳���룬����<c>GetChildSetting("app1", "app2", "app3")</c>����ʱ������Ϊ<c>app3</c>�����ý�<br />
		/// "."��ʾ��ǰ���ýڣ�".."��ʾ�ϼ����ý�
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
		/// ��¡�����ý�
		/// </summary>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <param name="deep">�Ƿ����εĿ�¡</param>
		/// <returns>���ý�</returns>
		public virtual ConfigSetting Clone(bool @readonly, bool deep) {
			ConfigSetting setting = this.CreateConfigSetting(this, deep);
			setting.@readonly = @readonly;
			return setting;
		}

		/// <summary>
		/// ��¡�����ý�
		/// </summary>
		/// <returns>���ý�</returns>
		public virtual ConfigSetting Clone() {
			return this.Clone(this.ReadOnly, true);
		}

		/// <summary>
		/// �ϲ����ý�
		/// </summary>
		/// <param name="setting">�豻�ϲ������ý�</param>
		/// <returns>�ϲ�������ý�</returns>
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
		/// ���뱾���ýڣ���ִ��һЩ�����������������������ý���ִ�б�������ſ���ʹ��
		/// </summary>
		/// <param name="current">��ǰ���ý�</param>
		/// <param name="settings">���������</param>
		/// <returns>���������ý�</returns>
		protected static ConfigSetting Compile(ConfigSetting current, ConfigSettingCollection settings) {
			ConfigSettingCollection currentSettings = current.childSettings;
			if (settings.Count > 0) {
				for (int i = 0; i < settings.Count; i++) {
					ConfigSetting setting = settings[i];
					switch(setting.SettingOperator) {
						case ConfigSettingOperator.Add:
							if (currentSettings.Contains(setting.Name)) {
								throw new ConfigException(string.Format("�Ѵ��������ý� {0}", setting.Name));
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
		/// ��ȡ�����ý�
		/// </summary>
		/// <returns>���ý�</returns>
		public virtual ConfigSetting GetRootSetting() {
			ConfigSetting root = this;
			while(root.parent != null) {
				root = root.parent;
			}
			return root;
		}

		/// <summary>
		/// ת�����ַ�����ʽ
		/// </summary>
		/// <returns>�ַ���</returns>
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
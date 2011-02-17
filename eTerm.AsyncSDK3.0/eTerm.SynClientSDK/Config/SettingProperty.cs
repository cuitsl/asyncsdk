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
using eTerm.SynClientSDK.Utils;

namespace eTerm.SynClientSDK.Config
{
	/// <summary>
	///  ���ý����Ե�ʵ��
	/// </summary>
	public abstract class SettingProperty : ISettingProperty
	{
		/// <summary>
		/// �������췽��
		/// </summary>
		/// <param name="properties">���Լ���</param>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		protected SettingProperty(SettingValueCollection properties, bool @readonly) {
			this.properties = properties;
			this.@readonly = @readonly;
		}

		/// <summary>
		/// �Ƿ�ֻ��
		/// </summary>
		protected bool @readonly;

		/// <summary>
		/// ���Լ���
		/// </summary>
		protected SettingValueCollection properties;

		/// <summary>
		/// ������������ʵ��
		/// </summary>
		/// <param name="properties">���Լ���</param>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <returns>SettingProperty</returns>
		protected abstract SettingProperty CreateSettingProperty(SettingValueCollection properties, bool @readonly);

		/// <summary>
		/// ������������ʵ��
		/// </summary>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <returns>SettingProperty</returns>
		protected virtual SettingProperty CreateSettingProperty(bool @readonly) {
			return this.CreateSettingProperty(new SettingValueCollection(), @readonly);
		}

		/// <summary>
		/// ��������ֵ
		/// </summary>
		/// <param name="name">����ֵ��</param>
		/// <param name="value">����ֵ</param>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <returns>SettingValue</returns>
		protected abstract SettingValue CreateSettingValue(string name, string value, bool @readonly);

		/// <summary>
		/// ��ǰ���ý������Ƿ�ֻ��
		/// </summary>
		public virtual bool ReadOnly {
			get { return this.@readonly; }
		}

		/// <summary>
		/// ���ýڵ����Ը���
		/// </summary>
		public virtual int Count {
			get { return this.properties.Count; }
		}

		/// <summary>
		/// ��ȡ/��������ֵ(����������)
		/// </summary>
		/// <param name="propertyName">������</param>
		public virtual SettingValue this[string propertyName] {
			get { return this.properties[propertyName]; }
			set {
				if(this.ReadOnly) {
					throw new ConfigException("��������ֻ��");
				}
				this.properties.Set(propertyName, value);
			}
		}

		/// <summary>
		/// ��ȡ����ֵ(������������)
		/// </summary>
		/// <param name="propertyIndex">��������</param>
		public virtual SettingValue this[int propertyIndex] {
			get { return this.properties[propertyIndex]; }
		}

		/// <summary>
		/// ���Ի�ȡĳ����ֵ
		/// </summary>
		/// <param name="propertyName">������</param>
		/// <returns>����ֵ</returns>
		public virtual string TryGetPropertyValue(string propertyName) {
			SettingValue value = this.properties[propertyName];
			if (value != null) {
				return value.Value;
			} else {
				return null;
			}
		}

		/// <summary>
		/// ���Ի�ȡĳ����ֵ��ת����ָ������
		/// </summary>
		/// <typeparam name="T">ת����ָ��������</typeparam>
		/// <param name="propertyName">������</param>
		/// <returns>ָ�����͵�ʵ��</returns>
		public virtual T TryGetPropertyValue<T>(string propertyName) {
			return this.TryGetPropertyValue<T>(propertyName, default(T));
		}

		/// <summary>
		/// ���Ի�ȡĳ����ֵ��ת����ָ������
		/// </summary>
		/// <typeparam name="T">ת����ָ��������</typeparam>
		/// <param name="propertyName">������</param>
		/// <param name="defaultValue">ȱʡֵ</param>
		/// <returns>ָ�����͵�ʵ��</returns>
		public virtual T TryGetPropertyValue<T>(string propertyName, T defaultValue) {
			SettingValue value = this.properties[propertyName];
			if (value != null) {
				return (value as IConverting).TryToObject<T>(defaultValue);
			} else {
				return defaultValue;
			}
		}

		internal virtual SettingProperty Merge(SettingProperty property) {
			foreach(string key in property.properties.Keys) {
				switch(key) {
					case ConfigSetting.ConfigFilePropertyName:
					case ConfigSetting.ConfigNodePropertyName:
						break;
					default:
						this.properties.Set(property[key].Clone());
						break;
				}
			}
			return this;
		}

		#region Clone

		/// <summary>
		/// ��¡��������
		/// </summary>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <param name="deep">�Ƿ���ȸ���</param>
		/// <returns>SettingProperty</returns>
		public virtual SettingProperty Clone(bool @readonly, bool deep) {
			SettingProperty property = this.CreateSettingProperty(@readonly);
			if(deep) {
				foreach(SettingValue settingValue in this.properties.Values) {
					property.properties.Add(settingValue.Clone(@readonly));
				}
			} else {
				property.properties = this.properties;
			}
			return property;
		}

		#endregion Clone

		#region ICloneable Members

		object ICloneable.Clone() {
			return this.Clone(this.ReadOnly, true);
		}

		#endregion

		#region ISettingProperty Members

		bool ISettingProperty.ReadOnly {
			get { return this.ReadOnly; }
		}

		int ISettingProperty.Count {
			get { return this.Count; }
		}

		ISettingValue ISettingProperty.this[string propertyName] {
			get { return this[propertyName]; }
		}

		ISettingValue ISettingProperty.this[int propertyIndex] {
			get { return this[propertyIndex]; }
		}

		ISettingProperty ISettingProperty.Clone(bool @readonly, bool deep) {
			return this.Clone(@readonly, deep);
		}

		string ISettingProperty.TryGetPropertyValue(string propertyName) {
			return this.TryGetPropertyValue(propertyName);
		}

		T ISettingProperty.TryGetPropertyValue<T>(string propertyName) {
			return this.TryGetPropertyValue<T>(propertyName);
		}

		T ISettingProperty.TryGetPropertyValue<T>(string propertyName, T defaultValue) {
			return this.TryGetPropertyValue<T>(propertyName, defaultValue);
		}

		#endregion
	}
}
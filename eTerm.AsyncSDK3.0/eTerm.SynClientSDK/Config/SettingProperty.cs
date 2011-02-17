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
	///  配置节属性的实现
	/// </summary>
	public abstract class SettingProperty : ISettingProperty
	{
		/// <summary>
		/// 保护构造方法
		/// </summary>
		/// <param name="properties">属性集合</param>
		/// <param name="readonly">是否只读</param>
		protected SettingProperty(SettingValueCollection properties, bool @readonly) {
			this.properties = properties;
			this.@readonly = @readonly;
		}

		/// <summary>
		/// 是否只读
		/// </summary>
		protected bool @readonly;

		/// <summary>
		/// 属性集合
		/// </summary>
		protected SettingValueCollection properties;

		/// <summary>
		/// 创建配置属性实例
		/// </summary>
		/// <param name="properties">属性集合</param>
		/// <param name="readonly">是否只读</param>
		/// <returns>SettingProperty</returns>
		protected abstract SettingProperty CreateSettingProperty(SettingValueCollection properties, bool @readonly);

		/// <summary>
		/// 创建配置属性实例
		/// </summary>
		/// <param name="readonly">是否只读</param>
		/// <returns>SettingProperty</returns>
		protected virtual SettingProperty CreateSettingProperty(bool @readonly) {
			return this.CreateSettingProperty(new SettingValueCollection(), @readonly);
		}

		/// <summary>
		/// 创建配置值
		/// </summary>
		/// <param name="name">配置值名</param>
		/// <param name="value">配置值</param>
		/// <param name="readonly">是否只读</param>
		/// <returns>SettingValue</returns>
		protected abstract SettingValue CreateSettingValue(string name, string value, bool @readonly);

		/// <summary>
		/// 当前配置节属性是否只读
		/// </summary>
		public virtual bool ReadOnly {
			get { return this.@readonly; }
		}

		/// <summary>
		/// 配置节的属性个数
		/// </summary>
		public virtual int Count {
			get { return this.properties.Count; }
		}

		/// <summary>
		/// 获取/设置属性值(根据属性名)
		/// </summary>
		/// <param name="propertyName">属性名</param>
		public virtual SettingValue this[string propertyName] {
			get { return this.properties[propertyName]; }
			set {
				if(this.ReadOnly) {
					throw new ConfigException("配置属性只读");
				}
				this.properties.Set(propertyName, value);
			}
		}

		/// <summary>
		/// 获取属性值(根据属性索引)
		/// </summary>
		/// <param name="propertyIndex">属性索引</param>
		public virtual SettingValue this[int propertyIndex] {
			get { return this.properties[propertyIndex]; }
		}

		/// <summary>
		/// 尝试获取某属性值
		/// </summary>
		/// <param name="propertyName">属性名</param>
		/// <returns>属性值</returns>
		public virtual string TryGetPropertyValue(string propertyName) {
			SettingValue value = this.properties[propertyName];
			if (value != null) {
				return value.Value;
			} else {
				return null;
			}
		}

		/// <summary>
		/// 尝试获取某属性值并转换成指定类型
		/// </summary>
		/// <typeparam name="T">转换成指定的类型</typeparam>
		/// <param name="propertyName">属性名</param>
		/// <returns>指定类型的实例</returns>
		public virtual T TryGetPropertyValue<T>(string propertyName) {
			return this.TryGetPropertyValue<T>(propertyName, default(T));
		}

		/// <summary>
		/// 尝试获取某属性值并转换成指定类型
		/// </summary>
		/// <typeparam name="T">转换成指定的类型</typeparam>
		/// <param name="propertyName">属性名</param>
		/// <param name="defaultValue">缺省值</param>
		/// <returns>指定类型的实例</returns>
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
		/// 克隆配置属性
		/// </summary>
		/// <param name="readonly">是否只读</param>
		/// <param name="deep">是否深度复制</param>
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
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

namespace eTerm.ASynClientSDK.Config
{
	/// <summary>
	/// 配置节接口
	/// </summary>
	/// <remarks>
	///	形如下面的XML节表示一个配置节：
	///		<code>
	///			&lt;app my="myProperty"&gt;myValue&lt;/app&gt;
	///		</code>
	///	此时，<c>Name="app"</c>，Value的值为"myValue"，Property的值为"myProperty"
	/// </remarks>
	public interface IConfigSetting : ICloneable
	{
		/// <summary>
		/// 当前配置节是否只读
		/// </summary>
		bool ReadOnly { get; }

		/// <summary>
		/// 此配置节的名
		/// </summary>
		string Name { get; }

		/// <summary>
		/// 此配置节实际名称
		/// </summary>
		string SettingName { get; }

		/// <summary>
		/// 此配置节的名/值
		/// </summary>
		ISettingValue Value { get; }

		/// <summary>
		/// 包含此配置节的父配置节
		/// </summary>
		IConfigSetting Parent { get; }

		/// <summary>
		/// 此配置节包含的子配置节数目
		/// </summary>
		int Children { get; }

		/// <summary>
		/// 配置节属性
		/// </summary>
		ISettingProperty Property { get; }

		/// <summary>
		/// 获取子配置节
		/// </summary>
		/// <param name="childSettingName">子配置节名</param>
		/// <remarks>
		/// 如果不存在，将返回<c>null</c>
		/// </remarks>
		IConfigSetting this[string childSettingName] { get; }

		/// <summary>
		/// 获取子配置节
		/// </summary>
		/// <param name="childSettingIndex">子配置节顺序</param>
		/// <remarks>
		/// 如果不存在，将返回null
		/// </remarks>
		IConfigSetting this[int childSettingIndex] { get; }

		/// <summary>
		/// 获取所有子配置节
		/// </summary>
		/// <returns>配置节数组</returns>
		IConfigSetting[] GetChildSettings();

		/// <summary>
		/// 按XPath方式获取配置节
		/// </summary>
		/// <param name="xpath">XPath</param>
		/// <returns>配置节</returns>
		/// <remarks>
		/// XPath为类似XML的XPath，形如<c>framework/modules"</c><br />
		/// 如果有相同的配置节，则返回第一个配置节
		/// </remarks>
		IConfigSetting GetChildSetting(string xpath);

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
		///	则按顺序传入，比如<c>GetChildSetting("app1", "app2", "app3")</c>，此时返回名为<c>app3</c>的配置节
		/// </remarks>
		IConfigSetting GetChildSetting(params string[] settingName);

		/// <summary>
		/// 获取根配置节
		/// </summary>
		/// <returns>配置节</returns>
		IConfigSetting GetRootSetting();

		/// <summary>
		/// 合并配置节
		/// </summary>
		/// <param name="setting">被合并的配置节</param>
		void Merge(IConfigSetting setting);

		/// <summary>
		/// 克隆此配置节
		/// </summary>
		/// <param name="readonly">是否只读</param>
		/// <param name="deep">是否深层次的克隆</param>
		/// <returns>配置节</returns>
		IConfigSetting Clone(bool @readonly, bool deep);
	}
}

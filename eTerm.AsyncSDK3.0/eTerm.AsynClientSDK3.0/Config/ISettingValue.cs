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
using eTerm.ASynClientSDK.Utils;

namespace eTerm.ASynClientSDK.Config
{
	/// <summary>
	/// 配置值接口
	/// </summary>
	/// <remarks>
	///	形如下面的XML节表示一个配置节：
	///		<code>
	///			&lt;app my="myProperty"&gt;myValue&lt;/app&gt;
	///		</code>
	///	此时，<c>Name="app"</c>，Value的值为"myValue"，Property的值为"myProperty"
	/// </remarks>
	public interface ISettingValue : ICloneable, IConverting
	{
		/// <summary>
		/// 当前配置值是否只读
		/// </summary>
		bool ReadOnly { get; }

		/// <summary>
		/// 配置值名
		/// </summary>
		string Name { get; }

		/// <summary>
		/// 配置值
		/// </summary>
		string Value { get; }

		/// <summary>
		///  克隆配置值
		/// </summary>
		/// <param name="readonly">是否只读</param>
		/// <returns>ISettingValue</returns>
		ISettingValue Clone(bool @readonly);
	}
}
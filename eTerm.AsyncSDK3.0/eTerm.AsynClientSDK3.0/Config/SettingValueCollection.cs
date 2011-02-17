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



namespace eTerm.ASynClientSDK.Config
{
	/// <summary>
	/// 配置值集合
	/// </summary>
	public class SettingValueCollection : CollectionBase<SettingValue>
	{
		/// <summary>
		/// 构造方法
		/// </summary>
		public SettingValueCollection() : base(true) {}

		/// <summary>
		/// 添加配置值
		/// </summary>
		/// <param name="value">配置值</param>
		/// <returns>配置值</returns>
		public virtual SettingValue Add(SettingValue value) {
			this.Add(value.Name, value);
			return value;
		}

		/// <summary>
		/// 添加/替换配置值（如果存在则替换）
		/// </summary>
		/// <param name="value">配置值</param>
		public virtual void Set(SettingValue value) {
			this.Set(value.Name, value);
		}
	}
}
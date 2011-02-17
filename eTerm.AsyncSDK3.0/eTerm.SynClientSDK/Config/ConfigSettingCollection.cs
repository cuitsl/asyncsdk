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



using eTerm.SynClientSDK.Utils;
namespace eTerm.SynClientSDK.Config
{
	/// <summary>
	/// 配置节集合
	/// </summary>
	public class ConfigSettingCollection : CollectionBase<ConfigSetting>
	{
		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="uniqueKey">键值是否唯一</param>
		public ConfigSettingCollection(bool uniqueKey) : base(uniqueKey) { }

		/// <summary>
		/// 添加配置节
		/// </summary>
		/// <param name="setting">配置节</param>
		/// <returns>配置节</returns>
		public virtual ConfigSetting Add(ConfigSetting setting) {
			this.Add(setting.Name, setting);
			return setting;
		}

		/// <summary>
		/// 添加/替换配置节（如果存在则替换）
		/// </summary>
		/// <param name="setting">配置节</param>
		public virtual ConfigSetting Set(ConfigSetting setting) {
			ConvertUtil.StringToEnum<ConfigSettingOperator>("");
			this.Set(setting.Name, setting);
			return setting;
		}

		/// <summary>
		/// 深度复制集合
		/// </summary>
		/// <returns>复制后的集合</returns>
		public virtual ConfigSettingCollection Clone() {
			ConfigSettingCollection collection = new ConfigSettingCollection(this.UniqueKey);
			foreach(ConfigSetting setting in this.Values) {
				collection.Add(setting.Clone()).Parent = setting.Parent;
			}
			return collection;
		}
	}
}
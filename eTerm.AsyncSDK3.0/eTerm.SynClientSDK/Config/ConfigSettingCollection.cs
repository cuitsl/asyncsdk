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
	/// ���ýڼ���
	/// </summary>
	public class ConfigSettingCollection : CollectionBase<ConfigSetting>
	{
		/// <summary>
		/// ���췽��
		/// </summary>
		/// <param name="uniqueKey">��ֵ�Ƿ�Ψһ</param>
		public ConfigSettingCollection(bool uniqueKey) : base(uniqueKey) { }

		/// <summary>
		/// ������ý�
		/// </summary>
		/// <param name="setting">���ý�</param>
		/// <returns>���ý�</returns>
		public virtual ConfigSetting Add(ConfigSetting setting) {
			this.Add(setting.Name, setting);
			return setting;
		}

		/// <summary>
		/// ���/�滻���ýڣ�����������滻��
		/// </summary>
		/// <param name="setting">���ý�</param>
		public virtual ConfigSetting Set(ConfigSetting setting) {
			ConvertUtil.StringToEnum<ConfigSettingOperator>("");
			this.Set(setting.Name, setting);
			return setting;
		}

		/// <summary>
		/// ��ȸ��Ƽ���
		/// </summary>
		/// <returns>���ƺ�ļ���</returns>
		public virtual ConfigSettingCollection Clone() {
			ConfigSettingCollection collection = new ConfigSettingCollection(this.UniqueKey);
			foreach(ConfigSetting setting in this.Values) {
				collection.Add(setting.Clone()).Parent = setting.Parent;
			}
			return collection;
		}
	}
}
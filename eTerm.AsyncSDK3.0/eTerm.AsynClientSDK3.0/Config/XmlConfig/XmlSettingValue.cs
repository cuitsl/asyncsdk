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

using System.Web;
using System.Xml;

namespace eTerm.ASynClientSDK.Config.XmlConfig
{
	/// <summary>
	/// 使用XML实现<see cref="SettingValue"/>
	/// </summary>
	public class XmlSettingValue : SettingValue
	{
		#region constructor

		/// <summary>
		/// 使用name/value的形式初始化
		/// </summary>
		/// <param name="name">配置值名</param>
		/// <param name="value">配置值</param>
		/// <param name="readonly">是否只读</param>
		public XmlSettingValue(string name, string @value, bool @readonly) : base(name, @value, @readonly) {}

		/// <summary>
		/// 使用XmlNode初始化
		/// </summary>
		/// <param name="xmlNode">XmlNode</param>
		/// <param name="readonly">是否只读</param>
		public XmlSettingValue(XmlNode xmlNode, bool @readonly) : base(null, null, @readonly) {
			this.name = xmlNode.Name;
			foreach(XmlNode node in xmlNode.ChildNodes) {
				if(node.NodeType == XmlNodeType.Text) {
					this.value = node.Value;
					break;
				}
			}
		}

		#endregion

		/// <summary>
		/// 创建配置值实例
		/// </summary>
		/// <param name="name">配置值名</param>
		/// <param name="value">配置值</param>
		/// <param name="readonly">是否只读</param>
		/// <returns>SettingValue</returns>
		protected override SettingValue CreateSettingValue(string name, string value, bool @readonly) {
			return new XmlSettingValue(name, value, @readonly);
		}

		/// <summary>
		/// 转换成字符串格式
		/// </summary>
		/// <returns>字符串</returns>
		public override string ToString() {
			return string.Format("{0}=\"{1}\"", this.Name, HttpUtility.HtmlAttributeEncode(this.Value));
		}
	}
}
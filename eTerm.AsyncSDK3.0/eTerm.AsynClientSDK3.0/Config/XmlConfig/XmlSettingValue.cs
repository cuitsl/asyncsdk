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
	/// ʹ��XMLʵ��<see cref="SettingValue"/>
	/// </summary>
	public class XmlSettingValue : SettingValue
	{
		#region constructor

		/// <summary>
		/// ʹ��name/value����ʽ��ʼ��
		/// </summary>
		/// <param name="name">����ֵ��</param>
		/// <param name="value">����ֵ</param>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		public XmlSettingValue(string name, string @value, bool @readonly) : base(name, @value, @readonly) {}

		/// <summary>
		/// ʹ��XmlNode��ʼ��
		/// </summary>
		/// <param name="xmlNode">XmlNode</param>
		/// <param name="readonly">�Ƿ�ֻ��</param>
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
		/// ��������ֵʵ��
		/// </summary>
		/// <param name="name">����ֵ��</param>
		/// <param name="value">����ֵ</param>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <returns>SettingValue</returns>
		protected override SettingValue CreateSettingValue(string name, string value, bool @readonly) {
			return new XmlSettingValue(name, value, @readonly);
		}

		/// <summary>
		/// ת�����ַ�����ʽ
		/// </summary>
		/// <returns>�ַ���</returns>
		public override string ToString() {
			return string.Format("{0}=\"{1}\"", this.Name, HttpUtility.HtmlAttributeEncode(this.Value));
		}
	}
}
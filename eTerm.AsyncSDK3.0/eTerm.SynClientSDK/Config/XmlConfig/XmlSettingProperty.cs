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

using System.Text;
using System.Xml;

namespace eTerm.SynClientSDK.Config.XmlConfig
{
	/// <summary>
	/// ʹ��XMLʵ�����ý����ԣ�<see cref="SettingProperty"/>
	/// </summary>
	public class XmlSettingProperty : SettingProperty
	{
		#region constructor

		/// <summary>
		/// ��ʼ��
		/// </summary>
		/// <param name="properties">���Լ���</param>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		protected XmlSettingProperty(SettingValueCollection properties, bool @readonly) : base(properties, @readonly) {}

		/// <summary>
		/// ��ʼ��
		/// </summary>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		public XmlSettingProperty(bool @readonly) : base(new SettingValueCollection(), @readonly) { }

		/// <summary>
		/// ʹ��XmlNode��ʼ��
		/// </summary>
		/// <param name="xmlNode">XmlNode</param>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		public XmlSettingProperty(XmlNode xmlNode, bool @readonly) : this(@readonly) {
			this.InitData(xmlNode, @readonly);
		}

		#endregion

		#region private members

		/// <summary>
		/// ��ʼ��XML���
		/// </summary>
		private void InitData(XmlNode xmlNode, bool @readonly) {
			foreach(XmlNode attribute in xmlNode.Attributes) {
				string name = attribute.Name;
				string @value = attribute.Value;
				this.properties.Set(new XmlSettingValue(name, @value, @readonly));
			}
		}

		#endregion

		/// <summary>
		/// ������������ʵ��
		/// </summary>
		/// <param name="properties">���Լ���</param>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <returns>SettingProperty</returns>
		protected override SettingProperty CreateSettingProperty(SettingValueCollection properties, bool @readonly) {
			return new XmlSettingProperty(properties, @readonly);
		}

		/// <summary>
		/// ��������ֵ
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
			StringBuilder sb = new StringBuilder();
			foreach(SettingValue value in this.properties.Values) {
				sb.AppendFormat(" {0}", value);
			}
			if(sb.Length > 0) {
				sb.Remove(0, 1);
			}
			return sb.ToString();
		}
	}
}
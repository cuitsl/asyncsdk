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
	/// ���ýڽӿ�
	/// </summary>
	/// <remarks>
	///	���������XML�ڱ�ʾһ�����ýڣ�
	///		<code>
	///			&lt;app my="myProperty"&gt;myValue&lt;/app&gt;
	///		</code>
	///	��ʱ��<c>Name="app"</c>��Value��ֵΪ"myValue"��Property��ֵΪ"myProperty"
	/// </remarks>
	public interface IConfigSetting : ICloneable
	{
		/// <summary>
		/// ��ǰ���ý��Ƿ�ֻ��
		/// </summary>
		bool ReadOnly { get; }

		/// <summary>
		/// �����ýڵ���
		/// </summary>
		string Name { get; }

		/// <summary>
		/// �����ý�ʵ������
		/// </summary>
		string SettingName { get; }

		/// <summary>
		/// �����ýڵ���/ֵ
		/// </summary>
		ISettingValue Value { get; }

		/// <summary>
		/// ���������ýڵĸ����ý�
		/// </summary>
		IConfigSetting Parent { get; }

		/// <summary>
		/// �����ýڰ����������ý���Ŀ
		/// </summary>
		int Children { get; }

		/// <summary>
		/// ���ý�����
		/// </summary>
		ISettingProperty Property { get; }

		/// <summary>
		/// ��ȡ�����ý�
		/// </summary>
		/// <param name="childSettingName">�����ý���</param>
		/// <remarks>
		/// ��������ڣ�������<c>null</c>
		/// </remarks>
		IConfigSetting this[string childSettingName] { get; }

		/// <summary>
		/// ��ȡ�����ý�
		/// </summary>
		/// <param name="childSettingIndex">�����ý�˳��</param>
		/// <remarks>
		/// ��������ڣ�������null
		/// </remarks>
		IConfigSetting this[int childSettingIndex] { get; }

		/// <summary>
		/// ��ȡ���������ý�
		/// </summary>
		/// <returns>���ý�����</returns>
		IConfigSetting[] GetChildSettings();

		/// <summary>
		/// ��XPath��ʽ��ȡ���ý�
		/// </summary>
		/// <param name="xpath">XPath</param>
		/// <returns>���ý�</returns>
		/// <remarks>
		/// XPathΪ����XML��XPath������<c>framework/modules"</c><br />
		/// �������ͬ�����ýڣ��򷵻ص�һ�����ý�
		/// </remarks>
		IConfigSetting GetChildSetting(string xpath);

		/// <summary>
		/// ���༶��ʽ��ȡ���ý�
		/// </summary>
		/// <param name="settingName">�༶�����ý���</param>
		/// <returns>���ý�</returns>
		/// <remarks>
		/// �༶�����ý������������������ã�
		///		<code>
		///			&lt;app1&gt;
		///				&lt;app2&gt;
		///					&lt;app3&gt;&lt;/app3&gt;
		///				&lt;/app2&gt;
		///			&lt;/app1&gt;
		///		</code>
		///	��˳���룬����<c>GetChildSetting("app1", "app2", "app3")</c>����ʱ������Ϊ<c>app3</c>�����ý�
		/// </remarks>
		IConfigSetting GetChildSetting(params string[] settingName);

		/// <summary>
		/// ��ȡ�����ý�
		/// </summary>
		/// <returns>���ý�</returns>
		IConfigSetting GetRootSetting();

		/// <summary>
		/// �ϲ����ý�
		/// </summary>
		/// <param name="setting">���ϲ������ý�</param>
		void Merge(IConfigSetting setting);

		/// <summary>
		/// ��¡�����ý�
		/// </summary>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <param name="deep">�Ƿ����εĿ�¡</param>
		/// <returns>���ý�</returns>
		IConfigSetting Clone(bool @readonly, bool deep);
	}
}

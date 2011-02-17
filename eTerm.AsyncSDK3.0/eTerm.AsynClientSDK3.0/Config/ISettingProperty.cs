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
	/// ���ý����Խӿ�
	/// </summary>
	/// <remarks>
	///	���������XML�ڱ�ʾһ�����ýڣ�
	///		<code>
	///			&lt;app my="myProperty"&gt;myValue&lt;/app&gt;
	///		</code>
	///	��ʱ��<c>Name="app"</c>��Value��ֵΪ"myValue"��Property��ֵΪ"myProperty"
	/// </remarks>
	public interface ISettingProperty : ICloneable
	{
		/// <summary>
		/// ��ǰ���ý������Ƿ�ֻ��
		/// </summary>
		bool ReadOnly { get; }

		/// <summary>
		/// ���ýڵ����Ը���
		/// </summary>
		int Count { get; }

		/// <summary>
		/// ��ȡ����ֵ(����������)
		/// </summary>
		/// <param name="propertyName">������</param>
		ISettingValue this[string propertyName] { get; }

		/// <summary>
		/// ��ȡ����ֵ(������������)
		/// </summary>
		/// <param name="propertyIndex">��������</param>
		ISettingValue this[int propertyIndex] { get; }

		/// <summary>
		/// ���Ի�ȡĳ����ֵ
		/// </summary>
		/// <param name="propertyName">������</param>
		/// <returns>����ֵ</returns>
		string TryGetPropertyValue(string propertyName);

		/// <summary>
		/// ���Ի�ȡĳ����ֵ��ת����ָ������
		/// </summary>
		/// <typeparam name="T">ת����ָ��������</typeparam>
		/// <param name="propertyName">������</param>
		/// <returns>ָ�����͵�ʵ��</returns>
		T TryGetPropertyValue<T>(string propertyName);

		/// <summary>
		/// ���Ի�ȡĳ����ֵ��ת����ָ������
		/// </summary>
		/// <typeparam name="T">ת����ָ��������</typeparam>
		/// <param name="propertyName">������</param>
		/// <param name="defaultValue">ȱʡֵ</param>
		/// <returns>ָ�����͵�ʵ��</returns>
		T TryGetPropertyValue<T>(string propertyName, T defaultValue);

		/// <summary>
		/// ��¡����
		/// </summary>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <param name="deep">�Ƿ���ȸ���</param>
		/// <returns>ISettingProperty</returns>
		ISettingProperty Clone(bool @readonly, bool deep);
	}
}
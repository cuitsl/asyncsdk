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
	/// ����ֵ�ӿ�
	/// </summary>
	/// <remarks>
	///	���������XML�ڱ�ʾһ�����ýڣ�
	///		<code>
	///			&lt;app my="myProperty"&gt;myValue&lt;/app&gt;
	///		</code>
	///	��ʱ��<c>Name="app"</c>��Value��ֵΪ"myValue"��Property��ֵΪ"myProperty"
	/// </remarks>
	public interface ISettingValue : ICloneable, IConverting
	{
		/// <summary>
		/// ��ǰ����ֵ�Ƿ�ֻ��
		/// </summary>
		bool ReadOnly { get; }

		/// <summary>
		/// ����ֵ��
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ����ֵ
		/// </summary>
		string Value { get; }

		/// <summary>
		///  ��¡����ֵ
		/// </summary>
		/// <param name="readonly">�Ƿ�ֻ��</param>
		/// <returns>ISettingValue</returns>
		ISettingValue Clone(bool @readonly);
	}
}
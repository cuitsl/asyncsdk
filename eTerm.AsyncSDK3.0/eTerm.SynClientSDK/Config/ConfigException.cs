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

namespace eTerm.SynClientSDK.Config
{
	/// <summary>
	/// �����쳣
	/// </summary>
	/// <remarks>
	/// ���������棬�ܷ��ֵ��쳣�����װ�ɴ����ʵ��
	/// </remarks>
	[Serializable]
    public class ConfigException : System.Exception
	{
		/// <summary>
		/// ���캯��
		/// </summary>
		public ConfigException() : base() {
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="message">�쳣��Ϣ</param>
		/// <param name="innerException">�ڲ��쳣</param>
        public ConfigException(string message, System.Exception innerException)
			: base(message, innerException) {
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="message">�쳣��Ϣ</param>
		public ConfigException(string message)
			: base(message) {
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="message">�쳣��Ϣ</param>
		public ConfigException(int errorNo, string message)
			: base(message) {
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="message">�쳣��Ϣ</param>
		/// <param name="innerException">�ڲ��쳣</param>
        public ConfigException(int errorNo, string message, System.Exception innerException)
			: base(message, innerException) {
		}
	}
}

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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using eTerm.SynClientSDK.Config.XmlConfig;

namespace eTerm.SynClientSDK.Config
{
	/// <summary>
	/// �������õ�һЩʵ�÷���
	/// </summary>
	public static class ConfigHelper
	{
		/// <summary>
		/// ȱʡ��XML�����ļ�����Ŀ¼�б�
		/// </summary>
		public static readonly string[] ConfigFileDefaultSearchPath = {
			"./", "./../", "./../../", "./../../../",
			"./../Configuration/", "./../../Configuration/", "./../../../Configuration/",
			Environment.CurrentDirectory + "/",
			AppDomain.CurrentDomain.SetupInformation.ApplicationBase
		};

		/// <summary>
		/// ��֤�����ļ�·��������
		/// </summary>
		/// <param name="fileName">�����ļ���</param>
		/// <param name="searchPath">����Ŀ¼�б�</param>
		/// <returns>���������ļ�·��</returns>
		public static string SearchConfigFile(string fileName, string[] searchPath) {
			if(File.Exists(fileName)) {
				return fileName;
			}
			if(searchPath == null || searchPath.Length <= 0) {
				searchPath = ConfigFileDefaultSearchPath;
			}
			foreach(string filePath in searchPath) {
				string fullName = Path.GetFullPath(filePath + fileName);
				if(File.Exists(fullName)) {
					return fullName;
				}
			}
			return null;
		}

		/// <summary>
		/// ������ͨ���ָ�����ļ�����
		/// </summary>
		/// <param name="filePattern">�ļ�ͨ���</param>
		/// <param name="searchPath">����Ŀ¼�б�</param>
		/// <returns>�ҵ����ļ��б�</returns>
		public static string[] SearchConfigFileWithPattern(string filePattern, string[] searchPath) {
			if(searchPath == null || searchPath.Length <= 0) {
				searchPath = ConfigFileDefaultSearchPath;
			}
			List<string> foundFils = new List<string>();
			foreach(string filePath in searchPath) {
				string fullPath = Path.GetFullPath(filePath);
				if(Directory.Exists(fullPath)) {
					string[] files = Directory.GetFiles(fullPath, filePattern, SearchOption.TopDirectoryOnly);
					foundFils.AddRange(files);
				}
			}
			return foundFils.ToArray();
		}

		/// <summary>
		/// ��Xml�ַ��������� <see cref="IConfigSetting"/>
		/// </summary>
		/// <param name="xmlString">Xml�ַ���</param>
		/// <returns><see cref="IConfigSetting"/></returns>
		public static IConfigSetting CreateFromXmlString(string xmlString) {
			XmlNode xmlNode = LoadXmlNodeFromString(xmlString, "/");
			if(xmlNode is XmlDocument) {
				xmlNode = ((XmlDocument)xmlNode).DocumentElement;
			}
			return XmlConfigSetting.Create(null, xmlNode, true, null, null);
		}

		/// <summary>
		/// ��Xml�ļ������� <see cref="IConfigSetting"/>
		/// </summary>
		/// <param name="xmlFileName">Xml�ļ�</param>
		/// <returns><see cref="IConfigSetting"/></returns>
		public static IConfigSetting CreateFromXmlFile(string xmlFileName) {
			return XmlConfigSetting.Create(xmlFileName);
		}

		/// <summary>
		/// �� <see cref="XmlNode"/> ���� <see cref="IConfigSetting"/>
		/// </summary>
		/// <param name="xmlNode"><see cref="XmlNode"/></param>
		/// <returns><see cref="IConfigSetting"/></returns>
		public static IConfigSetting CreateFromXmlNode(XmlNode xmlNode) {
			return XmlConfigSetting.Create(null, xmlNode, true, null, null);
		}

		/// <summary>
		/// ����Դ��Uri�������� <see cref="IConfigSetting"/>
		/// </summary>
		/// <param name="xmlSource">Uri�ַ���</param>
		/// <param name="sourceInType">�������Ƕ��Դ���ڵĳ���</param>
		/// <returns><see cref="IConfigSetting"/></returns>
		public static IConfigSetting CreateFromXmlSource(string xmlSource, Type sourceInType) {
			IConfigSetting setting = null;
			if(xmlSource.StartsWith("res://", true, null)) {
				string sourceName = xmlSource.Substring(6);
				Assembly assembly = sourceInType.Assembly;
				Stream stream = assembly.GetManifestResourceStream(sourceName);
				if(stream == null) {
					throw new ConfigException("δ�ҵ���Դ" + xmlSource);
				}
				StreamReader sr = new StreamReader(stream);
				string xmlString = sr.ReadToEnd();
				setting = CreateFromXmlString(xmlString);
			} else if(xmlSource.StartsWith("http://", true, null)) {
				throw new ConfigException("δʵ��http://");
			} else {
				setting = CreateFromXmlFile(xmlSource);
			}
			return setting;
		}

		/// <summary>
		/// ��ȡXML�ļ�������
		/// </summary>
		/// <param name="fileName">XML�ļ���</param>
		/// <param name="sectionName">��Ӧ��XPath</param>
		/// <param name="rawType">�Ƿ񲻽����κ�ת��������</param>
		/// <returns>XmlNode</returns>
		public static XmlNode LoadXmlNodeFromFile(string fileName, string sectionName, bool rawType) {
			XmlDocument doc = new XmlDocument();
			LoadXmlFile(doc, fileName);
			XmlNode xmlNode = doc.SelectSingleNode(sectionName);
			if(xmlNode != null) {
				xmlNode = xmlNode.CloneNode(true);
			}
			if(!rawType && xmlNode is XmlDocument) {
				xmlNode = ((XmlDocument)xmlNode).DocumentElement;
			}
			return xmlNode;
		}

		/// <summary>
		/// ����XML�ַ�������
		/// </summary>
		/// <param name="xmlString">XML�ַ���</param>
		/// <param name="sectionName">��Ӧ��XPath</param>
		/// <returns>XmlNode</returns>
		public static XmlNode LoadXmlNodeFromString(string xmlString, string sectionName) {
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlString);
			XmlNode xmlNode = doc.SelectSingleNode(sectionName);
			if(xmlNode != null) {
				return xmlNode.CloneNode(true);
			}
			return xmlNode;
		}

		/// <summary>
		/// ����XML�ļ�����
		/// </summary>
		/// <param name="doc">XmlDocument</param>
		/// <param name="fileName">�ļ���</param>
		private static void LoadXmlFile(XmlDocument doc, string fileName) {
			doc.Load(fileName);
		}
	}
}
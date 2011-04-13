using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using eTerm.AsyncSDK.Util;
using System.Xml.Serialization;

namespace eTerm.AsyncSDK.Base {
    /// <summary>
    /// 二进制序列化基类
    /// </summary>
    /// <typeparam name="T">继承类型</typeparam>
    [Serializable]
    public abstract class BaseBinary<T>
        where T:BaseBinary<T>,new ()
    {
        /// <summary>
        /// 把对象序列化并返回相应的字节
        /// </summary>
        /// <param name="pObj">需要序列化的对象</param>
        /// <returns>byte[]</returns>
        public byte[] SerializeObject(T pObj) {
            if (pObj == null)
                return null;
            System.IO.MemoryStream _memory = new System.IO.MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(_memory, pObj);
            _memory.Position = 0;
            byte[] read = new byte[_memory.Length];
            _memory.Read(read, 0, read.Length);
            _memory.Close();
            return read;
        }

        /// <summary>
        /// 将对像本身序列化成XML串.
        /// </summary>
        /// <param name="Keys">密钥.</param>
        /// <returns></returns>
        public byte[] XmlSerialize(byte[] Keys) {
            StringBuilder sb = new StringBuilder(1024);
            StringWriter sw = new StringWriter(sb);
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(sw, this);
            //return sb.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", string.Empty).Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", string.Empty).Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty).ToString();
            return Keys==null||Keys.Length==0?Encoding.UTF8.GetBytes(sb.ToString()): new TEACrypter().Encrypt(Encoding.UTF8.GetBytes(sb.ToString()), Keys);
        }

        /// <summary>
        /// XMLs the serialize.
        /// </summary>
        /// <param name="Keys">The keys.</param>
        /// <param name="pathInfo">The path info.</param>
        /// <returns></returns>
        public byte[] XmlSerialize(byte[] Keys, string pathInfo) {
            lock (this) {
                //if (File.Exists(pathInfo))
                //    File.Delete(pathInfo);
                byte[] buffer = XmlSerialize(Keys);

                using (FileStream fs = new FileStream(pathInfo, FileMode.OpenOrCreate)) {
                    fs.SetLength(0);
                    fs.Flush();
                    BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8);

                    bw.Write(buffer);
                    bw.Flush();
                    bw.Close();
                }
                return buffer;
            }
        }

        /// <summary>
        /// Des the XML serialize.
        /// </summary>
        /// <param name="Keys">The keys.</param>
        /// <param name="Buffer">The buffer.</param>
        /// <returns></returns>
        public T DeXmlSerialize(byte[] Keys, byte[] Buffer) {
            StreamReader sr = null;
            try {
                using (MemoryStream ms = new MemoryStream(Keys==null||Keys.Length==0?Buffer: new TEACrypter().Decrypt( Buffer,Keys))) {
                    sr = new StreamReader(ms);
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    object obj = serializer.Deserialize(sr);
                    sr.Close();
                    return (T)obj;
                }
            }
            finally {
                if (sr != null)
                    try { sr.Close(); }
                    catch (Exception) { }
            }
        }

        /// <summary>
        /// Serializes to file.
        /// </summary>
        /// <param name="FilePath">The file path.</param>
        public void SerializeToFile(string FilePath) {
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate)) {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(SerializeObject(this as T));
                bw.Flush();
                bw.Close();
            }
        }

        /// <summary>
        /// Serializes to file.
        /// </summary>
        /// <param name="FilePath">The file path.</param>
        /// <param name="Keys">The keys.</param>
        public void SerializeToFile(string FilePath, byte[] Keys) {
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate)) {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(new TEACrypter().Encrypt(SerializeObject(this as T),Keys));
                bw.Flush();
                bw.Close();
            }
        }

        /// <summary>
        /// 把字节反序列化成相应的对象
        /// </summary>
        /// <param name="pBytes">字节流</param>
        /// <returns>object</returns>
        public T DeserializeObject(byte[] pBytes) {
            object _newOjb = null;
            if (pBytes == null)
                return null;
            using (System.IO.MemoryStream _memory = new System.IO.MemoryStream(pBytes)) {
                _memory.Position = 0;
                BinaryFormatter formatter = new BinaryFormatter();
                _newOjb = formatter.Deserialize(_memory);
            }
            return (T)_newOjb;
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="pBytes">The p bytes.</param>
        /// <param name="Keys">The keys.</param>
        /// <returns></returns>
        public T DeserializeObject(byte[] pBytes,byte[] Keys) {
            T o = new T();
            using (MemoryStream ms = new MemoryStream(pBytes)) {
                ms.Position = 0;
                BinaryFormatter formatter = new BinaryFormatter();
                o=(T) formatter.Deserialize(ms);
            }
            return o;
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="Keys">The keys.</param>
        /// <returns></returns>
        public T DeserializeObject(string filePath, byte[] Keys) {
            using (FileStream fs = new FileStream(filePath, FileMode.Open)) {
                BinaryReader br = new BinaryReader(fs);
                byte[] buffer = new byte[fs.Length];
                br.Read(buffer, 0, buffer.Length);
                br.Close();
                return DeserializeObject(new TEACrypter().Decrypt( buffer, Keys));
            }
        }
    }
}

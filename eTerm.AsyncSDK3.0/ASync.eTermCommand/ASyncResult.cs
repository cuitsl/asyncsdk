using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
namespace ASync.eTermCommand {
    /// <summary>
    /// 结果基类
    /// </summary>
    [Serializable]
    public abstract class ASyncResult {
        /// <summary>
        /// Gets or sets the A syn CMD.
        /// </summary>
        /// <value>The A syn CMD.</value>
        [XmlIgnore]
        public string ASynCmd { set; get; }


        /// <summary>
        /// 将对像本身序列化成XML串.
        /// </summary>
        /// <returns></returns>
        public string XmlSerialize() {
            StringBuilder sb = new StringBuilder(1024);
            StringWriter sw = new StringWriter(sb);
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(sw, this);
            return sb.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", string.Empty).Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", string.Empty).Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty).ToString();
        }
    }
}

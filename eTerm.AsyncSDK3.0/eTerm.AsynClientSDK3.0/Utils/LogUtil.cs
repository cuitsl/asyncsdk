using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace eTerm.ASynClientSDK.Utils {
    /// <summary>
    /// 记录日志的实用类
    /// </summary>
    public static class LogUtil {
        private static object lockObject = new object();

        /// <summary>
        /// 向指定文件添加文本信息
        /// </summary>
        /// <param name="logPath">所在路径</param>
        /// <param name="fileName">文本文件名（支持DateTime格式）</param>
        /// <param name="msgs">信息列表</param>
        public static void WriteLog(string logPath, string fileName, params object[] msgs) {
            fileName = string.Format("{0}\\{1}", logPath, DateTime.Now.ToString(fileName));
            WriteLog(fileName, msgs);
        }

        /// <summary>
        /// 向指定文件添加文本信息
        /// </summary>
        /// <param name="fileName">全路径文本文件名</param>
        /// <param name="msgs">信息列表</param>
        public static void WriteLog(string fileName, params object[] msgs) {
            FileInfo file = new FileInfo(fileName);
            if (!file.Directory.Exists) {
                Directory.CreateDirectory(file.Directory.FullName);
            }
            if (msgs == null || msgs.Length == 0) {
                return;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < msgs.Length; i++) {
                sb.AppendFormat("{{{0}}}\r\n", i);
            }
            string messageFormat = sb.ToString();
            lock (lockObject) {
                File.AppendAllText(fileName, string.Format(messageFormat, msgs));
            }
        }

        /// <summary>
        /// 向指定文件添加文本信息
        /// </summary>
        /// <param name="fileName">全路径文本文件名</param>
        /// <param name="message">文本信息</param>
        public static void WriteLog(string fileName, string message) {
            WriteLog(fileName, (object)message);
        }
    }
}

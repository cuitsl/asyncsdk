using System;
using System.Collections.Generic;

using System.Text;
using System.Security.Cryptography;

namespace eTerm.SynClientSDK.Utils {
    /// <summary>
    /// 随机数实用类
    /// </summary>
    public static class RandomUtil {
        /// <summary>
        /// 获取随机字节序列
        /// </summary>
        /// <param name="length">字节序列的长度</param>
        /// <returns>字节序列</returns>
        public static byte[] GetRandomBytes(int length) {
            if (length <= 0) {
                return new byte[0];
            }
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] ret = new byte[length];
            rng.GetNonZeroBytes(ret);
            return ret;
        }

        /// <summary>
        /// 缺省的字符串取值范围
        /// </summary>
        public const string DEFAULT_CHARLIST = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
        /// <summary>
        /// 可读的字符串取值范围
        /// </summary>
        public const string READ_CHARLIST = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="charList">字符串取值范围（如果为Null或为空，则返回空字符串）</param>
        /// <returns>随机字符串</returns>
        public static string GetRandomString(int length, string charList) {
            //if (length <= 0 || Checker.CheckEmptyString("charList", charList, false)) {
            //    return string.Empty;
            //}
            int num = charList.Length;
            char[] ret = new char[length];
            byte[] rnd = GetRandomBytes(length);
            for (int i = 0; i < rnd.Length; i++) {
                ret[i] = charList[rnd[i] % num];
            }
            return new string(ret);
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns>随机字符串</returns>
        /// <remarks>
        /// 缺省使用ASCII从33到126共94个字符作为取值范围
        /// </remarks>
        public static string GetRandomString(int length) {
            return GetRandomString(length, DEFAULT_CHARLIST);
        }
    }
}

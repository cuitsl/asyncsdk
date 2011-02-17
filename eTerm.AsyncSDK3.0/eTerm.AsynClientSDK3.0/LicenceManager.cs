using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using eTerm.ASynClientSDK.Utils;
using System.Threading;
using System.IO;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 服务器授权主体
    /// </summary>
    public sealed class LicenceManager {
        private static readonly LicenceManager __instance = new LicenceManager();
        /// <summary>
        /// 执行代理
        /// </summary>
        private delegate bool ExecuteValidate();
        private ExecuteValidate _Execute;
        private bool __flag = false;
        private byte[] __AsyncKey;

        /// <summary>
        /// 授权号（使用官方自定义加密算法生成）.
        /// <remarks>
        ///     由于采用Byte数组，因此授权方式采用授权文件导入
        /// </remarks>
        /// </summary>
        /// <value>The async license.</value>
        public byte[] AsyncLicense { set; private get; }

        /// <summary>
        /// 授权包.
        /// </summary>
        /// <value>The async licence.</value>
        //public AsyncLicenceKey LicenceBody { get; private set; }

        /// <summary>
        /// Gets the cpu SN.
        /// </summary>
        /// <returns></returns>
        private string GetCpuSN() {
            ManagementClass cimobject = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = cimobject.GetInstances();
            StringBuilder sb = new StringBuilder();
            foreach (ManagementObject mo in moc) {
                sb.Append(mo.Properties["ProcessorId"].Value.ToString().Replace(":", string.Empty).Replace(" ", string.Empty));
            }
            //sb.Append("1qaz@WSX3edc");
            return sb.ToString();
        }

        /// <summary>
        /// 单例不可继承.
        /// </summary>
        /// <value>The instance.</value>
        public static LicenceManager Instance { get { return __instance; } }

        /// <summary>
        /// 授权认证结果.
        /// </summary>
        /// <value><c>true</c> if [validate result]; otherwise, <c>false</c>.</value>
        public bool ValidateResult { get { return this.__flag; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenceManager"/> class.
        /// </summary>
        private LicenceManager() {
            _Execute = new ExecuteValidate(ValidateNet);
        }


        /// <summary>
        /// Validates the net.
        /// </summary>
        /// <returns></returns>
        public bool ValidateNet() {
            __flag = true;
            string cpuCode = GetCpuSN();
#if DEBUG
            using (FileStream fs = new FileStream(@"C:\AsyncSDK3.0.Bin", FileMode.OpenOrCreate)) {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(cpuCode);
                sw.Flush();
                sw.Close();
            }
#endif
            __AsyncKey = TEACrypter.MD5(Encoding.Default.GetBytes(cpuCode));
            //开如处理授权码
            //LicenceBody = new AsyncLicenceKey().DeXmlSerialize(__AsyncKey,AsyncLicense);
#if DEBUG
            //if (LicenceBody != null) {
            //    Console.WriteLine(LicenceBody.Company);
            //}
            //else {
            //    Console.WriteLine("DeserializeObject Error");
            //    return false;
            //}
#endif 
            //if (LicenceBody.ExpireDate < DateTime.Now) __flag = false;
            //if (Encoding.UTF8.GetString(new TEACrypter().Decrypt( LicenceBody.Key,__AsyncKey)) != cpuCode) __flag = false;
            return __flag;
        }


        /// <summary>
        /// 开启异步授权认证.
        /// </summary>
        /// <param name="callBack">授权回调方法.</param>
        /// <returns></returns>
        public IAsyncResult BeginValidate(AsyncCallback callBack) {
            try {
                return _Execute.BeginInvoke(callBack, __flag);
            }
            catch (System.Exception e) {
                // Hide inside method invoking stack 
                throw e;
            }
        }

        /// <summary>
        /// 异步回调结束.
        /// </summary>
        /// <param name="iar">The iar.</param>
        /// <returns></returns>
        public bool EndValidate(IAsyncResult iar) {
            if (iar == null)
                throw new NullReferenceException();
            try {
                bool flag = _Execute.EndInvoke(iar);
                return flag;
            }
            catch (System.Exception e) {
                // Hide inside method invoking stack 
                throw e;
            }
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using eTerm.AsyncSDK.Util;
using System.Threading;
using System.IO;

namespace eTerm.AsyncSDK {
    /// <summary>
    /// 服务器授权主体
    /// </summary>
    public sealed class LicenceManager {
        private static readonly LicenceManager __instance = new LicenceManager();
        /// <summary>
        /// 执行代理
        /// </summary>
        private delegate bool ExecuteValidate(string Identification);
        private ExecuteValidate _Execute;
        //private Timer __KeyAsync;
        //private const long __KeyInterval = 1000 * 60 * 5;
        private bool __flag = false;
        private string __identification = string.Empty;
        private string __serialNumber = string.Empty;
        private byte[] __secreteKey = null;
        /// <summary>
        /// 授权包.
        /// </summary>
        /// <value>The async licence.</value>
        public AsyncLicenceKey LicenceBody { get; private set; }

        /// <summary>
        /// 机器码.
        /// </summary>
        /// <value>The serial number.</value>
        public string SerialNumber { get { return __serialNumber; } }

        /// <summary>
        /// Gets the secrete key.
        /// </summary>
        /// <value>The secrete key.</value>
        internal byte[] SecreteKey { get { return __secreteKey; } }

        /// <summary>
        /// Gets the authorization file.
        /// </summary>
        /// <value>The authorization file.</value>
        internal string AuthorizationFile { get { return __identification; } }

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
            /*
            __KeyAsync = new Timer(
new TimerCallback(
        delegate(object sender)
        {
            LicenceBody.RemainingMinutes -= __KeyInterval / (1000 * 60);
            LicenceBody.XmlSerialize(__secreteKey, __identification);
        }),
    null, __KeyInterval, __KeyInterval);
           */
        }


        /// <summary>
        /// Validates the net.
        /// </summary>
        /// <returns></returns>
        public bool ValidateNet(string Identification) {
            __flag = true;
            byte[] buffer;
            LicenceBody=new AsyncLicenceKey();
            try {
                __serialNumber = GetCpuSN();
                __identification = Identification;
                __secreteKey = TEACrypter.MD5(Encoding.Default.GetBytes(string.Format(@"{0}{1}", __serialNumber, @"3048ljLKJ337204YLuF47381&36!$**(@")));
                using (FileStream fs = new FileStream(Identification, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    BinaryReader br = new BinaryReader(fs);
                    buffer = new byte[fs.Length];
                    br.Read(buffer, 0, buffer.Length);
                    LicenceBody = LicenceBody.DeXmlSerialize(__secreteKey, buffer);
                    __flag = CompareBytes(new TEACrypter().Decrypt(LicenceBody.Key, __secreteKey), Encoding.Default.GetBytes(string.Format(@"{0}{1}", __serialNumber, @"3048ljLKJ337204YLuF47381&36!$**(@")));
                    __flag =__flag&& LicenceBody.ExpireDate >= DateTime.Now;
                    __flag = __flag && LicenceBody.RemainingMinutes > 0;
                }
            }
            catch {
                __flag = false;
            }
            return __flag;
        }

        /// <summary>
        /// Compares the bytes.
        /// </summary>
        /// <param name="sourceBytes">The source bytes.</param>
        /// <param name="targetBytes">The target bytes.</param>
        /// <returns></returns>
        private bool CompareBytes(byte[] sourceBytes, byte[] targetBytes) {
            if (sourceBytes.Length != targetBytes.Length) return false;
            int index = 0;
            do {
                if (sourceBytes[index] != targetBytes[index]) return false;
            } while (++index < sourceBytes.Length);
            return true;
        }


        /// <summary>
        /// 开启异步授权认证.
        /// </summary>
        /// <param name="callBack">授权回调方法.</param>
        /// <param name="Identification">The identification.</param>
        /// <returns></returns>
        public IAsyncResult BeginValidate(AsyncCallback callBack, string Identification) {
            try {
                return _Execute.BeginInvoke(Identification,callBack,__flag );
            }
            catch (Exception e) {
                // Hide inside method invoking stack 
                __flag = false;
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
            catch (Exception e) {
                // Hide inside method invoking stack 
                throw e;
            }
        } 
    }
}

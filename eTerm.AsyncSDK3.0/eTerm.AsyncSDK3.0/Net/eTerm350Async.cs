using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eTerm.AsyncSDK.Net {
    /// <summary>
    /// 非安全连接通信体
    /// </summary>
    public sealed class eTerm350Async : eTerm443Async {
        /// <summary>
        /// Initializes a new instance of the <see cref="eTerm350Async"/> class.
        /// </summary>
        public eTerm350Async():base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="eTerm350Async"/> class.
        /// </summary>
        /// <param name="Ip">The ip.</param>
        /// <param name="Port">The port.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userPass">The user pass.</param>
        /// <param name="Sid">The sid.</param>
        /// <param name="Rid">The rid.</param>
        public eTerm350Async(string Ip, int Port, string userName, string userPass, byte Sid, byte Rid)
            :base(Ip,Port,userName,userPass,Sid,Rid)
        {
            base.IsSsl = false;
        }
    }
}

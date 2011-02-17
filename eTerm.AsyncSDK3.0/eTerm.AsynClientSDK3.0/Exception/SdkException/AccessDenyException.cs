using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK.Exception {
    /// <summary>
    /// 访问被拒绝，客户端配置错误或客户端不具备当前配置下的访问权限
    /// </summary>
    public class AccessDenyException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessDenyException"/> class.
        /// </summary>
        public AccessDenyException() : base("访问被拒绝，客户端配置错误或客户端不具备当前配置下的访问权限，请检查您的用户名或密码！") { }
    }
}

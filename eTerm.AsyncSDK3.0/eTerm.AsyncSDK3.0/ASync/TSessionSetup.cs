using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace eTerm.AsyncSDK {

    /// <summary>
    /// 客户配置
    /// </summary>
    [Serializable]
    public sealed class TSessionSetup : BaseBinary<TSessionSetup> {
        private string __Regex = string.Empty;
        /// <summary>
        /// Initializes a new instance of the <see cref="TSessionSetup"/> class.
        /// </summary>
        public TSessionSetup() {
            this.TSessionForbidCmd = new List<string>();
            this.Traffics = new List<SocketTraffic>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSessionSetup"/> class.
        /// </summary>
        /// <param name="sessionCode">The session code.</param>
        public TSessionSetup(string sessionCode) : this() {
            this.SessionCode = sessionCode;    
        }

        /// <summary>
        /// Gets or sets the session code.
        /// </summary>
        /// <value>The session code.</value>
        public string SessionCode { get; set; }

        /// <summary>
        /// Gets or sets the group code.
        /// </summary>
        /// <value>The group code.</value>
        public string GroupCode { get; set; }

        /// <summary>
        /// Gets or sets the session pass.
        /// </summary>
        /// <value>The session pass.</value>
        public string SessionPass { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the session expire.
        /// </summary>
        /// <value>The session expire.</value>
        public int SessionExpire { get; set; }

        /// <summary>
        /// Gets or sets the flow rate.
        /// </summary>
        /// <value>The flow rate.</value>
        public float FlowRate { get; set; }

        /// <summary>
        /// 是否允许重复登录.
        /// </summary>
        /// <value>The allow duplicate.</value>
        public bool? AllowDuplicate { get; set; }

        /// <summary>
        /// Gets or sets the traffics.
        /// </summary>
        /// <value>The traffics.</value>
        public List<SocketTraffic> Traffics { get; set; }

        /// <summary>
        /// 到期日期.
        /// </summary>
        /// <value>The expire date.</value>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        /// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
        public bool IsOpen { get; set; }

        /// <summary>
        /// 正规表达式 .
        /// </summary>
        /// <value>The forbid CMD reg.</value>
        public string ForbidCmdReg {
            get {
                if (!string.IsNullOrEmpty(this.__Regex)) return this.__Regex;
                StringBuilder sb = new StringBuilder();
                foreach (string cmd in this.TSessionForbidCmd) {
                    sb.AppendFormat("|^{0}", cmd);
                }
                if (sb.Length > 0)
                    sb.Remove(0, 1);
                this.__Regex = sb.ToString();
                return this.__Regex;
            }
        }

        /// <summary>
        /// 特殊指令时限设置，格式为:^AV|10,^SS|20.
        /// </summary>
        /// <value>The special interval list.</value>
        public string SpecialIntervalList {
            get;
            set;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj) {
            return (obj as TSessionSetup).SessionCode == this.SessionCode;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return this.SessionCode;
        }

        /// <summary>
        /// 禁用指令集合（不区分大小写）.
        /// </summary>
        /// <value>The T session forbid CMD.</value>
        public List<string> TSessionForbidCmd { get; set; }
    }
}

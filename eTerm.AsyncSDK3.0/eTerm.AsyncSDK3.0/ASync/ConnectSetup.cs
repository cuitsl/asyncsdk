using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace eTerm.AsyncSDK {
    /// <summary>
    /// 分组类
    /// </summary>
    [Serializable]
    public sealed class SDKGroup : BaseBinary<SDKGroup> {
        /// <summary>
        /// Gets or sets the group code.
        /// </summary>
        /// <value>The group code.</value>
        public string groupCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        public string groupName { get; set; }

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
            return (obj as SDKGroup).groupCode == this.groupCode;
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
            return this.groupName;
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [Serializable]
    public sealed class ConnectSetup : BaseBinary<ConnectSetup> {

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectSetup"/> class.
        /// </summary>
        public ConnectSetup() { this.Traffics = new List<SocketTraffic>(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectSetup"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="userName">Name of the user.</param>
        public ConnectSetup(string address, string userName):this() {
            this.Address = address;
            this.userName = userName;
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the office code.
        /// </summary>
        /// <value>The office code.</value>
        public string OfficeCode { get; set; }

        /// <summary>
        /// Gets or sets the group code.
        /// </summary>
        /// <value>The group code.</value>
        public string GroupCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is SSL.
        /// </summary>
        /// <value><c>true</c> if this instance is SSL; otherwise, <c>false</c>.</value>
        public bool IsSsl { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string userName { get; set; }

        /// <summary>
        /// Gets or sets the user pass.
        /// </summary>
        /// <value>The user pass.</value>
        public string userPass { get; set; }

        /// <summary>
        /// Gets or sets the SID.
        /// </summary>
        /// <value>The SID.</value>
        public byte SID { get; set; }

        /// <summary>
        /// Gets or sets the RID.
        /// </summary>
        /// <value>The RID.</value>
        public byte RID { get; set; }

        /// <summary>
        /// Gets or sets the si text.
        /// </summary>
        /// <value>The si text.</value>
        public string SiText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        /// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Gets or sets the traffics.
        /// </summary>
        /// <value>The traffics.</value>
        public List<SocketTraffic> Traffics { get; set; }

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
            return 
                (obj as ConnectSetup).userName == this.userName
                &&
                (obj as ConnectSetup).Address==this.Address
                ;
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
            return this.userName;
        }
    }
}

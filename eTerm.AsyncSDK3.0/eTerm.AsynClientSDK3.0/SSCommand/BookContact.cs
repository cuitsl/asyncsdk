using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;
using System.Runtime.Serialization;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 本类代表一个PNR联系组信息
    /// <remarks>
    /// 联系组的功能是记录各种联系信息，方便查询代理人及旅客信息
    /// </remarks>
    /// </summary>
    [DataContract]
    public class BookContact:ASyncResult {

        /// <summary>
        /// Initializes a new instance of the <see cref="BookContact"/> class.
        /// </summary>
        public BookContact() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookContact"/> class.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <param name="contact">The contact.</param>
        /// <param name="psgrName">Name of the PSGR.</param>
        public BookContact(string city, string contact, string psgrName) {
            this.getcity = city;
            this.getcontact = contact;
            this.psgrName = psgrName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookContact"/> class.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <param name="contact">The contact.</param>
        public BookContact(string city, string contact) : this(city, contact, string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookContact"/> class.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public BookContact(string contact)
            : this(string.Empty
                , contact) { }

        /// <summary>
        /// Gets or sets the getcity.
        /// </summary>
        /// <value>The getcity.</value>
        [DataMember]
        public string getcity { get; set; }

        /// <summary>
        /// 联系信息.
        /// </summary>
        /// <value>The getcontact.</value>
        [DataMember]
        public string getcontact { get; set; }

        /// <summary>
        /// 旅客姓名.
        /// </summary>
        /// <value>The name of the PSGR.</value>
        [DataMember]
        public string psgrName { get; set; }

        #region 重写
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return this.getcontact;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }
        #endregion
    }
}

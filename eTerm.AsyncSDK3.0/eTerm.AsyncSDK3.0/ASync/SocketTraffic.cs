using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace eTerm.AsyncSDK {
    /// <summary>
    /// 流量记数
    /// </summary>
    public sealed class SocketTraffic : BaseBinary<SocketTraffic> {

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketTraffic"/> class.
        /// </summary>
        public SocketTraffic() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketTraffic"/> class.
        /// </summary>
        /// <param name="monthString">The month string.</param>
        public SocketTraffic(string monthString) : this() {
            this.MonthString = monthString;
        }

        /// <summary>
        /// 月份，格式为：201101.
        /// </summary>
        /// <value>The month string.</value>
        public string MonthString { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>The update date.</value>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the traffic.
        /// </summary>
        /// <value>The traffic.</value>
        public double? Traffic { get; set; }

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
            return (obj as SocketTraffic).MonthString == this.MonthString;
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
            return this.MonthString;
        }
    }
}

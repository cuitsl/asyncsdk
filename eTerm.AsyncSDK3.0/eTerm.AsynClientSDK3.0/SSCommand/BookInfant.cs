using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 本类包含一个订座婴儿组信息 
    /// </summary>
    public class BookInfant:ASyncResult {

        /// <summary>
        /// 构造函数（该婴儿的生日，携带该婴儿的旅客在旅客组中的姓名，该婴儿的姓名）
        /// </summary>
        /// <param name="birth">The birth.</param>
        /// <param name="carrier">The carrier.</param>
        /// <param name="name">The name.</param>
        public BookInfant(DateTime birth, string carrier, string name) {
            this.getbirth = birth;
            this.getcarrierName = carrier;
            this.getname = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookInfant"/> class.
        /// </summary>
        public BookInfant() { }

        /// <summary>
        /// 该婴儿的生日.
        /// </summary>
        /// <value>The getbirth.</value>
        public DateTime getbirth { get; set; }

        /// <summary>
        ///  携带该婴儿的旅客姓名.
        /// </summary>
        /// <value>The name of the carrier.</value>
        public string getcarrierName { get; set; }

        /// <summary>
        /// 该婴儿的姓名 .
        /// </summary>
        /// <value>The getname.</value>
        public string getname { get; set; }


        #region 重写
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return this.getname;
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

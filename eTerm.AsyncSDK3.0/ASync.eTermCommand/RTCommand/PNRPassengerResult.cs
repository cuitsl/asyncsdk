using System;
using System.Collections.Generic;
using System.Text;
using ASync.eTermCommand;

namespace ASync.eTermCommand {
    /// <summary>
    /// 本类包含一个PNR旅客组的信息。旅客有成人、儿童和无陪伴儿童三种类别。 对于不同类别的旅客，该类中的有效字段个数会有所区别。比如，成人旅客 和儿童旅客没有年龄信息，这时相应的提取函数返回空值或者0值。
    /// </summary>
    public class PNRPassengerResult:ASyncResult {

        /// <summary>
        /// Initializes a new instance of the <see cref="PNRPassengerResult"/> class.
        /// </summary>
        public PNRPassengerResult() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PNRPassengerResult"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="age">The age.</param>
        /// <param name="type">The type.</param>
        public PNRPassengerResult(string name,int age,PASSENGERTYPE type) {
            this.getAge = age;
            this.getName = name;
            this.getType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PNRPassengerResult"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public PNRPassengerResult(string name) : this(name, 0, PASSENGERTYPE.PASSENGER_ADULT) { }

        /// <summary>
        /// 获取该旅客的年龄.
        /// </summary>
        /// <value>The get age.</value>
        public int getAge { get; set; }

        /// <summary>
        /// 获取该旅客的姓名.
        /// </summary>
        /// <value>The name of the get.</value>
        public string getName { get; set; }

        /// <summary>
        /// 获取该旅客的类别.
        /// </summary>
        /// <value>The type of the get.</value>
        public PASSENGERTYPE getType { get; set; }
    }

    #region 乘客类型
    /// <summary>
    /// 乘客类型
    /// </summary>
    public enum PASSENGERTYPE : int {
        /// <summary>
        /// 表示一个成人旅客
        /// </summary>
        PASSENGER_ADULT=1,
        /// <summary>
        /// 表示一个儿童旅客
        /// </summary>
        PASSENGER_CHILD=2,
        /// <summary>
        /// 表示一个无陪伴儿童
        /// </summary>
        PASSENGER_CHILD_UNACCOMPANIED =3
    }
    #endregion

}

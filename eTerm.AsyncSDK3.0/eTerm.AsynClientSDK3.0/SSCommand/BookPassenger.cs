using System;
using System.Collections.Generic;
using System.Text;
using eTerm.ASynClientSDK;
using System.Runtime.Serialization;

namespace eTerm.ASynClientSDK {

    /// <summary>
    /// 本类包含一个PNR旅客组的信息
    /// <code>
    /// 旅客有成人、儿童和无陪伴儿童三种类别。
    /// 对于不同类别的旅客，该类中的有效字段个数会有所区别。比如，成人旅客 和儿童旅客没有年龄信息，
    /// 这时相应的提取函数返回空值或者0值。
    /// </code>
    /// <![CDATA[
    /// 旅客姓名由英文字母或汉字组成；若输入英文字母的姓名，姓与名之间需用斜线（/）分开（中文姓名无此限制）；
    /// 旅客姓名均应由英文26字母组成，每个旅客姓名最多只能有1个斜线（/）；对于输入英文字母的姓名，姓不得少于两个字母；
    /// 旅客名单按照姓氏的字母顺序排序；旅客姓名长度最大为55字母；散客记录最大旅客数为9人
    /// 例如旅客姓名
    /// 英文（拼音）姓名: ZHANG/JIAN
    /// 中文姓名: 孙家浩
    /// ]]>
    /// </summary>
    [DataContract]
    public class BookPassenger : ASyncResult {
        /// <summary>
        /// 身份证件类型 NI身份证，CC信用卡，PP护照.
        /// </summary>
        /// <value>The type of the id.</value>
        [DataMember]
        public string IdType { get; internal set; }

        /// <summary>
        /// 对应的身份证件号码.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public string Id { get; internal set; }

        /// <summary>
        /// 对应旅客姓名 .
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string Name { get; internal set; }

        /// <summary>
        /// 该旅客的类别.
        /// </summary>
        /// <value>The type of the get.</value>
        [DataMember]
        public PASSENGERTYPE getType { get; internal set; }

        /// <summary>
        /// 该旅客的年龄.
        /// </summary>
        /// <value>The get age.</value>
        [DataMember]
        public int getAge { get; internal set; }

        /// <summary>
        /// 创建旅客.
        /// </summary>
        /// <param name="name">姓名.</param>
        /// <param name="id">证件号.</param>
        /// <param name="idType">证件类型.</param>
        /// <param name="age">年龄.</param>
        /// <param name="type">类型.</param>
        /// <returns></returns>
        private static BookPassenger createBookPassenger(string name, string id, string idType, int age, PASSENGERTYPE type) {
            return new BookPassenger() {getAge=age,getType=type,Id=id,Name=name,IdType=idType };
        }

        /// <summary>
        /// 建立成人旅客姓名.
        /// </summary>
        /// <param name="name">姓名.</param>
        /// <param name="idType">证件类型.</param>
        /// <param name="id">证件号.</param>
        /// <returns></returns>
        public static BookPassenger createAdult(string name, string idType, string id) {
            return createBookPassenger(name, id, idType, 0, PASSENGERTYPE.PASSENGER_ADULT);
        }

        /// <summary>
        /// 建立成人旅客姓名(默认使用NI证件类型).
        /// </summary>
        /// <param name="name">姓名.</param>
        /// <param name="id">证件号.</param>
        /// <returns></returns>
        public static BookPassenger createAdult(string name, string id) {
            return createAdult(name, "NI", id);
        }
    }
}

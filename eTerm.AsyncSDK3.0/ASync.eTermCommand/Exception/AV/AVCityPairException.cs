using System;
using System.Collections.Generic;
using System.Text;
using ASync.eTermCommand;

namespace ASync.eTermCommand.AVException {
    /// <summary>
    /// 给出的城市对不正确
    /// </summary>
    public class AVCityPairException:SdkException {
        /// <summary>
        /// Initializes a new instance of the <see cref="AVCityPairException"/> class.
        /// </summary>
        public AVCityPairException() : base("给出的城市对不正确") { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// Sdk所有Exception的基类
    /// </summary>
    public abstract class SdkException:System.Exception {
        private string __message = string.Empty;
        private int __errorNumber = 0;


        /// <summary>
        /// Initializes a new instance of the <see cref="SdkException"/> class.
        /// </summary>
        public SdkException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdkException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SdkException(string message) : this(message, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdkException"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        protected SdkException(int number) : this(string.Empty, number) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdkException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="number">The number.</param>
        public SdkException(string message,int number) {
            this.__errorNumber = number;
            this.__message = message;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message {
            get {
                return this.__message;
            }
        }
    }
}

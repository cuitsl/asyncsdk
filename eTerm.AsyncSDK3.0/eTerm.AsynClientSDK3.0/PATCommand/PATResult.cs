using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using eTerm.ASynClientSDK.Base;
using eTerm.ASynClientSDK;
using System.Linq;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// PATResult
    /// </summary>
    public sealed class PATResult:ASyncResult {
        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public string Sequence { get; set; }

        /// <summary>
        /// Gets or sets the cabin string.
        /// </summary>
        /// <value>The cabin string.</value>
        public string CabinString { get; set; }

        /// <summary>
        /// Gets or sets the cabin fare.
        /// </summary>
        /// <value>The cabin fare.</value>
        public float CabinFare { get; set; }

        /// <summary>
        /// Gets or sets the cabin tax.
        /// </summary>
        /// <value>The cabin tax.</value>
        public float CabinTax { get; set; }

        /// <summary>
        /// Gets or sets the cabin YQ.
        /// </summary>
        /// <value>The cabin YQ.</value>
        public float CabinYQ { get; set; }

        /// <summary>
        /// Gets or sets the cabin total fare.
        /// </summary>
        /// <value>The cabin total fare.</value>
        public float CabinTotalFare { get; set; }
    }
}

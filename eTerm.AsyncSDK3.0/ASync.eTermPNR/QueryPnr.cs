using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASync.MiddleWare;

namespace ASync.eTermPNR {
    public partial class QueryPnr : BaseAddIn {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPnr"/> class.
        /// </summary>
        public QueryPnr() {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        public override string ButtonName {
            get { return "PNR管理组件"; }
        }

        /// <summary>
        /// Gets the image icon.
        /// </summary>
        /// <value>The image icon.</value>
        public override string ImageIcon {
            get { return "Hourglass.png"; }
        }

    }
}

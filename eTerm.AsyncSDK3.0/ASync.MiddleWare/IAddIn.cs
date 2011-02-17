using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK;

namespace ASync.MiddleWare {
    public interface IAddIn {
        /// <summary>
        /// Gets the image icon.
        /// </summary>
        /// <value>The image icon.</value>
        string ImageIcon { get; }

        /// <summary>
        /// Sets the A sync setup.
        /// </summary>
        /// <value>The A sync setup.</value>
        AsyncLicenceKey ASyncSetup { set; }

        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        string ButtonName { get; }
    }
}

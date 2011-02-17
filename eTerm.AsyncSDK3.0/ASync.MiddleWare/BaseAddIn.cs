using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using eTerm.AsyncSDK;

namespace ASync.MiddleWare {
    public abstract class BaseAddIn : UserControl, IAddIn {
        /// <summary>
        /// Gets the image icon.
        /// </summary>
        /// <value>The image icon.</value>
        public abstract string ImageIcon { get; }

        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        public abstract string ButtonName { get; }

        /// <summary>
        /// Gets or sets the A sync setup.
        /// </summary>
        /// <value>The A sync setup.</value>
        public AsyncLicenceKey ASyncSetup { set; protected get; }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e) {
            if (ASyncSetup == null) throw new PlatformNotSupportedException(string.Format("未发现授权文件，导入授权后再使用本插件！机器码为：{0}",LicenceManager.Instance.SerialNumber));
            if (ASyncSetup.ExpireDate <= DateTime.Now) throw new PlatformNotSupportedException(string.Format(@"授权已经到期，导入新授权后再使用本插件！{0}",LicenceManager.Instance.SerialNumber));
            base.OnLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e) {
            this.Dock = DockStyle.Fill;
            base.OnPaint(e);
        }

    }
}

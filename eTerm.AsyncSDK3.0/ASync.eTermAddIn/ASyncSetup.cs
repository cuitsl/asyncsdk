using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using eTerm.AsyncSDK;

namespace ASync.eTermAddIn {
    public partial class ASyncSetup : UserControl {
        public ASyncSetup() {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the btnPlugInPath control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnPlugInPath_Click(object sender, EventArgs e) {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                AsyncStackNet.Instance.ASyncSetup.PlugInPath = folderBrowserDialog1.SelectedPath;
                txtPlugInPath.Text=folderBrowserDialog1.SelectedPath;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e) {
            AsyncStackNet.Instance.ASyncSetup.AllowPlugIn = chkAllowPlugIn.Checked;
            AsyncStackNet.Instance.ASyncSetup.ExternalPort = txtPort.Value;
            AsyncStackNet.Instance.ASyncSetup.AutoReconnect = chkReconnect.Checked;
            AsyncStackNet.Instance.ASyncSetup.MaxReconnect = integerInput1.Value;
            AsyncStackNet.Instance.ASyncSetup.StatisticalFrequency = txtMaxReconnect.Value;
        }
    }
}

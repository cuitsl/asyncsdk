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
            try {
                AsyncStackNet.Instance.ASyncSetup.AllowPlugIn = chkAllowPlugIn.Checked;
                AsyncStackNet.Instance.ASyncSetup.ExternalPort = txtPort.Value;
                AsyncStackNet.Instance.ASyncSetup.AutoReconnect = chkReconnect.Checked;
                AsyncStackNet.Instance.ASyncSetup.MaxReconnect = integerInput1.Value;
                AsyncStackNet.Instance.ASyncSetup.StatisticalFrequency = txtMaxReconnect.Value;
                AsyncStackNet.Instance.ASyncSetup.XmlSerialize(AsyncStackNet.Instance.CrypterKey, AsyncStackNet.Instance.ASyncSetupFile);
                MessageBox.Show(@"系统配置保存成功，将在下次启动时起效！", @"系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) {
                MessageBox.Show(string.Format(@"发生系统错误：{0}",ex.Message), @"系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

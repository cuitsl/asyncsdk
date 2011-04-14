using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASync.MiddleWare;
using eTerm.AsyncSDK.Util;
using eTerm.AsyncSDK;

namespace ASync.eTermBuilder {
    public partial class SerialBuilder : BaseAddIn {
        public SerialBuilder() {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e) {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                this.txtFileName.Text = saveFileDialog1.FileName;
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the PickerExpire control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PickerExpire_ValueChanged(object sender, EventArgs e) {
            this.txtMinutes.Text = string.Format(@"{0}", ((TimeSpan)(PickerExpire.Value - DateTime.Now)).TotalMinutes);
        }

        /// <summary>
        /// Handles the Click event of the btnBuilder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnBuilder_Click(object sender, EventArgs e) {
            try {
                TEACrypter Crypter = new TEACrypter();
                byte[] keys = TEACrypter.MD5(Encoding.Default.GetBytes(string.Format(@"{0}{1}", this.txtCode.Text, @"3048ljLKJ337204YLuF47381&36!$**(@")));
                byte[] Result = Crypter.Encrypt(Encoding.Default.GetBytes(this.txtCode.Text), keys);




                AsyncLicenceKey Key = new AsyncLicenceKey() {
                    Company = txtCompany.Text,
                    ExpireDate = this.PickerExpire.Value,
                    Key = Result,
                    MaxAsync = int.Parse(txtASyncCount.Value.ToString()),
                    MaxTSession = int.Parse(txtMaxTSession.Value.ToString()),
                    AllowDatabase = chkAllowDb.Checked,
                    MaxCommandPerMonth = int.Parse(maskedTextBox1.Text),
                    connectionString = this.txtConnectString.Text,
                    providerName = this.txtProviderName.Text,
                    AllowAfterValidate = true,
                    AllowIntercept = chkAllowInter.Checked,
                    RemainingMinutes = ((TimeSpan)(this.PickerExpire.Value - DateTime.Now)).TotalMinutes
                };

                byte[] Buffer = Key.XmlSerialize(keys, this.txtFileName.Text);
                MessageBox.Show(string.Format(@"授权文件{0}已经生成！", txtFileName.Text), "授权提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        public override string ButtonName {
            get { return "授权生成器"; }
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASync.MiddleWare;
using eTerm.AsyncSDK;
using System.IO;

namespace ASync.eTermAddIn {
    public partial class ASyncAuthorize : BaseAddIn {
        /// <summary>
        /// Initializes a new instance of the <see cref="ASyncAuthorize"/> class.
        /// </summary>
        public ASyncAuthorize() {
            InitializeComponent();
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        this.txtCompany.Text = LicenceManager.Instance.LicenceBody.Company;
                        //this.txtDbString.Text = LicenceManager.Instance.LicenceBody.connectionString;
                        this.txtExpire.Value = LicenceManager.Instance.LicenceBody.ExpireDate;
                        this.txtMaxASync.Value = LicenceManager.Instance.LicenceBody.MaxAsync;
                        this.txtMaxSession.Value = LicenceManager.Instance.LicenceBody.MaxTSession;
                        this.chkOpenDb.Checked = LicenceManager.Instance.LicenceBody.AllowDatabase;
                        this.txtCode.Text = LicenceManager.Instance.SerialNumber;
                        this.txtRemainMinutes.Value = LicenceManager.Instance.LicenceBody.RemainingMinutes;
                        this.checkBoxX1.Checked = LicenceManager.Instance.LicenceBody.AllowDbLog ?? false;
                    }
                );
        }

        /// <summary>
        /// Handles the Click event of the buttonX1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonX1_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                LicenceManager.Instance.BeginValidate(new AsyncCallback(
                    delegate(IAsyncResult iar)
                    {
                        try {
                            if (!LicenceManager.Instance.EndValidate(iar)) {
                                //Console.WriteLine("Validate Error");
                            }
                            else {
                                //激活配置
                                File.Copy(openFileDialog1.FileName, @"Key.Bin",true);
                                MessageBox.Show(string.Format(@"授权文件{0}已经导入，下次重启后将生效！",openFileDialog1.FileName), "授权提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.OnLoad(EventArgs.Empty);
                            }
                        }
                        catch (Exception ex) {
                            MessageBox.Show(ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }), openFileDialog1.FileName);
            }
        }

        
        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        public override string ButtonName {
            get { return "系统授权器"; }
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

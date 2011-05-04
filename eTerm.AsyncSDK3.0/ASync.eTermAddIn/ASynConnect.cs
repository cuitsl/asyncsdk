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
using eTerm.AsyncSDK.Util;

namespace ASync.eTermAddIn {
    public partial class ASynConnect : BaseAddIn {
        /// <summary>
        /// Initializes a new instance of the <see cref="ASyncSession"/> class.
        /// </summary>
        public ASynConnect() {
            InitializeComponent();
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        tabControl1.SelectedTab = tabItem1;
                        this.lstSession.Items.Clear();
                        foreach (ConnectSetup Setup in AsyncStackNet.Instance.ASyncSetup.AsynCollection) {
                            SDKGroup group = null;
                            if (AsyncStackNet.Instance.ASyncSetup.GroupCollection != null && !string.IsNullOrEmpty(Setup.GroupCode) && AsyncStackNet.Instance.ASyncSetup.GroupCollection.Contains(new SDKGroup() { groupCode = Setup.GroupCode }))
                                group = AsyncStackNet.Instance.ASyncSetup.GroupCollection[
                                    AsyncStackNet.Instance.ASyncSetup.GroupCollection.IndexOf(new SDKGroup() {groupCode=Setup.GroupCode })];
                            ListViewItem Item = new ListViewItem(new string[] {
                                Setup.userName,
                                string.Format(@"{0}:{1}",Setup.Address,Setup.Port.ToString()),
                                Setup.IsSsl.ToString(),
                                Setup.IsOpen?"正常":"停用",
                                Setup.SiText,
                                Setup.OfficeCode,
                                group==null?"未分组":group.groupName,
                                (Setup.TSessionType??CertificationType.Address)== CertificationType.Address?@"地址认证":@"用户认证",
                                Setup.ToString()
                            });
                            Item.Name = Setup.ToString();
                            Item.Tag = Setup;
                            this.lstSession.Items.Add(Item);
                        }
                    }
                );
        }

        /// <summary>
        /// Handles the ItemChecked event of the lstSession control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ItemCheckedEventArgs"/> instance containing the event data.</param>
        private void lstSession_ItemChecked(object sender, ItemCheckedEventArgs e) {

        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDelete_Click(object sender, EventArgs e) {
            if (MessageBox.Show("操作不可恢复，确实要继续吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                foreach (ListViewItem item in this.lstSession.Items) {
                    if (!item.Selected) continue;
                    AsyncStackNet.Instance.ASyncSetup.AsynCollection.Remove(item.Tag as ConnectSetup);
                }
                btnSave_Click(null, EventArgs.Empty);
                this.OnLoad(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e) {
            if (MessageBox.Show("操作不可恢复，重启程序后配置将生效！", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                AsyncStackNet.Instance.BeginRateUpdate(new AsyncCallback(delegate(IAsyncResult iar)
                {
                    AsyncStackNet.Instance.EndRateUpdate(iar);
                }));
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSessionEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSessionEdit_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in this.lstSession.Items) {
                if (!item.Selected) continue;
                ConnectSetup Setup = item.Tag as ConnectSetup;
                this.PanelSession.Tag = Setup;
                this.txtAddress.Text = Setup.Address;
                this.txtPassword.Text = Setup.userPass;
                this.txtPort.Value = Setup.Port;

                this.radAddress.Checked = (Setup.TSessionType ?? CertificationType.Password) == CertificationType.Address;
                if (this.radAddress.Checked) {
                    this.radAddress_CheckedChanged(null, EventArgs.Empty);

                }

                this.radPassword.Checked = (Setup.TSessionType ?? CertificationType.Password) == CertificationType.Password;
                if (this.radPassword.Checked) {
                    this.radPassword_CheckedChanged(null, EventArgs.Empty);

                }
                this.ipLocalIp.Value = Setup.LocalIp;
                this.txtOfficeCode.Text = Setup.OfficeCode;
                chkIsOpen.Checked = Setup.IsOpen;
                chkIsSsl.Checked = Setup.IsSsl;
                integerInput2.Value = int.Parse((Setup.FlowRate??0.0).ToString());
                this.txtSessionName.Text = Setup.userName;
                this.txtSIText.Text = Setup.SiText;
                this.textBoxX1.Text = String.Format("{0:X}", Setup.SID);
                this.textBoxX2.Text = String.Format("{0:X}", Setup.RID);
                //this.integerInput3.Value = (int)Setup.RID;
                chkAutoSi.Checked = Setup.AutoSi ?? false;
                comboBoxEx1.Items.Clear();
                if (AsyncStackNet.Instance.ASyncSetup.GroupCollection == null) return;
                foreach (SDKGroup group in AsyncStackNet.Instance.ASyncSetup.GroupCollection) {
                    comboBoxEx1.Items.Add(new { Text = group.groupName, Value = group.groupCode });
                }


                if (!string.IsNullOrEmpty(Setup.GroupCode))
                    foreach (object group in this.comboBoxEx1.Items) {
                        string GValue = group.GetType().GetProperty("Value").GetValue(group, null).ToString();
                        //string GText = group.GetType().GetProperty("Text").GetValue(group, null).ToString();
                        if (GValue == Setup.GroupCode)
                            comboBoxEx1.SelectedItem = group;
                    }

                PanelSession.Enabled = true;
                if (!AsyncStackNet.Instance.ASyncSetup.AsynCollection.Contains(new ConnectSetup() { SID=Setup.SID, RID=Setup.RID }))
                    return;
                this.comboTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                this.comboTree1.ValueMember = @"MonthString";
                this.comboTree1.DisplayMembers = @"MonthString,Traffic,UpdateDate";
                this.comboTree1.DataSource = AsyncStackNet.Instance.ASyncSetup.AsynCollection[
                    AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup() { SID = Setup.SID, RID = Setup.RID })].Traffics;
                tabControl1.SelectedTab = tabItem2;

                //PanelSession.Show();
                break;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnInsert control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnInsert_Click(object sender, EventArgs e) {
            txtSIText.Text = string.Empty;
            txtPort.Value = 443;
            txtPassword.Text = string.Empty;
            txtSessionName.Text = string.Empty;
            PanelSession.Tag = null;
            PanelSession.Enabled = true;
            txtSIText.Text = string.Empty;
            PanelSession.Tag = new ConnectSetup();
            comboBoxEx1.Items.Clear();
            if (AsyncStackNet.Instance.ASyncSetup.GroupCollection == null) return;
            foreach (SDKGroup group in AsyncStackNet.Instance.ASyncSetup.GroupCollection) {
                comboBoxEx1.Items.Add(new { Text = group.groupName, Value = group.groupCode });
            }
            tabControl1.SelectedTab = tabItem2;

        }

        /// <summary>
        /// Handles the Click event of the btnSingleSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSingleSave_Click(object sender, EventArgs e) {
            PanelSession.Enabled = false;

            string groupCode = string.Empty;
            if (comboBoxEx1.SelectedItem != null)
                groupCode = comboBoxEx1.SelectedItem.GetType().GetProperty("Value").GetValue(comboBoxEx1.SelectedItem, null).ToString();
            int Sid = Int32.Parse(textBoxX1.Text, System.Globalization.NumberStyles.HexNumber);
            int Rid = Int32.Parse(textBoxX2.Text, System.Globalization.NumberStyles.HexNumber);
            ConnectSetup Setup = new ConnectSetup()
            { 
                 LocalIp=ipLocalIp.Value,
                  TSessionType=radPassword.Checked?CertificationType.Password:CertificationType.Address,
                 Address=txtAddress.Text,
                  IsOpen =chkIsOpen.Checked,
                 userName = string.IsNullOrEmpty(txtSessionName.Text) ? StringUtil.GenUniqueString() : txtSessionName.Text,
                    IsSsl=chkIsSsl.Checked,
                     Port=txtPort.Value,
                       SiText=txtSIText.Text,
                 GroupCode = groupCode, AutoSi=chkAutoSi.Checked,
                        OfficeCode=txtOfficeCode.Text,
                         userPass=txtPassword.Text,
                 SID = (byte)Sid,
                 RID = (byte)Rid,
                           FlowRate=integerInput2.Value
            };
            Setup.Traffics = (PanelSession.Tag as ConnectSetup).Traffics;
            PanelSession.Tag = Setup;
            if (!AsyncStackNet.Instance.ASyncSetup.AsynCollection.Contains(Setup))
                AsyncStackNet.Instance.ASyncSetup.AsynCollection.Add(Setup);
            else{
                int indexOf = AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(this.PanelSession.Tag as ConnectSetup);
                AsyncStackNet.Instance.ASyncSetup.AsynCollection[indexOf] = Setup;
            }
            btnSave_Click(null, EventArgs.Empty);
            this.OnLoad(EventArgs.Empty);
        }

        /// <summary>
        /// Handles the Click event of the btnDispose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDispose_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in this.lstSession.Items) {
                if (!item.Selected) continue;
                ConnectSetup setup = item.Tag as ConnectSetup;
                AsyncStackNet.Instance.AppendAsync(
                    new eTerm.AsyncSDK.Net.eTerm443Async(
                        setup.Address,
                        setup.Port,
                        setup.userName,
                        setup.userPass,
                        setup.SID,
                        setup.RID) { SiText=setup.SiText, IsSsl=setup.IsSsl, GroupCode=setup.GroupCode, OfficeCode=setup.OfficeCode });
            }
        }


        
        
        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        public override string ButtonName {
            get { return "eTerm配置管理"; }
        }

        /// <summary>
        /// Gets the image icon.
        /// </summary>
        /// <value>The image icon.</value>
        public override string ImageIcon {
            get { return "Hourglass.png"; }
        }

        /// <summary>
        /// Handles the SelectedTabChanged event of the tabControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DevComponents.DotNetBar.TabStripTabChangedEventArgs"/> instance containing the event data.</param>
        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e) {
            if (PanelSession.Tag == null) return;
            ConnectSetup Setup = PanelSession.Tag as ConnectSetup;
            if (string.IsNullOrEmpty(Setup.userName) || AsyncStackNet.Instance.ASyncSetup.AsynCollection == null || AsyncStackNet.Instance.ASyncSetup.AsynCollection.Count == 0) return;
            if (AsyncStackNet.Instance.ASyncSetup.AsynCollection[
                AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup() { SID = Setup.SID, RID = Setup.RID })].Traffics == null)
                return;
            this.comboTree1.DataSource = AsyncStackNet.Instance.ASyncSetup.AsynCollection[
                AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup() { SID = Setup.SID, RID = Setup.RID })].Traffics;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstSession control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //private void lstSession_SelectedIndexChanged(object sender, EventArgs e) {
        //    if (lstSession.SelectedItems.Count != 1) {
        //        btnDispose.Enabled = false;
        //        btnInsert.Enabled = false;
        //        btnSessionEdit.Enabled = false;
        //        btnDelete.Enabled = false;
        //        return; 
        //    }
        //    btnDelete.Enabled = true;
        //    btnDispose.Enabled = true;
        //    btnInsert.Enabled = true;
        //    btnSessionEdit.Enabled = true;
        //    ConnectSetup Setup = lstSession.SelectedItems[0].Tag as ConnectSetup;
        //    if (string.IsNullOrEmpty(Setup.userName) || AsyncStackNet.Instance.ASyncSetup.AsynCollection == null || AsyncStackNet.Instance.ASyncSetup.AsynCollection.Count == 0) return;
        //    if (AsyncStackNet.Instance.ASyncSetup.AsynCollection[
        //        AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup() { SID = Setup.SID, RID = Setup.RID })].Traffics == null)
        //        return;
        //    this.comboTree1.DataSource = AsyncStackNet.Instance.ASyncSetup.AsynCollection[
        //        AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup() { SID = Setup.SID, RID = Setup.RID })].Traffics;
        //}

        /// <summary>
        /// Handles the Click event of the btnClearTraffic control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnClearTraffic_Click(object sender, EventArgs e) {
            ConnectSetup Setup = lstSession.SelectedItems[0].Tag as ConnectSetup;
            if(this.comboTree1.SelectedValue.ToString()==DateTime.Now.ToString(@"yyyyMM")){MessageBox.Show(@"当月流量统计不允许清除！",@"操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return;}
            List<SocketTraffic> Traffics= AsyncStackNet.Instance.ASyncSetup.AsynCollection[
                AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup() { SID = Setup.SID, RID = Setup.RID })].Traffics;
            Traffics.RemoveAt(Traffics.IndexOf(new SocketTraffic() { MonthString = this.comboTree1.SelectedValue.ToString() }));
            AsyncStackNet.Instance.BeginRateUpdate(new AsyncCallback(delegate(IAsyncResult iar)
            {
                AsyncStackNet.Instance.EndRateUpdate(iar);
                btnSessionEdit_Click(null, EventArgs.Empty);
            }));
        }

        /// <summary>
        /// Handles the CheckedChanged event of the radPassword control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void radPassword_CheckedChanged(object sender, EventArgs e) {
            labelX1.Visible = true;
            txtSessionName.Visible = true;
            labelX2.Visible = true;
            txtPassword.Visible = true;

            labelX15.Visible = false;
            ipLocalIp.Visible = false;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the radAddress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void radAddress_CheckedChanged(object sender, EventArgs e) {
            labelX1.Visible = false;
            txtSessionName.Visible = false;
            labelX2.Visible = false;
            txtPassword.Visible = false;

            labelX15.Visible = true;
            ipLocalIp.Visible = true;
        }

        /// <summary>
        /// Handles the DoubleClick event of the lstSession control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstSession_DoubleClick(object sender, EventArgs e) {
            btnSessionEdit_Click(null, EventArgs.Empty);
        }
        
    }
}

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
                                Setup.IsOpen.ToString(),
                                Setup.SiText,
                                Setup.OfficeCode,
                                group==null?"未分组":group.groupName
                            });
                            Item.Name = Setup.userName;
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
            int checkCount = 0;
            foreach (ListViewItem item in this.lstSession.Items) {
                checkCount = item.Checked ? checkCount + 1 : checkCount;
            }
            btnSessionEdit.Enabled = checkCount == 1;
            btnDelete.Enabled = checkCount > 0;
            btnDispose.Enabled = checkCount > 0;
            //btnInsert.Enabled = checkCount > 0;
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDelete_Click(object sender, EventArgs e) {
            if (MessageBox.Show("操作不可恢复，确实要继续吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                foreach (ListViewItem item in this.lstSession.Items) {
                    if (!item.Checked) continue;
                    AsyncStackNet.Instance.ASyncSetup.AsynCollection.Remove(new ConnectSetup() { userName=item.Name });
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
                AsyncStackNet.Instance.ASyncSetup.XmlSerialize(AsyncStackNet.Instance.CrypterKey, AsyncStackNet.Instance.ASyncSetupFile);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSessionEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSessionEdit_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in this.lstSession.Items) {
                if (!item.Checked) continue;
                ConnectSetup Setup = item.Tag as ConnectSetup;
                this.PanelSession.Tag = Setup;
                this.txtAddress.Value = Setup.Address;
                this.txtPassword.Text = Setup.userPass;
                this.txtPort.Value = Setup.Port;
                this.txtOfficeCode.Text = Setup.OfficeCode;
                chkIsOpen.Checked = Setup.IsOpen;
                chkIsSsl.Checked = Setup.IsSsl;
                this.txtSessionName.Text = Setup.userName;
                this.txtSIText.Text = Setup.SiText;
                this.integerInput1.Value =(int) Setup.SID;
                this.integerInput2.Value = (int)Setup.RID;

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
                if (!AsyncStackNet.Instance.ASyncSetup.AsynCollection.Contains(new ConnectSetup(Setup.Address,Setup.userName)))
                    return;
                this.comboTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                this.comboTree1.ValueMember = @"MonthString";
                this.comboTree1.DisplayMembers = @"MonthString,Traffic,UpdateDate";
                this.comboTree1.DataSource = AsyncStackNet.Instance.ASyncSetup.AsynCollection[
                    AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup(Setup.Address, Setup.userName))].Traffics;

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
            PanelSession.Show();
            comboBoxEx1.Items.Clear();
            if (AsyncStackNet.Instance.ASyncSetup.GroupCollection == null) return;
            foreach (SDKGroup group in AsyncStackNet.Instance.ASyncSetup.GroupCollection) {
                comboBoxEx1.Items.Add(new { Text = group.groupName, Value = group.groupCode });
            }


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

            ConnectSetup Setup = new ConnectSetup() { 
                 Address=txtAddress.Value,
                  IsOpen =chkIsOpen.Checked,
                   userName=txtSessionName.Text,
                    IsSsl=chkIsSsl.Checked,
                     Port=txtPort.Value,
                       SiText=txtSIText.Text,
                 GroupCode = groupCode,
                        OfficeCode=txtOfficeCode.Text,
                         userPass=txtPassword.Text,
                         SID=(byte)integerInput1.Value,
                          RID=(byte)integerInput2.Value
            };
            Setup.Traffics = (PanelSession.Tag as ConnectSetup).Traffics;
            if (this.PanelSession.Tag == null)
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
                if (!item.Checked) continue;
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
            if (AsyncStackNet.Instance.ASyncSetup.AsynCollection[
                AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup(Setup.Address, Setup.userName))].Traffics == null)
                return;
            this.comboTree1.DataSource = AsyncStackNet.Instance.ASyncSetup.AsynCollection[
                AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup(Setup.Address, Setup.userName))].Traffics;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstSession control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstSession_SelectedIndexChanged(object sender, EventArgs e) {
            if (lstSession.SelectedItems.Count != 1) return;
            ConnectSetup Setup = lstSession.SelectedItems[0].Tag as ConnectSetup;
            if (AsyncStackNet.Instance.ASyncSetup.AsynCollection[
                AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup(Setup.Address, Setup.userName))].Traffics == null)
                return;
            this.comboTree1.DataSource = AsyncStackNet.Instance.ASyncSetup.AsynCollection[
                AsyncStackNet.Instance.ASyncSetup.AsynCollection.IndexOf(new ConnectSetup(Setup.Address, Setup.userName))].Traffics;
        }
        
    }
}

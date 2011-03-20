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
using System.Text.RegularExpressions;
using System.Collections;

namespace ASync.eTermAddIn {
    public partial class ASyncSession : BaseAddIn {
        /// <summary>
        /// Initializes a new instance of the <see cref="ASyncSession"/> class.
        /// </summary>
        public ASyncSession() {
            InitializeComponent();
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        this.lstSession.Items.Clear();
                        foreach (TSessionSetup Setup in AsyncStackNet.Instance.ASyncSetup.SessionCollection) {
                            SDKGroup group = null;
                            if (AsyncStackNet.Instance.ASyncSetup.GroupCollection != null && !string.IsNullOrEmpty(Setup.GroupCode) && AsyncStackNet.Instance.ASyncSetup.GroupCollection.Contains(new SDKGroup() { groupCode = Setup.GroupCode }))
                                group = AsyncStackNet.Instance.ASyncSetup.GroupCollection[
                                    AsyncStackNet.Instance.ASyncSetup.GroupCollection.IndexOf(new SDKGroup() { groupCode = Setup.GroupCode })];
                            ListViewItem Item = new ListViewItem(new string[] {
                                Setup.SessionCode,
                                group==null?"未分组":group.groupName,
                                Setup.SessionExpire.ToString(),
                                Setup.FlowRate.ToString(),
                                Setup.ForbidCmdReg
                            });
                            Item.Name = Setup.SessionCode;
                            Item.Tag = Setup;
                            this.lstSession.Items.Add(Item);
                            this.comboTree1.Nodes.Clear();
                            comboBoxEx3.Items.Clear();
                        }
                    });
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
                    AsyncStackNet.Instance.ASyncSetup.SessionCollection.Remove(new TSessionSetup() { SessionCode=item.Name });
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
                TSessionSetup Setup = item.Tag as TSessionSetup;
                PanelSession.Tag = Setup;
                txtDescription.Text = Setup.Description;
                txtExpire.Value = Setup.SessionExpire;
                txtPassword.Text = Setup.SessionPass;
                txtSessionName.Text= Setup.SessionCode;
                chkIsOpen.Checked = Setup.IsOpen;
                txtFlow.Value =int.Parse( Setup.FlowRate.ToString());
                PanelSession.Tag = Setup;
                comboBoxEx3.Items.Clear();
                comboBoxEx1.Items.Clear();
                comboBoxEx3.ValueMember = "SessionCode";
                comboBoxEx3.DisplayMember = "Description";
                foreach (string Cmd in Setup.TSessionForbidCmd)
                    this.comboBoxEx1.Items.Add(Cmd);


                comboBoxEx2.Items.Clear();
                if (AsyncStackNet.Instance.ASyncSetup.GroupCollection == null) return;
                foreach (SDKGroup group in AsyncStackNet.Instance.ASyncSetup.GroupCollection) {
                    comboBoxEx2.Items.Add(new { Text = group.groupName, Value = group.groupCode });
                }
                if (!string.IsNullOrEmpty(Setup.SpecialIntervalList)) {
                    foreach (Match m in Regex.Matches(Setup.SpecialIntervalList, @"\^([A-Z0-9]+)\|(\d+)\,", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                        comboBoxEx3.Items.Add(new TSessionSetup() { SessionCode = m.Groups[1].Value, Description = string.Format(@"{1} {0}", m.Groups[2].Value, m.Groups[1].Value), SessionPass =m.Groups[2].Value });

                    }
                }

                if (!string.IsNullOrEmpty(Setup.GroupCode))
                    foreach (object group in this.comboBoxEx2.Items) {
                        string GValue = group.GetType().GetProperty("Value").GetValue(group, null).ToString();
                        //string GText = group.GetType().GetProperty("Text").GetValue(group, null).ToString();
                        if (GValue == Setup.GroupCode)
                            comboBoxEx2.SelectedItem = group;
                    }
                PanelSession.Enabled = true;
                if (!AsyncStackNet.Instance.ASyncSetup.SessionCollection.Contains(new TSessionSetup(Setup.SessionCode)))
                    return;
                this.comboTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                this.comboTree1.ValueMember = @"MonthString";
                this.comboTree1.DisplayMembers = @"MonthString,Traffic,UpdateDate";
                this.comboTree1.DataSource = AsyncStackNet.Instance.ASyncSetup.SessionCollection[
                    AsyncStackNet.Instance.ASyncSetup.SessionCollection.IndexOf(new TSessionSetup(Setup.SessionCode))].Traffics;
                //PanelSession.act
                break;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnInsert control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnInsert_Click(object sender, EventArgs e) {
            txtDescription.Text = string.Empty;
            txtExpire.Value = 10;
            txtFlow.Value = 100;
            txtPassword.Text = string.Empty;
            txtSessionName.Text = string.Empty;
            PanelSession.Tag = null;
            PanelSession.Enabled = true;
            PanelSession.Show();
            comboBoxEx2.Items.Clear();
            if (AsyncStackNet.Instance.ASyncSetup.GroupCollection == null) return;
            foreach (SDKGroup group in AsyncStackNet.Instance.ASyncSetup.GroupCollection) {
                comboBoxEx2.Items.Add(new { Text = group.groupName, Value = group.groupCode });
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
            if (comboBoxEx2.SelectedItem != null)
                groupCode = comboBoxEx2.SelectedItem.GetType().GetProperty("Value").GetValue(comboBoxEx2.SelectedItem, null).ToString();


            TSessionSetup Setup = new TSessionSetup() { 
                 Description=txtDescription.Text,
                  IsOpen =chkIsOpen.Checked,
                   SessionCode=txtSessionName.Text,
                 GroupCode = groupCode,
                    SessionExpire=txtExpire.Value,
                     SessionPass=txtPassword.Text,
                      FlowRate=float.Parse(this.txtFlow.Value.ToString()),
                 SpecialIntervalList =string.Empty
            };

            Setup.Traffics = (PanelSession.Tag as TSessionSetup).Traffics;

            foreach (TSessionSetup item in this.comboBoxEx3.Items) {
                Setup.SpecialIntervalList += string.Format(@"^{0}|{1},", item.SessionCode, item.SessionPass);
            }

            foreach (string Cmd in this.comboBoxEx1.Items) {
                Setup.TSessionForbidCmd.Add(Cmd);
            }
            if (this.PanelSession.Tag == null)
                AsyncStackNet.Instance.ASyncSetup.SessionCollection.Add(Setup);
            else{
                int indexOf = AsyncStackNet.Instance.ASyncSetup.SessionCollection.IndexOf(this.PanelSession.Tag as TSessionSetup);
                AsyncStackNet.Instance.ASyncSetup.SessionCollection[indexOf] = Setup;
            }
            btnSave_Click(null, EventArgs.Empty);
            this.OnLoad(EventArgs.Empty);
        }

        /// <summary>
        /// Handles the Click event of the btnAddSingle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAddSingle_Click(object sender, EventArgs e) {
            this.comboBoxEx1.Items.Add(this.comboBoxEx1.Text);
            this.comboBoxEx1.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteSingleCmd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDeleteSingleCmd_Click(object sender, EventArgs e) {
            if (!(this.comboBoxEx1.SelectedIndex >= 0)) return;
            this.comboBoxEx1.Items.RemoveAt(this.comboBoxEx1.SelectedIndex);
        }


        
        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        public override string ButtonName {
            get { return "eTerm帐号管理"; }
        }

        /// <summary>
        /// Gets the image icon.
        /// </summary>
        /// <value>The image icon.</value>
        public override string ImageIcon {
            get { return "Hourglass.png"; }
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteSpecial control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDeleteSpecial_Click(object sender, EventArgs e) {
            this.comboBoxEx3.Items.RemoveAt(this.comboBoxEx3.SelectedIndex);
        }

        /*
        /// <summary>
        /// Handles the Click event of the btnUpdateSpecial control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnUpdateSpecial_Click(object sender, EventArgs e) {
            if (!Regex.IsMatch(this.comboBoxEx3.Text, @"^([A-Z0-9]+)\|(\d+)", RegexOptions.Multiline | RegexOptions.IgnoreCase)) {
                MessageBox.Show(@"输入格式不正确，正确格式为：SS|25", @"格式错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Match m = Regex.Match(this.comboBoxEx3.Text, @"^([A-Z0-9]+)\|(\d+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            this.comboBoxEx3.Items[this.comboBoxEx3.SelectedIndex] = new { Interval = m.Groups[2].Value, Command = string.Format(@"{1}   {0}", m.Groups[2].Value, m.Groups[1].Value) };
        }
        */


        /// <summary>
        /// Handles the Click event of the btnNewSpecial control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnNewSpecial_Click(object sender, EventArgs e) {
            TSessionSetup setup = PanelSession.Tag as TSessionSetup;
            if (!Regex.IsMatch(this.comboBoxEx3.Text, @"^([A-Z0-9]+)\|(\d+)", RegexOptions.Multiline | RegexOptions.IgnoreCase)) {
                MessageBox.Show(@"输入格式不正确，正确格式为：SS|25", @"格式错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Match m = Regex.Match(this.comboBoxEx3.Text, @"^([A-Z0-9]+)\|(\d+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            this.comboBoxEx3.Items.Add(new TSessionSetup() { SessionCode = m.Groups[1].Value, Description = string.Format(@"{1} {0}", m.Groups[2].Value, m.Groups[1].Value), SessionPass = m.Groups[2].Value });
            
        }

        /// <summary>
        /// Handles the SelectedTabChanged event of the tabControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DevComponents.DotNetBar.TabStripTabChangedEventArgs"/> instance containing the event data.</param>
        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e) {
            if (PanelSession.Tag == null) return;
            TSessionSetup Setup = PanelSession.Tag as TSessionSetup;
            this.comboTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.comboTree1.ValueMember = @"MonthString";
            this.comboTree1.DisplayMembers = @"MonthString,Traffic,UpdateDate";
            if (AsyncStackNet.Instance.ASyncSetup.SessionCollection[
                AsyncStackNet.Instance.ASyncSetup.SessionCollection.IndexOf(new TSessionSetup(Setup.SessionCode))].Traffics == null)
                return;
            this.comboTree1.DataSource = AsyncStackNet.Instance.ASyncSetup.SessionCollection[
                AsyncStackNet.Instance.ASyncSetup.SessionCollection.IndexOf(new TSessionSetup(Setup.SessionCode))].Traffics;
        }
        
    }
}
